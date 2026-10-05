using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Sap.Informes.InformeComercial;
using Miluc.Shared.DTOs.Sap.InformesComercial;
using MiniExcelLibs;

namespace Miluc.Server.Servicios.SapService
{
    public class SapFrecuenciaVentasService(SapDbContex _sapDbContex) : ISapFrecuenciaVentasService
    {
        public async Task<Stream> CanalDeDistribucion(DateTime FechaIncio, DateTime FechaFinal, int? GrDistribucion = null)
        {
            try
            {
                if (GrDistribucion == null)
                    GrDistribucion = 0;

                var ConsultaCanalDeDistribucion = from fac in _sapDbContex.OINV.AsNoTracking()
                                                  join detFac in _sapDbContex.INV1.AsNoTracking() on fac.DocEntry equals detFac.DocEntry into dtfac
                                                  from detFac in dtfac.DefaultIfEmpty()
                                                  join art in _sapDbContex.OITM.AsNoTracking() on detFac.ItemCode equals art.ItemCode into itDeta
                                                  from art in itDeta.DefaultIfEmpty()
                                                  join oitb in _sapDbContex.OITB.AsNoTracking() on art.ItmsGrpCod equals oitb.ItmsGrpCod into artgru
                                                  from oitb in itDeta.DefaultIfEmpty()
                                                  join cli in _sapDbContex.OCRD.AsNoTracking() on fac.CardCode equals cli.CardCode into clifac
                                                  from cli in clifac.DefaultIfEmpty()
                                                  join gru in _sapDbContex.OBPP.AsNoTracking() on cli.Priority equals gru.PrioCode into cligru
                                                  from gru in cligru.DefaultIfEmpty()
                                                  join can in _sapDbContex.OCRG.AsNoTracking() on cli.GroupCode equals can.GroupCode into cancli
                                                  from can in cancli.DefaultIfEmpty()
                                                  where (fac.CANCELED == 'N' && fac.DocDate <= FechaIncio && fac.DocDate <= FechaFinal && GrDistribucion == '0' && can.GroupCode == GrDistribucion)
                                                  select new CanalDeDistribucionReaderDto
                                                  {
                                                      CodigoCliente = fac.CardCode,
                                                      NombreCliente = fac.CardName,
                                                      Sucursal = cli.CardFName,
                                                      Canal = can.GroupName,
                                                      Documento = fac.DocNum,
                                                      Telefono1 = cli.Phone1,
                                                      Telefono2 = cli.Phone2,
                                                      Correo = cli.E_Mail,
                                                      FechaContabilizacion = fac.DocDate,
                                                      LineTotal = detFac.LineTotal,
                                                      Articulos = detFac.ItemCode,
                                                      NombreArticulo = art.ItemName,
                                                      Cantidad = detFac.Quantity,
                                                      Peso = art.SWeight1,
                                                      UnidadDeMedida = art.BuyUnitMsr

                                                  };

                Stream stream = new MemoryStream();

                await MiniExcel.SaveAsAsync(stream, ConsultaCanalDeDistribucion, printHeader: true, sheetName: "Cana de distribución ", excelType: ExcelType.XLSX);

                stream.Position = 0;

                return stream;
                //await MiniExcel.SaveAsAsync(
                //      stream,
                //      consulta,
                //      printHeader: true,
                //      sheetName: "FrecuenciaVentas",
                //      excelType: ExcelType.XLSX);
            }
            catch (Exception ex)
            {
                throw new Exception($" Error  {ex.Message}");
            }
        }

        public async Task<Stream> CarteraVencida(DateTime FechaFinal, string? CardCode = null, int? CodeVendedor = null, int? CodCanal = null)
        {
            try
            {

                var consultaDiasVencidos = from fac in _sapDbContex.OINV.AsNoTracking()
                                           join cli in _sapDbContex.OCRD.AsNoTracking() on fac.CardCode equals cli.CardCode into clijoin
                                           from cli in clijoin.DefaultIfEmpty()
                                           join ven in _sapDbContex.OSLP.AsNoTracking() on fac.SlpCode equals ven.SlpCode into venjoin
                                           from ven in venjoin.DefaultIfEmpty()
                                           join credito in _sapDbContex.OCTG.AsNoTracking() on cli.GroupNum equals credito.GroupNum into creditojoin
                                           from credito in creditojoin.DefaultIfEmpty()
                                           join can in _sapDbContex.OCRG.AsNoTracking() on cli.GroupCode equals can.GroupCode into canjoin
                                           from can in canjoin.DefaultIfEmpty()
                                           where fac.CANCELED == 'N' 
                                           && fac.DocTotal - fac.PaidToDate > 0
                                           && (CardCode == null || fac.CardName.Contains(CardCode) || fac.CardCode.Contains(CardCode) )
                                           && (CodeVendedor == 0 || ven.SlpCode == CodeVendedor )
                                           && (CodCanal == 0 || can.GroupCode == CodCanal)

                                           orderby fac.DocDueDate
                                           select new CarteraVencidaReaderDto
                                           {
                                               CodigoCliente = fac.CardCode,
                                               NombreCliente = fac.CardName,
                                               Sucursal = cli.CardFName ?? "SIN SUCURSAL",
                                               Canal = can.GroupName ?? "Sin canal asignado",
                                               Vendedor = ven.SlpName ?? "Sin vendedor asignado",
                                               Documento = fac.DocNum,
                                               FechaContabilizacion = fac.DocDate,
                                               FechaVencimiento = fac.DocDueDate,
                                               TotalDocumento = fac.DocTotal,
                                               Pagado = fac.PaidToDate,
                                               SaldoPendiente = fac.DocTotal - fac.PaidToDate,
                                               CondicionPago = credito.PymntGroup ?? "Sin condición de pago",
                                               DiasDeCredito = Convert.ToInt32(credito.ExtraMonth * 30 + credito.ExtraDays ?? 0),
                                               DiasVencidos = EF.Functions.DateDiffDay(fac.DocDueDate, DateTime.Now),
                                               RangoDiasVencidos = EF.Functions.DateDiffDay(fac.DocDueDate, DateTime.Now) < 0 ? "No vencido" :
                                                                   EF.Functions.DateDiffDay(fac.DocDueDate, DateTime.Now) <= 30 ? "1-30 días" :
                                                                   EF.Functions.DateDiffDay(fac.DocDueDate, DateTime.Now) <= 60 ? "31-60 días" :
                                                                   EF.Functions.DateDiffDay(fac.DocDueDate, DateTime.Now) <= 90 ? "61-90 días" :
                                                                   EF.Functions.DateDiffDay(fac.DocDueDate, DateTime.Now) <= 120 ? "90-120 días" : ">120 días"

                                           };



                var stream = new MemoryStream();

                await MiniExcel.SaveAsAsync(
                      stream,
                      consultaDiasVencidos,
                      printHeader: true,
                      sheetName: "CarteraVencida",
                      excelType: ExcelType.XLSX);

                //var resultado = await consulta.AsNoTracking().ToListAsync();

                stream.Position = 0;

                return stream;


                //var stream = new MemoryStream();

                //stream.Position = 0;

                //await MiniExcel.SaveAsAsync(
                //      stream,
                //      consultaDiasVencidos,
                //      printHeader: true,
                //      sheetName: "Cartera Vencida",
                //      excelType: ExcelType.XLSX);

                //return stream;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error Cartera Vencida {ex.Message}");
            }
        }

        public async Task<Stream> FrecuenciaPorVendedorCabeceraFacturas(DateTime FechaInicio, DateTime FechaFinal, int? slpCode = null)
        {
            try
            {
                if (slpCode <= 0)
                    slpCode = -1;
                //consulta para encontrar la ultima fcatura por cada cliente
                var ultimaFactura = from fac in _sapDbContex.OINV.AsNoTracking()
                                    where (fac.CANCELED == 'N' && fac.DocDate >= FechaInicio && fac.DocDate <= FechaFinal)
                                    group fac by fac.CardCode into grupo
                                    select new
                                    {
                                        CardCode = grupo.Key,
                                        DocEntry = grupo.Max(x => x.DocEntry)
                                    };

                var consulta =
                    from fac in _sapDbContex.OINV.AsNoTracking()
                    join cli in _sapDbContex.OCRD.AsNoTracking() on fac.CardCode equals cli.CardCode into clijoin
                    from cli in clijoin.DefaultIfEmpty()
                    join dtfac in _sapDbContex.INV1.AsNoTracking() on fac.DocEntry equals dtfac.DocEntry into detfac
                    from dtfac in detfac.DefaultIfEmpty()
                    join venfac in _sapDbContex.OSLP.AsNoTracking() on fac.SlpCode equals venfac.SlpCode into venfacjoin
                    from venfac in venfacjoin.DefaultIfEmpty()
                    join vencli in _sapDbContex.OSLP.AsNoTracking() on cli.SlpCode equals vencli.SlpCode into clivende
                    from vencli in clivende.DefaultIfEmpty()
                    join suc in _sapDbContex.OBPP.AsNoTracking() on cli.Priority equals suc.PrioCode into scli
                    from ruta in scli.DefaultIfEmpty()

                    join art in _sapDbContex.OITM.AsNoTracking() on dtfac.ItemCode equals art.ItemCode into artDte
                    from art in artDte.DefaultIfEmpty()
                    join oitb in _sapDbContex.OITB.AsNoTracking() on art.ItmsGrpCod equals oitb.ItmsGrpCod into artAl
                    from grupo in artDte.DefaultIfEmpty()
                    join ultFact in ultimaFactura on fac.DocEntry equals ultFact.DocEntry

                    where fac.CANCELED == 'N' && fac.DocDate >= FechaInicio && fac.DocDate <= FechaFinal
                            && (slpCode == -1 || vencli.SlpCode == slpCode)
                    orderby cli.CardCode, fac.DocNum
                    select new FrecuienciaDeVentasReaderDto
                    {
                        CodigoCliente = cli.CardCode,
                        NombreCliente = cli.CardName,
                        Sucursal = cli.CardFName ?? "SIN SUCURSAL",
                        Ruta = ruta.PrioDesc ?? "Sin Ruta",
                        Telefono1 = cli.Phone1,
                        Telefono2 = cli.Phone2,
                        Correo = cli.E_Mail,
                        Documento = fac.DocNum,
                        VendedorAsignado = vencli.SlpName,
                        VendedorFactura = venfac.SlpName,
                        FechaContabilizacion = fac.DocDate,
                        NombreArticulo = dtfac.ItemCode,

                        UnidadDeMedida = dtfac.unitMsr,
                        //Cantidad=dtfac.Quantity,
                        Cantidad = dtfac.unitMsr == "UN" ? dtfac.Quantity * dtfac.Width1 : dtfac.unitMsr == "KG" ? dtfac.Quantity : dtfac.Quantity,
                        GrupoInventario = grupo.ItemName,
                        Peso = dtfac.Width1,
                        ValorTotal = fac.DocTotal,
                    };


                var stream = new MemoryStream();

                await MiniExcel.SaveAsAsync(
                      stream,
                      consulta,
                      printHeader: true,
                      sheetName: "FrecuenciaVentas",
                      excelType: ExcelType.XLSX);

                //var resultado = await consulta.AsNoTracking().ToListAsync();

                stream.Position = 0;

                return stream;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Frecuencia de venta por vendedor detalle y vendedor {ex.Message}");
            }
        }

        public async Task<Stream> FrecuenciaPorVendedorUltimaFactura(DateTime FechaInicio, DateTime FechaFinal, int? slpCode = null)
        {
            try
            {

                if (slpCode <= 0)
                    slpCode = -1;

                //consulta para encontrar la ultima fcatura por cada cliente
                var ultimaFactura = from fac in _sapDbContex.OINV.AsNoTracking()
                                    where (fac.CANCELED == 'N' && fac.DocDate >= FechaInicio && fac.DocDate <= FechaFinal)
                                    group fac by fac.CardCode into grupo
                                    select new
                                    {
                                        CardCode = grupo.Key,
                                        DocEntry = grupo.Max(x => x.DocEntry)
                                    };

                var consulta =
                    from fac in _sapDbContex.OINV.AsNoTracking()
                    join cli in _sapDbContex.OCRD.AsNoTracking()
                        on fac.CardCode equals cli.CardCode into clijoin
                    from cli in clijoin.DefaultIfEmpty()
                    join venfac in _sapDbContex.OSLP.AsNoTracking()
                        on fac.SlpCode equals venfac.SlpCode into venfacjoin
                    from venfac in venfacjoin.DefaultIfEmpty()
                    join vencli in _sapDbContex.OSLP.AsNoTracking()
                        on cli.SlpCode equals vencli.SlpCode into clivende
                    from vencli in clivende.DefaultIfEmpty()
                    join suc in _sapDbContex.OBPP.AsNoTracking()
                        on cli.Priority equals suc.PrioCode into scli
                    from ruta in scli.DefaultIfEmpty()
                    join ultFact in ultimaFactura
                        on fac.DocEntry equals ultFact.DocEntry
                    where fac.CANCELED == 'N' && fac.DocDate >= FechaInicio && fac.DocDate <= FechaFinal
                            && (slpCode == -1 || vencli.SlpCode == slpCode)
                    orderby cli.CardCode, fac.DocNum
                    select new FrecuienciaDeVentasReaderDto
                    {
                        CodigoCliente = cli.CardCode,
                        NombreCliente = cli.CardName,
                        Sucursal = cli.CardFName ?? "SIN SUCURSAL",
                        Ruta = ruta.PrioDesc ?? "Sin ruta asignada",
                        Telefono1 = cli.Phone1,
                        Telefono2 = cli.Phone2,
                        Correo = cli.E_Mail,
                        Documento = fac.DocNum,
                        VendedorAsignado = vencli.SlpName,
                        VendedorFactura = venfac.SlpName,
                        FechaContabilizacion = fac.DocDate
                    };


                var stream = new MemoryStream();

                await MiniExcel.SaveAsAsync(
                      stream,
                      consulta,
                      printHeader: true,
                      sheetName: "FrecuenciaVentas",
                      excelType: ExcelType.XLSX);

                //var resultado = await consulta.AsNoTracking().ToListAsync();

                stream.Position = 0;

                return stream;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Frecuencia de venta por vendedor {ex.Message}");
            }

        }
    }
}
