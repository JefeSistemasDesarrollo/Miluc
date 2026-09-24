using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Sap.Informes.InformeComercial;
using Miluc.Shared.DTOs.Sap.InformesComercial;
using MiniExcelLibs;

namespace Miluc.Server.Servicios.SapService
{
    public class SapFrecuenciaVentasService(SapDbContex _sapDbContex) : ISapFrecuenciaVentasService
    {
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
                //var ultimaFactura = from fac in _sapDbContex.OINV.AsNoTracking()
                //                    where (fac.CANCELED == 'N' && fac.DocDate >= FechaInicio && fac.DocDate <= FechaFinal)
                //                    group fac by fac.CardCode into grupo
                //                    let ultima = grupo
                //                        .OrderByDescending(x => x.DocDate)
                //                        .ThenByDescending(x => x.DocEntry)
                //                        .First()
                //                    select new
                //                    {
                //                        CardCode = grupo.Key,
                //                        DocEntry = ultima.DocEntry
                //                    };


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
                        CardCode = cli.CardCode,
                        CardName = cli.CardName,
                        CardFName = cli.CardFName ?? "SIN SUCURSAL",
                        PrioDesc = ruta.PrioDesc ?? "Sin Sucursal",
                        Telefono1 = cli.Phone1,
                        Telefono2 = cli.Phone2,
                        Correo = cli.E_Mail,
                        DocNum = fac.DocNum,
                        VendedorAsignado = vencli.SlpName,
                        VendedorFactura = venfac.SlpName,
                        DocDate = fac.DocDate
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
