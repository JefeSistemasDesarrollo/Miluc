using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Models.Sap;
using Miluc.Shared.DTOs.Sap.InformesComercial;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Miluc.Server.Controllers.SAPController
{
    [ApiController]
    [Route("api/[Controller]")]
    public class PruebasController(SapDbContex dbContex) : Controller
    {

        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            try
            {
                string CardCode = "C860528774";

                string fechaInicial = "2025-08-01";
                string fechaFinal = "2025-08-31";
                //  var fre = await dbContex.OINV.Include(x=>x.INV1).AsNoTracking().ToListAsync();
                int currentUserSlpCode = 3;


                DateTime fechaIni = Convert.ToDateTime(fechaInicial);
                DateTime fechaFin = Convert.ToDateTime(fechaFinal);

                var frecuenciaVentas = await (from fac in dbContex.OINV.AsNoTracking()
                                              join dtfac in dbContex.INV1.AsNoTracking() on fac.DocEntry equals dtfac.DocEntry
                                              join art in dbContex.OITM.AsNoTracking() on dtfac.ItemCode equals art.ItemCode
                                              join oit in dbContex.OITB.AsNoTracking() on art.ItmsGrpCod equals oit.ItmsGrpCod
                                              join ocrd in dbContex.OCRD.AsNoTracking() on fac.CardCode equals ocrd.CardCode
                                              join oslp in dbContex.OSLP.AsNoTracking() on fac.SlpCode equals oslp.SlpCode
                                              join obbp in dbContex.OBPP.AsNoTracking() on ocrd.Priority equals obbp.PrioCode

                                              where fac.DocDate >= fechaIni && fac.DocDate <= fechaFin
                                                && fac.CANCELED == 'N' // Nota: Si en BD es un string/varchar, usa "N". Si es char, usa 'N'
                                                && ocrd.CardType == "C"
                                                && (currentUserSlpCode == -1 || fac.SlpCode == currentUserSlpCode)
                                              select new FrecuienciaDeVentasReaderDto
                                              {
                                                  DocEntry=fac.DocEntry,
                                                  DocNum = fac.DocNum,
                                                  CardCode = fac.CardCode,
                                                  CardName = fac.CardName,
                                                  CardFName = ocrd.CardFName,
                                                  DocDate = fac.DocDate,
                                                  DocDueDate = fac.DocDueDate,
                                                  ItemCode = art.ItemCode,
                                                  ItemName = art.ItemName,
                                                  Quantity = dtfac.Quantity,
                                                  LineTotal = dtfac.LineTotal,
                                                  SWeight1 = art.SWeight1,
                                                  Almacen = oit.ItmsGrpNam,
                                                  SlpName = oslp.SlpName,
                                                  PrioDesc = obbp.PrioDesc,

                                                  // Mapeos adicionales según tu script de SQL (opcional, por si tu DTO los tiene):
                                                  // Telefono1 = ocrd.Phone1,
                                                  // CorreoElectronico = ocrd.E_Mail,
                                                  // TotalFactura = fac.DocTotal
                                              }).ToListAsync();

                //agrupar 
                var agrupar = frecuenciaVentas.GroupBy(x => x.CardCode).Max();

                


                return Ok(frecuenciaVentas);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            
            }
        }
    }
}
