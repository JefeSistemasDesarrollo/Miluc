using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Sap.Informes.InformeComercial;
using Miluc.Server.Models.Nomina;
using Miluc.Server.Models.Sap;
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


                var facturas = from fac in _sapDbContex.OINV.AsNoTracking()
                               join dtf in _sapDbContex.INV1.AsNoTracking() on fac.DocEntry equals dtf.DocEntry into dtfjoin
                               from dtf in dtfjoin.DefaultIfEmpty()
                               join cli in _sapDbContex.OCRD.AsNoTracking() on fac.CardCode equals cli.CardCode into clijoin
                               from cli in clijoin.DefaultIfEmpty()
                               join can in _sapDbContex.OCRG.AsNoTracking() on cli.GroupCode equals can.GroupCode into canjoin
                               from can in canjoin.DefaultIfEmpty()
                               join venfac in _sapDbContex.OSLP.AsNoTracking() on fac.SlpCode equals venfac.SlpCode into venfacjoin
                               from venfac in venfacjoin.DefaultIfEmpty()
                               join art in _sapDbContex.OITM.AsNoTracking() on dtf.ItemCode equals art.ItemCode into artDte
                               from art in artDte.DefaultIfEmpty()
                               join oitb in _sapDbContex.OITB.AsNoTracking() on art.ItmsGrpCod equals oitb.ItmsGrpCod into artAl
                               from grupo in artAl.DefaultIfEmpty()

                               where fac.CANCELED == 'N' && fac.DocDate >= FechaIncio && fac.DocDate <= FechaFinal
                                     && (GrDistribucion == 0|| can.GroupCode == GrDistribucion)
                               select new ConsolidadoCanalDistribucion
                               {
                                   Vendedor = venfac.SlpName ?? "SIN VENDEDOR",
                                   Articulos = dtf.ItemCode ?? "",
                                   NombreArticulo = dtf.Dscription ?? "",
                                   Cantidad = Convert.ToDecimal(dtf.Quantity),
                                   //UnidadDeMedidaFactura = dtf.unitMsr ?? "",
                                   Peso = Convert.ToDecimal(art.SWeight1),
                                   GrupoInventario = grupo.ItmsGrpNam ?? "",
                                   PesoKilogramos = dtf.unitMsr == "UN" ? Convert.ToDecimal(art.SWeight1) * dtf.Quantity : Convert.ToDecimal(art.SWeight1), // Convertir a kilogramos si la unidad es libras
                                   LineTotalFactura = Convert.ToDecimal(dtf.LineTotal),
                                   //ValorTotalFactura = Convert.ToDecimal(fac.DocTotal),
                                  Canal=can.GroupName
                               };

                var consolidados = facturas
                            .GroupBy(x => new
                            {
                                x.Articulos,
                                x.Vendedor,
                                x.GrupoInventario,
                                x.Canal,
                                
                            })
                            .Select(g => new ConsolidadoCanalDistribucion
                            {
                                Articulos = g.Key.Articulos,
                                NombreArticulo = g.First().NombreArticulo,
                                Vendedor = g.Key.Vendedor,
                                GrupoInventario = g.Key.GrupoInventario,
                                Canal = g.Key.Canal,
                                Cantidad = g.Sum(x => x.Cantidad),
                                Peso = g.Sum(x => x.Peso),
                                PesoKilogramos = g.Sum(x => x.PesoKilogramos),
                                LineTotalFactura = g.Sum(x => x.LineTotalFactura)

                            })
                            .OrderBy(x => x.Vendedor)
                            .ThenBy(x => x.GrupoInventario)
                            .ThenBy(x => x.Articulos)
                            .ThenBy(x=>x.Canal);


                Stream stream = new MemoryStream();

                await MiniExcel.SaveAsAsync(stream, consolidados, printHeader: true, sheetName: "Canal", excelType: ExcelType.XLSX);

                stream.Position = 0;

                return stream;
            }
            catch (Exception ex)
            {
                throw new Exception($" Error  {ex.Message}");
            }
        }
        public async Task<Stream> CarteraVencida(string? CardCode = null, int? CodeVendedor = null, int? CodCanal = null)
        {
            try
            {
                if (CodeVendedor == null)
                    CodeVendedor = -1;

                if (CodCanal == null)
                    CodCanal = 0;

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
                                               TotalDocumento = Convert.ToDecimal(fac.DocTotal),
                                               Pagado = Convert.ToDecimal(fac.PaidToDate),
                                               SaldoPendiente = Convert.ToDecimal(Convert.ToDecimal(fac.DocTotal) - Convert.ToDecimal(fac.PaidToDate)),
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
                        CodigoCliente = cli.CardCode ?? "",
                        NombreCliente = cli.CardName ?? "",
                        Sucursal = cli.CardFName ?? "SIN SUCURSAL",
                        Ruta = ruta.PrioDesc ?? "Sin ruta asignada",
                        Telefono1 = cli.Phone1 ?? "",
                        Telefono2 = cli.Phone2 ?? "",
                        Correo = cli.E_Mail ?? "",
                        Documento = fac.DocNum,
                        VendedorAsignado = vencli.SlpName ?? "Sin cliente asignado",
                        VendedorFactura = venfac.SlpName ?? "Sin cliente asignado",
                        FechaContabilizacion = fac.DocDate,
                        FechaEntrega = fac.DocDueDate,
                        ValorTotal = Convert.ToDecimal(fac.DocTotal)
                    };

                var stream = new MemoryStream();

                await MiniExcel.SaveAsAsync(
                      stream,
                      consulta,
                      printHeader: true,
                      sheetName: "FactHistoCabecerea",
                      excelType: ExcelType.XLSX);

                stream.Position = 0;

                return stream;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error de venta informe {ex.Message}");
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
                        CodigoCliente = cli.CardCode ?? "",
                        NombreCliente = cli.CardName ?? "",
                        Sucursal = cli.CardFName ?? "SIN SUCURSAL",
                        Ruta = ruta.PrioDesc ?? "Sin ruta asignada",
                        Telefono1 = cli.Phone1 ?? "",
                        Telefono2 = cli.Phone2 ?? "",
                        Correo = cli.E_Mail ?? "",
                        DocEntry = fac.DocEntry,
                        Documento = fac.DocNum,
                        VendedorAsignado = vencli.SlpName ?? "No se encontro",
                        VendedorFactura = venfac.SlpName ?? "No se encontro",
                        FechaContabilizacion = fac.DocDate,
                        FechaEntrega = fac.DocDate,
                        Articulos = detfac.ItemCode ?? "",
                        NombreArticulo = detfac.Dscription ?? "",
                        //Vendedor = venfac.SlpName,
                        Cantidad = Convert.ToDecimal(detfac.Quantity),
                        Peso = Convert.ToDecimal(detfac.Width1),
                        LineTotal = Convert.ToDecimal(detfac.LineTotal),
                        UnidadDeMedida = detfac.unitMsr ?? "",
                        ValorTotal = Convert.ToDecimal(fac.DocTotal),
                        GrupoInventario = grupo.ItmsGrpNam ?? ""
                    };


                var stream = new MemoryStream();

                await MiniExcel.SaveAsAsync(
                      stream,
                      consulta,
                      printHeader: true,
                      sheetName: "FactHistoricoDetalle",
                      excelType: ExcelType.XLSX);

                //var resultado = await consulta.AsNoTracking().ToListAsync();

                stream.Position = 0;

                return stream;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error de venta por vendedor {ex.Message}");
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
                        CodigoCliente = cli.CardCode ?? "",
                        NombreCliente = cli.CardName ?? "",
                        Sucursal = cli.CardFName ?? "SIN SUCURSAL",
                        Ruta = ruta.PrioDesc ?? "Sin ruta asignada",
                        Telefono1 = cli.Phone1 ?? "",
                        Telefono2 = cli.Phone2 ?? "",
                        Correo = cli.E_Mail ?? "",
                        Documento = fac.DocNum,
                        VendedorAsignado = vencli.SlpName ?? "",
                        VendedorFactura = venfac.SlpName ?? "",
                        FechaContabilizacion = fac.DocDate,
                        FechaEntrega = fac.DocDate,
                        ValorTotal = Convert.ToDecimal(fac.DocTotal)
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
                    join cli in _sapDbContex.OCRD.AsNoTracking() on fac.CardCode equals cli.CardCode into clijoin
                    from cli in clijoin.DefaultIfEmpty()

                    join detfac in _sapDbContex.INV1.AsNoTracking() on fac.DocEntry equals detfac.DocEntry into detfacjoin
                    from detfac in detfacjoin.DefaultIfEmpty()

                    join art in _sapDbContex.OITM.AsNoTracking() on detfac.ItemCode equals art.ItemCode into artDte
                    from art in artDte.DefaultIfEmpty()
                    join oitb in _sapDbContex.OITB.AsNoTracking() on art.ItmsGrpCod equals oitb.ItmsGrpCod into artAl
                    from grupo in artAl.DefaultIfEmpty()
                        //from cli in clijoin.DefaultIfEmpty()
                    join venfac in _sapDbContex.OSLP.AsNoTracking() on fac.SlpCode equals venfac.SlpCode into venfacjoin
                    from venfac in venfacjoin.DefaultIfEmpty()
                    join vencli in _sapDbContex.OSLP.AsNoTracking() on cli.SlpCode equals vencli.SlpCode into clivende
                    from vencli in clivende.DefaultIfEmpty()
                    join suc in _sapDbContex.OBPP.AsNoTracking() on cli.Priority equals suc.PrioCode into scli
                    from suc in scli.DefaultIfEmpty()

                    join ultFact in ultimaFactura
                        on fac.DocEntry equals ultFact.DocEntry
                    where fac.CANCELED == 'N' && fac.DocDate >= FechaInicio && fac.DocDate <= FechaFinal
                            && (slpCode == -1 || vencli.SlpCode == slpCode)
                    orderby cli.CardCode, fac.DocNum
                    select new FrecuienciaDeVentasReaderDto
                    {
                        CodigoCliente = cli.CardCode ?? "",
                        NombreCliente = cli.CardName ?? "",
                        Sucursal = cli.CardFName ?? "SIN SUCURSAL",
                        Ruta = suc.PrioDesc ?? "Sin ruta asignada",
                        Telefono1 = cli.Phone1 ?? "",
                        Telefono2 = cli.Phone2 ?? "",
                        Correo = cli.E_Mail ?? "",
                        DocEntry = fac.DocEntry,
                        Documento = fac.DocNum,
                        VendedorAsignado = vencli.SlpName ?? "",
                        VendedorFactura = venfac.SlpName ?? "",
                        FechaContabilizacion = fac.DocDate,
                        FechaEntrega = fac.DocDate,
                        Articulos = detfac.ItemCode ?? "",
                        NombreArticulo = detfac.Dscription ?? "",
                        //Vendedor=venfac.SlpName,
                        Cantidad = Convert.ToDecimal(detfac.Quantity),
                        Peso = Convert.ToDecimal(detfac.Width1),
                        LineTotal = Convert.ToDecimal(detfac.LineTotal),
                        UnidadDeMedida = detfac.unitMsr ?? "",
                        ValorTotal = Convert.ToDecimal(fac.DocTotal),
                        GrupoInventario = grupo.ItmsGrpNam ?? "Sin Grupo"
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
        public async Task<Stream> TrazabilidadFacturas(DateTime fechaInicio, DateTime fechaFin, int? currentUserSlpCode = null)
        {
            try
            {
                if (currentUserSlpCode == null)
                    currentUserSlpCode = -1;


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
                    sheetName: "Trazabailidad",
                    excelType: ExcelType.XLSX);
                stream.Position = 0;
                return stream;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Frecuencia de venta por vendedor {ex.Message}");
            }
        }

        public async Task<Stream> TotalVentasPorVendedorDetalle(DateTime FechaIncio, DateTime FechaFinal, int? slpCode = null, int? tipoInforme = null)
        {
            try
            {
                if (slpCode == null)
                    slpCode = -1;
                if (tipoInforme == null)
                    tipoInforme = 1;

                if (tipoInforme == 1)
                {

                    var facturas = from fac in _sapDbContex.OINV.AsNoTracking()

                                   join dtf in _sapDbContex.INV1.AsNoTracking() on fac.DocEntry equals dtf.DocEntry into dtfjoin
                                   from dtf in dtfjoin.DefaultIfEmpty()
                                   join venfac in _sapDbContex.OSLP.AsNoTracking() on fac.SlpCode equals venfac.SlpCode into venfacjoin
                                   from venfac in venfacjoin.DefaultIfEmpty()
                                   join art in _sapDbContex.OITM.AsNoTracking() on dtf.ItemCode equals art.ItemCode into artDte
                                   from art in artDte.DefaultIfEmpty()
                                   join oitb in _sapDbContex.OITB.AsNoTracking() on art.ItmsGrpCod equals oitb.ItmsGrpCod into artAl
                                   from grupo in artAl.DefaultIfEmpty()

                                   where fac.CANCELED == 'N' && fac.DocDate >= FechaIncio && fac.DocDate <= FechaFinal
                                         && (slpCode == -1 || venfac.SlpCode == slpCode)
                                   select new VentasPorVendedorMenosNotasCreditoDto
                                   {
                                       CodigoCliente = fac.CardCode ?? "",
                                       NombreCliente = fac.CardName ?? "",
                                       Vendedor = venfac.SlpName ?? "SIN VENDEDOR",
                                       DocumentoFactura = fac.DocNum,
                                       FechaContabilizacion = fac.DocDate,
                                       FechaEntrega = fac.DocDate,
                                       Articulos = dtf.ItemCode ?? "",
                                       NombreArticulo = dtf.Dscription ?? "",
                                       Cantidad = Convert.ToDecimal(dtf.Quantity),
                                       UnidadDeMedidaFactura = dtf.unitMsr ?? "",
                                       Peso = Convert.ToDecimal(art.SWeight1),
                                       GrupoInventario = grupo.ItmsGrpNam ?? "",
                                       PesoKilogramos = dtf.unitMsr == "UN" ? Convert.ToDecimal(art.SWeight1) * dtf.Quantity : Convert.ToDecimal(art.SWeight1), // Convertir a kilogramos si la unidad es libras
                                       LineTotalFactura = Convert.ToDecimal(dtf.LineTotal),
                                       ValorTotalFactura = Convert.ToDecimal(fac.DocTotal),
                                       TipoDocumento = "Factura"
                                   };
                    var notas = from nota in _sapDbContex.ORIN.AsNoTracking()
                                join dtf in _sapDbContex.RIN1.AsNoTracking() on nota.DocEntry equals dtf.DocEntry into dtfjoin
                                from dtf in dtfjoin.DefaultIfEmpty()
                                join art in _sapDbContex.OITM.AsNoTracking() on dtf.ItemCode equals art.ItemCode into artDte
                                from art in artDte.DefaultIfEmpty()
                                join oitb in _sapDbContex.OITB.AsNoTracking() on art.ItmsGrpCod equals oitb.ItmsGrpCod into artAl
                                from grupo in artAl.DefaultIfEmpty()
                                join venfac in _sapDbContex.OSLP.AsNoTracking() on nota.SlpCode equals venfac.SlpCode into venfacjoin
                                from venfac in venfacjoin.DefaultIfEmpty()
                                where nota.CANCELED == 'N' && nota.DocDate >= FechaIncio && nota.DocDate <= FechaFinal
                                        && (slpCode == -1 || venfac.SlpCode == slpCode)
                                select new VentasPorVendedorMenosNotasCreditoDto
                                {
                                    CodigoCliente = nota.CardCode ?? "",
                                    NombreCliente = nota.CardName ?? "",
                                    Vendedor = venfac.SlpName ?? "SIN VENDEDOR",
                                    DocumentoFactura = nota.DocNum,
                                    FechaContabilizacion = nota.DocDate,
                                    FechaEntrega = nota.DocDate,
                                    Articulos = dtf.ItemCode ?? "",
                                    NombreArticulo = dtf.Dscription ?? "",
                                    Cantidad = Convert.ToDecimal(dtf.Quantity) * -1,
                                    Peso = Convert.ToDecimal(art.SWeight1) * -1,
                                    UnidadDeMedidaFactura = dtf.unitMsr ?? "",
                                    GrupoInventario = grupo.ItmsGrpNam ?? "",
                                    PesoKilogramos = dtf.unitMsr == "UN" ? Convert.ToInt32(art.SWeight1) * dtf.Quantity : Convert.ToInt32(art.SWeight1),
                                    LineTotalFactura = Convert.ToDecimal(dtf.LineTotal * -1),
                                    ValorTotalFactura = Convert.ToDecimal(nota.DocTotal * -1),
                                    TipoDocumento = "Nota de Crédito"
                                };

                    var consulta = facturas.Concat(notas).OrderBy(x => x.CodigoCliente).ThenBy(x => x.DocumentoFactura);

                    Stream stream = new MemoryStream();

                    await MiniExcel.SaveAsAsync(stream, consulta, printHeader: true, sheetName: "Total detalle ", excelType: ExcelType.XLSX);

                    stream.Position = 0;

                    return stream;
                }
                else
                {
                    // var consulta = facturas.Concat(notas).OrderBy(x => x.CodigoCliente).ThenBy(x => x.DocumentoFactura);
                    var facturas = from fac in _sapDbContex.OINV.AsNoTracking()
                                   join dtf in _sapDbContex.INV1.AsNoTracking() on fac.DocEntry equals dtf.DocEntry into dtfjoin
                                   from dtf in dtfjoin.DefaultIfEmpty()
                                   join venfac in _sapDbContex.OSLP.AsNoTracking() on fac.SlpCode equals venfac.SlpCode into venfacjoin
                                   from venfac in venfacjoin.DefaultIfEmpty()
                                   join art in _sapDbContex.OITM.AsNoTracking() on dtf.ItemCode equals art.ItemCode into artDte
                                   from art in artDte.DefaultIfEmpty()
                                   join oitb in _sapDbContex.OITB.AsNoTracking() on art.ItmsGrpCod equals oitb.ItmsGrpCod into artAl
                                   from grupo in artAl.DefaultIfEmpty()

                                   where fac.CANCELED == 'N' && fac.DocDate >= FechaIncio && fac.DocDate <= FechaFinal
                                         && (slpCode == -1 || venfac.SlpCode == slpCode)
                                   select new VentasConsolidadosDto
                                   {
                                       Vendedor = venfac.SlpName ?? "SIN VENDEDOR",
                                        Articulos = dtf.ItemCode ?? "",
                                       NombreArticulo = dtf.Dscription ?? "",
                                       Cantidad = Convert.ToDecimal(dtf.Quantity),
                                       //UnidadDeMedidaFactura = dtf.unitMsr ?? "",
                                       Peso = Convert.ToDecimal(art.SWeight1),
                                       GrupoInventario = grupo.ItmsGrpNam ?? "",
                                       PesoKilogramos = dtf.unitMsr == "UN" ? Convert.ToDecimal(art.SWeight1) * dtf.Quantity : Convert.ToDecimal(art.SWeight1), // Convertir a kilogramos si la unidad es libras
                                       LineTotalFactura = Convert.ToDecimal(dtf.LineTotal),
                                       //ValorTotalFactura = Convert.ToDecimal(fac.DocTotal),
                                       TipoDocumento = "Factura"
                                   };

                    var notas = from nota in _sapDbContex.ORIN.AsNoTracking()
                                join dtf in _sapDbContex.RIN1.AsNoTracking() on nota.DocEntry equals dtf.DocEntry into dtfjoin
                                from dtf in dtfjoin.DefaultIfEmpty()
                                join art in _sapDbContex.OITM.AsNoTracking() on dtf.ItemCode equals art.ItemCode into artDte
                                from art in artDte.DefaultIfEmpty()
                                join oitb in _sapDbContex.OITB.AsNoTracking() on art.ItmsGrpCod equals oitb.ItmsGrpCod into artAl
                                from grupo in artAl.DefaultIfEmpty()
                                join venfac in _sapDbContex.OSLP.AsNoTracking() on nota.SlpCode equals venfac.SlpCode into venfacjoin
                                from venfac in venfacjoin.DefaultIfEmpty()
                                where nota.CANCELED == 'N' && nota.DocDate >= FechaIncio && nota.DocDate <= FechaFinal
                                        && (slpCode == -1 || venfac.SlpCode == slpCode)
                                select new VentasConsolidadosDto
                                {
                                   
                                    Vendedor = venfac.SlpName ?? "SIN VENDEDOR",
                                    Articulos = dtf.ItemCode ?? "",
                                    NombreArticulo = dtf.Dscription ?? "",
                                    Cantidad = Convert.ToDecimal(dtf.Quantity) * -1,
                                    Peso = Convert.ToDecimal(art.SWeight1) * -1,
                                   // UnidadDeMedidaFactura = dtf.unitMsr ?? "",
                                    GrupoInventario = grupo.ItmsGrpNam ?? "",
                                    PesoKilogramos = dtf.unitMsr == "UN" ? Convert.ToInt32(art.SWeight1) * dtf.Quantity : Convert.ToInt32(art.SWeight1),
                                    LineTotalFactura = Convert.ToDecimal(dtf.LineTotal * -1),
                                    //ValorTotalFactura = Convert.ToDecimal(nota.DocTotal * -1),
                                    TipoDocumento = "Nota de Crédito"
                                };
                    var consulta = facturas.Concat(notas).ToList();



                    var consolidados = consulta
                                .GroupBy(x => new
                                {
                                    x.Articulos,
                                    x.Vendedor,
                                    x.GrupoInventario,
                                    x.TipoDocumento,
                                })
                                .Select(g => new VentasConsolidadosDto
                                {
                                    Articulos = g.Key.Articulos,
                                    NombreArticulo = g.First().NombreArticulo,
                                    Vendedor = g.Key.Vendedor,
                                    GrupoInventario = g.Key.GrupoInventario,
                                    TipoDocumento=g.Key.TipoDocumento,
                                    Cantidad = g.Sum(x => x.Cantidad),
                                    Peso = g.Sum(x => x.Peso),
                                    PesoKilogramos = g.Sum(x => x.PesoKilogramos),
                                    LineTotalFactura = g.Sum(x => x.LineTotalFactura)
                                    
                                })
                                .OrderBy(x => x.Vendedor)
                                .ThenBy(x => x.GrupoInventario)
                                .ThenBy(x => x.Articulos);
                   
                    Stream stream = new MemoryStream();

                    await MiniExcel.SaveAsAsync(stream, consolidados, printHeader: true, sheetName: "Total ventas consolidadas", excelType: ExcelType.XLSX);

                    stream.Position = 0;

                    return stream;

                }
            }
            catch (Exception ex)
            {
                throw new Exception($" Error  {ex.Message}");
            }
        }


    }
}
