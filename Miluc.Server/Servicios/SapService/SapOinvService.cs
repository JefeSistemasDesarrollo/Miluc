using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Sap.Oinv;

namespace Miluc.Server.Servicios.SapService
{
    public class SapOinvService(SapDbContex _sapDbContex) : ISapOinvServices
    {


        public async Task<int> ObtenerNumeroPedidoDesdeFactura(int docEntry)
        {
            // Caso 1: la factura se creó desde una entrega
            var numeroPedido = await (
                from fac in _sapDbContex.INV1.AsNoTracking()
                join ent in _sapDbContex.DLN1.AsNoTracking()
                    on new { DocEntry = fac.BaseEntry, LineNum = fac.BaseLine }
                    equals new { DocEntry = (int?)ent.DocEntry, LineNum = (int?)ent.LineNum }
                join ped in _sapDbContex.RDR1.AsNoTracking()
                    on new { DocEntry = ent.BaseEntry, LineNum = ent.BaseLine }
                    equals new { DocEntry = (int?)ped.DocEntry, LineNum = (int?)ped.LineNum }
                join orden in _sapDbContex.ORDR.AsNoTracking()
                    on ped.DocEntry equals orden.DocEntry
                where fac.DocEntry == docEntry
                      && fac.BaseType == 15
                      && ent.BaseType == 17
                select (int?)orden.DocNum
            ).FirstOrDefaultAsync();

            if (numeroPedido.HasValue)
                return Convert.ToInt32(numeroPedido);

            // Caso 2: la factura se creó directamente desde un pedido
            numeroPedido = await (
                from fac in _sapDbContex.INV1.AsNoTracking()
                join ped in _sapDbContex.RDR1.AsNoTracking()
                    on new { DocEntry = fac.BaseEntry, LineNum = fac.BaseLine }
                    equals new { DocEntry = (int?)ped.DocEntry, LineNum = (int?)ped.LineNum }
                join orden in _sapDbContex.ORDR.AsNoTracking()
                    on ped.DocEntry equals orden.DocEntry
                where fac.DocEntry == docEntry
                      && fac.BaseType == 17
                select (int?)orden.DocNum
            ).FirstOrDefaultAsync();

            return Convert.ToInt32(numeroPedido);
        }

    }



}
