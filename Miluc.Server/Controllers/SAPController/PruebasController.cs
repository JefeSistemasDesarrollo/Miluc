using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Models.Sap;
using Miluc.Shared.DTOs.Sap.InformesComercial;
using MiniExcelLibs;

namespace Miluc.Server.Controllers.SAPController
{
    [ApiController]
    [Route("api/[Controller]")]
    public class PruebasController(SapDbContex _context) : Controller
    {

        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            try
            {
                //string CardCode = "C860528774";

                string fechaInicial = "2026-05-01";
                string fechaFinal = "2026-05-20";
                ////  var fre = await dbContex.OINV.Include(x=>x.INV1).AsNoTracking().ToListAsync();
                //int currentUserSlpCode = 3;


                DateTime fechaIni = Convert.ToDateTime(fechaInicial);
                DateTime fechaFin = Convert.ToDateTime(fechaFinal);


                //var fechaInicial = new DateTime(2025, 8, 1);
                //var fechaFinal = new DateTime(2025, 9, 1);
                var slpCode = -1;



                var ultimaFactura =
                    from fac in _context.OINV
                    where fac.CANCELED == 'N'
                          && fac.DocDate >= fechaIni
                          && fac.DocDate <= fechaFin
                    group fac by fac.CardCode into grupo
                    select new
                    {
                        CardCode = grupo.Key,
                        DocEntry = grupo.Max(x => x.DocEntry)
                    };

                var consulta =
                    from fac in _context.OINV

                    join cli in _context.OCRD
                        on fac.CardCode equals cli.CardCode into cliJoin
                    from cli in cliJoin.DefaultIfEmpty()

                    join venFact in _context.OSLP
                        on fac.SlpCode equals venFact.SlpCode into venFactJoin
                    from venFact in venFactJoin.DefaultIfEmpty()

                    join venCli in _context.OSLP
                        on cli.SlpCode equals venCli.SlpCode into venCliJoin
                    from venCli in venCliJoin.DefaultIfEmpty()

                    join suc in _context.OBPP
                        on cli.Priority equals suc.PrioCode into sucJoin
                    from suc in sucJoin.DefaultIfEmpty()

                    join ultima in ultimaFactura
                        on fac.DocEntry equals ultima.DocEntry

                    where fac.CANCELED == 'N'
                          && fac.DocDate >= fechaIni
                          && fac.DocDate <= fechaFin
                          && (slpCode == -1 || venCli.SlpCode == slpCode)

                    orderby cli.CardCode, fac.DocDate

                    select new FrecuienciaDeVentasReaderDto
                    {
                        CardCode = cli.CardCode,
                        CardName= cli.CardName,
                        CardFName= cli.CardFName ?? "SIN SUCURSAL",
                         PrioDesc= suc.PrioDesc,
                        Telefono1 = cli.Phone1,
                        Telefono2 = cli.Phone2,

                        DocNum = fac.DocNum,
                       // VendedorFactura = venFact.SlpName,
                        SlpName= venCli.SlpName,
                        DocDate= fac.DocDate,
                        ValorTotal = fac.DocTotal
                    };

                var resultado = await consulta
                    .AsNoTracking()
                    .ToListAsync();






                return Ok(resultado);



                //var frecuenciaVentas = await (from fac in dbContex.OINV.AsNoTracking()
                //                              join dtfac in dbContex.INV1.AsNoTracking() on fac.DocEntry equals dtfac.DocEntry
                //                              join art in dbContex.OITM.AsNoTracking() on dtfac.ItemCode equals art.ItemCode
                //                              join oit in dbContex.OITB.AsNoTracking() on art.ItmsGrpCod equals oit.ItmsGrpCod
                //                              join ocrd in dbContex.OCRD.AsNoTracking() on fac.CardCode equals ocrd.CardCode
                //                              join oslp in dbContex.OSLP.AsNoTracking() on fac.SlpCode equals oslp.SlpCode
                //                              join obbp in dbContex.OBPP.AsNoTracking() on ocrd.Priority equals obbp.PrioCode




                //                              where fac.DocDate >= fechaIni && fac.DocDate <= fechaFin
                //                                && fac.CANCELED == 'N' // Nota: Si en BD es un string/varchar, usa "N". Si es char, usa 'N'
                //                                && ocrd.CardType == "C"
                //                                && (currentUserSlpCode == -1 || fac.SlpCode == currentUserSlpCode)
                //                                select new FrecuienciaDeVentasReaderDto
                //                                  {

                //                                      DocEntry=fac.DocEntry,
                //                                      DocNum = fac.DocNum,
                //                                      CardCode = fac.CardCode,
                //                                      CardName = fac.CardName,
                //                                      CardFName = ocrd.CardFName,
                //                                      DocDate = fac.DocDate,
                //                                      DocDueDate = fac.DocDueDate,
                //                                      ItemCode = art.ItemCode,
                //                                      ItemName = art.ItemName,
                //                                      Quantity = dtfac.Quantity,
                //                                      LineTotal = dtfac.LineTotal,
                //                                      SWeight1 = art.SWeight1,
                //                                      Almacen = oit.ItmsGrpNam,
                //                                      SlpName = oslp.SlpName,
                //                                      PrioDesc = obbp.PrioDesc,

                //                                      // Mapeos adicionales según tu script de SQL (opcional, por si tu DTO los tiene):
                //                                      // Telefono1 = ocrd.Phone1,
                //                                      // CorreoElectronico = ocrd.E_Mail,
                //                                      // TotalFactura = fac.DocTotal
                //                                  }).ToListAsync();





                ////agrupar 


                ////var con=await frecuenciaVentas.GroupBy


                //return Ok(frecuenciaVentas);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            
            }
        }
    }
}
