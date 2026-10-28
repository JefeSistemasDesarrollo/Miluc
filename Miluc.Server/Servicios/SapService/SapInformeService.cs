using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Sap.Informes.InformeComercial;
using Miluc.Shared.DTOs.Sap.InformesComercial;
using MiniExcelLibs;

namespace Miluc.Server.Servicios.SapService
{
    public class SapInformeService(SapDbContex _sapDbContex) : ISapInformeService
    {
        public async Task<Stream> CanalDeDistribucion(DateTime FechaIncio, DateTime FechaFinal, int? GrDistribucion = null)
        {
            try
            {
                if (GrDistribucion == null)
                    GrDistribucion = 0;


                var consulta =
                   from fac in _sapDbContex.OINV.AsNoTracking()
                   join cli in _sapDbContex.OCRD.AsNoTracking() on fac.CardCode equals cli.CardCode into clijoin
                   from cli in clijoin.DefaultIfEmpty()
                   join detfac in _sapDbContex.INV1.AsNoTracking() on fac.DocEntry equals detfac.DocEntry into detfacjoin
                   from detfac in detfacjoin.DefaultIfEmpty()
                   join art in _sapDbContex.OITM.AsNoTracking() on detfac.ItemCode equals art.ItemCode into artDte
                   from art in artDte.DefaultIfEmpty()
                   join oitb in _sapDbContex.OITB.AsNoTracking() on art.ItmsGrpCod equals oitb.ItmsGrpCod into artAl
                   from grupo in artAl.DefaultIfEmpty()
                       // join venfac in _sapDbContex.OSLP.AsNoTracking() on fac.SlpCode equals venfac.SlpCode into venfacjoin from venfac in venfacjoin.DefaultIfEmpty()
                   join vencli in _sapDbContex.OSLP.AsNoTracking() on cli.SlpCode equals vencli.SlpCode into clivende
                   from vencli in clivende.DefaultIfEmpty()
                   join suc in _sapDbContex.OBPP.AsNoTracking() on cli.Priority equals suc.PrioCode into scli
                   from suc in scli.DefaultIfEmpty()
                   join can in _sapDbContex.OCRG.AsNoTracking() on cli.GroupCode equals can.GroupCode into canjoin
                   from can in canjoin.DefaultIfEmpty()

                   where fac.CANCELED == 'N' && fac.DocDate >= FechaIncio && fac.DocDate <= FechaFinal
                        && (GrDistribucion == 0 || can.GroupCode == GrDistribucion)
                   //orderby cli.CardCode, fac.DocNum
                   select new CanalDeDistribucionReaderDto
                   {
                       CodigoCliente = cli.CardCode,
                       NombreCliente = cli.CardName,
                       Sucursal = cli.CardFName ?? "SIN SUCURSAL",
                       Canal = can.GroupName ?? "Sin canal asignado",
                       Vendedor = vencli.SlpName ?? "Sin vendedor asignado",
                       Documento = fac.DocNum,
                       FechaContabilizacion = fac.DocDate,
                       FechaEntrega = fac.DocDate,
                       Articulos = detfac.ItemCode,
                       NombreArticulo = detfac.Dscription,
                       Cantidad = detfac.Quantity,
                       Peso = detfac.Width1,
                       LineTotal = detfac.LineTotal,
                       UnidadDeMedida = detfac.unitMsr,
                       ValorTotal = fac.DocTotal,
                       GrupoInventario = grupo.ItmsGrpNam,
                       Telefono1 = cli.Phone1,
                       Telefono2 = cli.Phone2,
                       Correo = cli.E_Mail,
                       VendedorAsignado = vencli.SlpName,

                   };


                Stream stream = new MemoryStream();

                await MiniExcel.SaveAsAsync(stream, consulta, printHeader: true, sheetName: "Canal", excelType: ExcelType.XLSX);

                stream.Position = 0;

                return stream;
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
                                           // Buscar por código o nombre del cliente
                                           && (string.IsNullOrWhiteSpace(CardCode)
                                               || fac.CardCode == CardCode || fac.CardName.Contains(CardCode))

                                               && (CodeVendedor == -1 || ven.SlpCode == CodeVendedor)
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

                stream.Position = 0;

                return stream;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Cartera Vencida {ex.Message}");
            }
        }

        public async Task<Stream> FacturasPorVendedorCabeceraHistoricoFacturas(int? slpCode = null)
        {
            try
            {

                if (slpCode <= 0)
                    slpCode = -1;

                //consulta para encontrar la ultima fcatura por cada cliente
                var ultimaFactura = from fac in _sapDbContex.OINV.AsNoTracking()
                                    where (fac.CANCELED == 'N')
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
                    join venfac in _sapDbContex.OSLP.AsNoTracking() on fac.SlpCode equals venfac.SlpCode into venfacjoin
                    from venfac in venfacjoin.DefaultIfEmpty()
                    join vencli in _sapDbContex.OSLP.AsNoTracking() on cli.SlpCode equals vencli.SlpCode into clivende
                    from vencli in clivende.DefaultIfEmpty()
                    join suc in _sapDbContex.OBPP.AsNoTracking() on cli.Priority equals suc.PrioCode into scli
                    from ruta in scli.DefaultIfEmpty()

                    join ultFact in ultimaFactura on fac.DocEntry equals ultFact.DocEntry
                    where fac.CANCELED == 'N'
                            && (slpCode == -1 || vencli.SlpCode == slpCode)
                    orderby cli.CardCode, fac.DocNum
                    select new FrecuienciaDeVentasReaderDto
                    {
                        DocEntry = fac.DocEntry,
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
                        FechaContabilizacion = fac.DocDate,
                        FechaEntrega = fac.DocDate,
                        ValorTotal = fac.DocTotal
                    };

                var stream = new MemoryStream();

                await MiniExcel.SaveAsAsync(
                      stream,
                      consulta,
                      printHeader: true,
                      sheetName: "FrecHistoricoCabecerea",
                      excelType: ExcelType.XLSX);

                stream.Position = 0;

                return stream;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Frecuencia de venta por vendedor {ex.Message}");
            }
        }

        public async Task<Stream> FacturasPorVendedorCabeceraHistoricoFacturasDetalle(int? slpCode = null)
        {
            try
            {

                if (slpCode <= 0)
                    slpCode = -1;

                //consulta para encontrar la ultima fcatura por cada cliente
                var ultimaFactura = from fac in _sapDbContex.OINV.AsNoTracking()
                                    where (fac.CANCELED == 'N')
                                    group fac by fac.CardCode into grupo
                                    select new
                                    {
                                        CardCode = grupo.Key,
                                        DocEntry = grupo.Max(x => x.DocEntry)
                                    };

                var consulta =
                    from fac in _sapDbContex.OINV.AsNoTracking()
                    join cli in _sapDbContex.OCRD.AsNoTracking() on fac.CardCode equals cli.CardCode into clijoin

                    join detfac in _sapDbContex.INV1.AsNoTracking() on fac.DocEntry equals detfac.DocEntry into detfacjoin
                    from detfac in detfacjoin.DefaultIfEmpty()
                    join art in _sapDbContex.OITM.AsNoTracking() on detfac.ItemCode equals art.ItemCode into artDte
                    from art in artDte.DefaultIfEmpty()
                    join oitb in _sapDbContex.OITB.AsNoTracking() on art.ItmsGrpCod equals oitb.ItmsGrpCod into artAl
                    from grupo in artAl.DefaultIfEmpty()
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
                    where fac.CANCELED == 'N'
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
                        DocEntry = fac.DocEntry,
                        Documento = fac.DocNum,
                        VendedorAsignado = vencli.SlpName,
                        VendedorFactura = venfac.SlpName,
                        FechaContabilizacion = fac.DocDate,
                        FechaEntrega = fac.DocDate,
                        Articulos = detfac.ItemCode,
                        NombreArticulo = detfac.Dscription,
                        //Vendedor = venfac.SlpName,
                        Cantidad = detfac.Quantity,
                        Peso = detfac.Width1,
                        LineTotal = detfac.LineTotal,
                        UnidadDeMedida = detfac.unitMsr,
                        ValorTotal = fac.DocTotal,
                        GrupoInventario = grupo.ItmsGrpNam

                    };


                var stream = new MemoryStream();

                await MiniExcel.SaveAsAsync(
                      stream,
                      consulta,
                      printHeader: true,
                      sheetName: "FrecHistoricoDetalle",
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
        public async Task<Stream> FacturasPorVendedorUltimaFactura(DateTime FechaInicio, DateTime FechaFinal, int? slpCode = null)
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
                        DocEntry = fac.DocEntry,
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
                        FechaContabilizacion = fac.DocDate,
                        FechaEntrega = fac.DocDate,
                        ValorTotal = fac.DocTotal
                    };

                var stream = new MemoryStream();

                await MiniExcel.SaveAsAsync(
                      stream,
                      consulta,
                      printHeader: true,
                      sheetName: "FrecuenciaVentas",
                      excelType: ExcelType.XLSX);

                stream.Position = 0;

                return stream;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Frecuencia de venta por vendedor {ex.Message}");
            }

        }

        public async Task<Stream> FacturasPorVendedorUltimaFacturaDetalle(DateTime FechaInicio, DateTime FechaFinal, int? slpCode = null)
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
                    join cli in _sapDbContex.OCRD.AsNoTracking() on fac.CardCode equals cli.CardCode into clijoin from cli  in clijoin.DefaultIfEmpty()

                    join detfac in _sapDbContex.INV1.AsNoTracking() on fac.DocEntry equals detfac.DocEntry into detfacjoin from detfac in detfacjoin.DefaultIfEmpty()

                    join art in _sapDbContex.OITM.AsNoTracking() on detfac.ItemCode equals art.ItemCode into artDte from art in artDte.DefaultIfEmpty()
                    join oitb in _sapDbContex.OITB.AsNoTracking() on art.ItmsGrpCod equals oitb.ItmsGrpCod into artAl from grupo in artAl.DefaultIfEmpty()
                    //from cli in clijoin.DefaultIfEmpty()
                    join venfac in _sapDbContex.OSLP.AsNoTracking() on fac.SlpCode equals venfac.SlpCode into venfacjoin from venfac in venfacjoin.DefaultIfEmpty()
                    join vencli in _sapDbContex.OSLP.AsNoTracking() on cli.SlpCode equals vencli.SlpCode into clivende from vencli in clivende.DefaultIfEmpty()
                    join suc in _sapDbContex.OBPP.AsNoTracking() on cli.Priority equals suc.PrioCode into scli from suc in scli.DefaultIfEmpty()

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
                        Ruta = suc.PrioDesc ?? "Sin ruta asignada",
                        Telefono1 = cli.Phone1,
                        Telefono2 = cli.Phone2,
                        Correo = cli.E_Mail,
                        DocEntry = fac.DocEntry,
                        Documento = fac.DocNum,
                        VendedorAsignado = vencli.SlpName,
                        VendedorFactura = venfac.SlpName,
                        FechaContabilizacion = fac.DocDate,
                        FechaEntrega = fac.DocDate,
                        Articulos = detfac.ItemCode,
                        NombreArticulo = detfac.Dscription,
                        //Vendedor=venfac.SlpName,
                        Cantidad = detfac.Quantity,
                        Peso = detfac.Width1,
                        LineTotal = detfac.LineTotal,
                        UnidadDeMedida = detfac.unitMsr,
                        ValorTotal = fac.DocTotal,
                        GrupoInventario = grupo.ItmsGrpNam
                    };
                var stream = new MemoryStream();
                await MiniExcel.SaveAsAsync(stream, consulta, printHeader: true, sheetName: "FrecuenciaVentasDetalle", excelType: ExcelType.XLSX);
                //var resultado = await consulta.AsNoTracking().ToListAsync();
                stream.Position = 0;
                return stream;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Frecuencia de venta por vendedor {ex.Message}");
            }
        }
        public async  Task<Stream> TrazabilidadFacturas(DateTime fechaInicio, DateTime fechaFin, int? currentUserSlpCode = null)
        {
            try
            {
                var consulta = await _sapDbContex.Database
                    .SqlQuery<PedidoTrazabilidadDto>($"""
                        EXEC dbo.SPTrazabilidadPedidos
                            @FechaInicio={fechaInicio.Date},
                            @FechaFin={fechaFin.Date},
                            @currentUserSlpCode={currentUserSlpCode}
                        """)
                    .ToListAsync();
                var stream = new MemoryStream();
                await MiniExcel.SaveAsAsync(
                    stream,
                    consulta,
                    printHeader: true,
                    sheetName: "FrecuenciaVentasDetalle",
                    excelType: ExcelType.XLSX);
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
