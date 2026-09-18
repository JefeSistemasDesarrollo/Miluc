using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Sap.TrasabilidadOrdendeVenta;
using Miluc.Shared.DTOs.Sap;
using Miluc.Shared.DTOs.Sap.Devolucion;
using Miluc.Shared.DTOs.Sap.Factura;
using Miluc.Shared.DTOs.Sap.InformesComercial;
using Miluc.Shared.DTOs.Sap.NotaCredito;
using Miluc.Shared.DTOs.Sap.NotaDeEntrega;
using Miluc.Shared.DTOs.Sap.TrazabilidadOrdenesVenta;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Servicios.SapService
{
    public class SapTrazabiliadPedidosService(SapDbContex _sapDbContex) : ISapTrazabilidadPedido
    {
        public Task<List<FrecuienciaDeVentasReaderDto>> FrecuenciaPorVendedorDetalle(int DocEntry)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseAPI<List<TrazabilidadOrdenReaderDto>>> ListaTrazabilidad(
       string fechaInicial,
       string fechaFinal)
        {
            try
            {
                var fechaInicio = Convert.ToDateTime(fechaInicial).Date;
                var fechaFin = Convert.ToDateTime(fechaFinal).Date.AddDays(1);

                var trazabilidad = await _sapDbContex.ORDR
                    .AsNoTracking()

                    // ORDENES DE VENTA POR FECHA
                    .Where(x =>
                        x.DocDate >= fechaInicio &&
                        x.DocDate < fechaFin)

                    .Select(x => new TrazabilidadOrdenReaderDto
                    {

                        DocEntry = x.DocEntry,



                        OdlnReader = x.RDR1
                            .SelectMany(rdr1 => rdr1.DLN1)
                            .Select(dln1 => new OdlnReaderDto
                            {
                                DocEntry = dln1.ODLN.DocEntry,
                                DocNum = dln1.ODLN.DocNum,
                                CardCode = dln1.ODLN.CardCode,
                                DocDate = dln1.ODLN.DocDate,
                                DocDueDate = dln1.ODLN.DocDueDate,
                                DocStatus = dln1.ODLN.DocStatus,
                                DocTotal = dln1.ODLN.DocTotal,
                                PaidToDate = dln1.ODLN.PaidToDate,
                                Printed = dln1.ODLN.Printed,

                                // ORDR → RDR1 → DLN1
                                BaseEntry = dln1.BaseEntry
                            })
                            .Distinct()
                            .ToList(),
                        // ORDR → RDR1 → INV1 → OINV
                        OinvOrders = x.RDR1
                            .SelectMany(rdr1 => rdr1.INV1)
                            .Select(inv1 => new OinvReaderDto
                            {
                                DocEntry = inv1.OINV.DocEntry,
                                DocNum = inv1.OINV.DocNum,
                                CardCode = inv1.OINV.CardCode,
                                DocDate = inv1.OINV.DocDate,
                                DocDueDate = inv1.OINV.DocDueDate,
                                DocStatus = inv1.OINV.DocStatus,
                                DocTotal = inv1.OINV.DocTotal,
                                PaidToDate = inv1.OINV.PaidToDate,
                                Printed = inv1.OINV.Printed,

                                // ORDR → RDR1 → INV1
                                BaseEntry = inv1.BaseEntry
                            })
                            .Distinct()
                            .ToList(),

                        // FACTURAS DESDE ENTREGA
                        // ORDR → RDR1 → DLN1 → INV1

                        OinvReader = x.RDR1
                            .SelectMany(rdr1 => rdr1.DLN1)
                            .SelectMany(dln1 => dln1.INV1)
                            .Select(inv1 => new OinvReaderDto
                            {
                                DocEntry = inv1.OINV.DocEntry,
                                DocNum = inv1.OINV.DocNum,
                                CardCode = inv1.OINV.CardCode,
                                DocDate = inv1.OINV.DocDate,
                                DocDueDate = inv1.OINV.DocDueDate,
                                DocStatus = inv1.OINV.DocStatus,
                                DocTotal = inv1.OINV.DocTotal,
                                PaidToDate = inv1.OINV.PaidToDate,
                                Printed = inv1.OINV.Printed,

                                // DLN1 → INV1
                                BaseEntry = inv1.BaseEntry
                            })
                            .Distinct()
                            .ToList(),


                        // DEVOLUCIONES
                        // ORDR → RDR1 → DLN1 → RDN1 → ORDN

                        OrdnReader = x.RDR1
                            .SelectMany(rdr1 => rdr1.DLN1)
                            .SelectMany(dln1 => dln1.RDN1)
                            .Select(rdn1 => new OrdnReaderDto
                            {
                                DocEntry = rdn1.ORDN.DocEntry,
                                DocNum = rdn1.ORDN.DocNum,
                                DocType = rdn1.ORDN.DocType,
                                CANCELED = rdn1.ORDN.CANCELED,
                                Printed = rdn1.ORDN.Printed,
                                DocStatus = rdn1.ORDN.DocStatus,
                                DocDate = rdn1.ORDN.DocDate,
                                DocDueDate = rdn1.ORDN.DocDueDate,
                                CardCode = rdn1.ORDN.CardCode,
                                CardName = rdn1.ORDN.CardName,
                                VatSum = rdn1.ORDN.VatSum,
                                DocTotal = rdn1.ORDN.DocTotal,
                                PaidToDate = rdn1.ORDN.PaidToDate,
                                NumAtCard = rdn1.ORDN.NumAtCard,

                                // DLN1 → RDN1
                                BaseEntry = rdn1.BaseEntry
                            })
                            .Distinct()
                            .ToList(),

                        // NOTAS CRÉDITO
                        // ORDR → RDR1 → DLN1 → INV1
                        // → RIN1 → ORIN
                        orinReaders = x.RDR1
                            .SelectMany(rdr1 => rdr1.DLN1)
                            .SelectMany(dln1 => dln1.INV1)
                            .SelectMany(inv1 => inv1.RIN1)
                            .Select(rin1 => new OrinReaderDto
                            {
                                DocEntry = rin1.ORIN.DocEntry,
                                DocNum = rin1.ORIN.DocNum,
                                DocType = rin1.ORIN.DocType,
                                CANCELED = rin1.ORIN.CANCELED,
                                Printed = rin1.ORIN.Printed,
                                DocStatus = rin1.ORIN.DocStatus,
                                DocDate = rin1.ORIN.DocDate,
                                DocDueDate = rin1.ORIN.DocDueDate,
                                CardCode = rin1.ORIN.CardCode,
                                NumAtCard = rin1.ORIN.NumAtCard,
                                VatSum = rin1.ORIN.VatSum,
                                DocTotal = rin1.ORIN.DocTotal,
                                PaidToDate = rin1.ORIN.PaidToDate,

                                // Factura origen
                                BaseEntry = rin1.BaseEntry
                            })
                            .Distinct()
                            .ToList()
                    })
                    .ToListAsync();

                return new ResponseAPI<List<TrazabilidadOrdenReaderDto>>
                {
                    Valor = trazabilidad,
                    EsCorrecto = true,
                    Mensaje = "Trazabilidad de ventas consultada correctamente.",
                    CantRegistros = trazabilidad.Count
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<TrazabilidadOrdenReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = "Error al consultar la trazabilidad de ventas.",
                    Errores = [ex.Message]
                };
            }
        }

        public Task<ResponseAPI<List<TrazabiliadPorDetalleOrdenReaderDto>>> ListaTrazabilidadExcel(string fechaInicial, string fechaFinal)
        {
            throw new NotImplementedException();
        }

        //public Task<ResponseAPI<List<TrazabiliadPorDetalleOrdenReaderDto>>> ListaTrazabilidadExcel(string fechaInicial, string fechaFinal)
        //{
        //    try
        //    {
        //        var consulta = (from t1 in _sapDbContex.ORDR.AsNoTracking()
        //                        where t1.CANCELED != 'Y' && t1.DocDate >= Convert.ToDateTime(fechaInicial)
        //                                               && t1.DocDate <= Convert.ToDateTime(fechaFinal)
        //                        join t2 in _sapDbContex.RDR1 on t1.DocEntry equals t2.DocEntry

        //                        //left join nota de entrega
        //                        join t3Nota in
        //                        (
        //                            from dln1 in _sapDbContex.DLN1.AsNoTracking()
        //                            join odln in _sapDbContex.ODLN.AsNoTracking()
        //                                on dln1.DocEntry equals odln.DocEntry
        //                            select new
        //                            {
        //                                odln.DocDate,
        //                                odln.DocEntry,
        //                                dln1.BaseEntry,
        //                                odln.DocNum,
        //                                dln1.Quantity,
        //                                dln1.LineNum,
        //                                dln1.ItemCode,
        //                            }
        //                        )
        //                        on new
        //                        { //lo que vamos a devolver 
        //                            BaseEntry = t2.DocEntry,
        //                            t2.ItemCode,
        //                            LineNum = t2.LineNum
        //                        } equals new
        //                        {
        //                            BaseEntry = t3Nota.BaseEntry,
        //                            t3Nota.ItemCode,
        //                            LineNum = t3Nota.LineNum,

        //                        }


        //                         ).ToListAsync();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error al consultar la Trazabilidad de pedidos{ex.Message}");

        //    }
        //}

        public async Task<List<Rdn1DetalleDevolucionReaderDto>> ObtenerDetalleDevolucion(int DocEntry)
        {

            try
            {
                var detalleNotaEntrega = await _sapDbContex.RDN1.AsNoTracking()
                    .Where(x => x.DocEntry == DocEntry).Select(x => new Rdn1DetalleDevolucionReaderDto
                    {
                        DocEntry = x.DocEntry,
                        ItemCode = x.ItemCode,
                        Dscription = x.Dscription,
                        Quantity = x.Quantity,
                        Price = x.Price,
                        UnitMsr = x.UnitMsr,
                        U_UnidadSal = x.U_UnidadSal,
                        U_CantidadKg = x.U_CantidadKg,
                        LineTotal = x.LineTotal,
                        U_Picking = x.U_Picking,
                        WhsCode = x.WhsCode,
                        LineNum = x.LineNum,
                        BaseEntry = x.BaseEntry,

                    }).ToListAsync();

                return detalleNotaEntrega;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<List<Inv1DetalleReaderDto>> ObtenerDetalleFactura(int DocEntry)
        {

            try
            {
                var detalleNotaEntrega = await _sapDbContex.INV1.AsNoTracking()
                    .Where(x => x.DocEntry == DocEntry).Select(x => new Inv1DetalleReaderDto
                    {
                        DocEntry = x.DocEntry,
                        ItemCode = x.ItemCode,
                        Dscription = x.Dscription,
                        Quantity = x.Quantity,
                        LineNum = x.LineNum,
                        Price = x.Price,
                        unitMsr = x.unitMsr,
                        U_UnidadSal = x.U_UnidadSal,
                        U_CantidadKg = x.U_CantidadKg,
                        LineTotal = x.LineTotal,
                        WhsCode = x.WhsCode,
                        U_Picking = x.U_Picking,
                        BaseLine = x.BaseLine,
                        BaseEntry = x.BaseEntry,

                    }).ToListAsync();

                return detalleNotaEntrega;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<List<Rin1DetalleNotaCredito>> ObtenerDetalleNotaCredito(int DocEntry)
        {

            try
            {
                var detalleNotaEntrega = await _sapDbContex.RIN1.AsNoTracking()
                    .Where(x => x.DocEntry == DocEntry).Select(x => new Rin1DetalleNotaCredito
                    {
                        LineNum = x.LineNum,
                        DocEntry = x.DocEntry,
                        ItemCode = x.ItemCode,
                        Dscription = x.Dscription,
                        Quantity = x.Quantity,
                        Price = x.Price,
                        U_UnidadSal = x.U_UnidadSal,
                        U_CantidadKg = x.U_CantidadKg,
                        LineTotal = x.LineTotal,
                        unitMsr = x.unitMsr,
                        WhsCode = x.WhsCode,
                        U_Picking = x.U_Picking,
                        BaseLine = x.BaseLine,
                        BaseEntry = x.BaseEntry


                    }).ToListAsync();

                return detalleNotaEntrega;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<List<Dln1DetalleReaderDto>> ObtenerDetalleNotaDeEntrega(int DocEntry)
        {
            try
            {
                var detalleNotaEntrega = await _sapDbContex.DLN1.AsNoTracking()
                    .Where(x => x.DocEntry == DocEntry).Select(x => new Dln1DetalleReaderDto
                    {
                        DocEntry = x.DocEntry,
                        LineNum = x.LineNum,
                        ItemCode = x.ItemCode,
                        Dscription = x.Dscription,
                        Quantity = x.Quantity,
                        unitMsr = x.unitMsr,
                        U_UnidadSal = x.U_UnidadSal,
                        U_CantidadKg = x.U_CantidadKg,
                        LineTotal = x.LineTotal,
                        Price = x.Price,
                        WhsCode = x.WhsCode,
                        U_Picking = x.U_Picking,
                        //BaseLine=x.BaseLine,
                        //BaseEntry=x.BaseEntry,

                    }).ToListAsync();

                return detalleNotaEntrega;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<TrazabilidadOrdenReaderDto?> ObtenerTrazabilidadOrden(int DocNum)
        {
            try
            {
                //trazabilidad de la orden de venta, notas de entrega, facturas, devoluciones y notas de crédito
                var trazabilidad = await _sapDbContex.ORDR.AsNoTracking()
                    .Where(x => x.DocNum == DocNum)
                    .Select(x => new TrazabilidadOrdenReaderDto
                    {
                        DocEntry = x.DocEntry,
                        OdlnReader = x.RDR1.SelectMany(rdr1 => rdr1.DLN1.Select(dln1 => new OdlnReaderDto
                        {
                            DocEntry = dln1.ODLN.DocEntry,
                            DocNum = dln1.ODLN.DocNum,
                            CardCode = dln1.ODLN.CardCode,
                            DocDate = dln1.ODLN.DocDate,
                            DocDueDate = dln1.ODLN.DocDueDate,
                            DocStatus = dln1.ODLN.DocStatus,
                            DocTotal = dln1.ODLN.DocTotal,
                            PaidToDate = dln1.ODLN.PaidToDate,
                            Printed = dln1.ODLN.Printed,
                            BaseEntry = dln1.BaseEntry,
                        })).ToList(),

                        OinvOrders = x.RDR1
                    .SelectMany(rdr1 => rdr1.INV1
                        .Select(inv1 => new OinvReaderDto
                        {
                            DocEntry = inv1.OINV.DocEntry,
                            DocNum = inv1.OINV.DocNum,
                            DocDate = inv1.OINV.DocDate,
                            CardCode = inv1.OINV.CardCode,
                            DocDueDate = inv1.OINV.DocDueDate,
                            DocStatus = inv1.OINV.DocStatus,
                            DocTotal = inv1.OINV.DocTotal,
                            PaidToDate = inv1.OINV.PaidToDate,
                            Printed = inv1.OINV.Printed,
                            BaseEntry = inv1.BaseEntry,
                        }))
                    .ToList(),

                        OinvReader = x.RDR1.SelectMany(rdr1 => rdr1.DLN1.
                        SelectMany(dln1 => dln1.INV1.Select(inv1 => new OinvReaderDto
                        {
                            DocEntry = inv1.OINV.DocEntry,
                            DocNum = inv1.OINV.DocNum,
                            CardCode = inv1.OINV.CardCode,
                            DocDate = inv1.OINV.DocDate,
                            DocDueDate = inv1.OINV.DocDueDate,
                            DocStatus = inv1.OINV.DocStatus,
                            DocTotal = inv1.OINV.DocTotal,
                            PaidToDate = inv1.OINV.PaidToDate,
                            Printed = inv1.OINV.Printed,
                            BaseEntry = inv1.BaseEntry,
                        }))).ToList(),

                        OrdnReader = x.RDR1.SelectMany(rdr1 => rdr1.DLN1.SelectMany(dln1 => dln1.RDN1.
                        Select(rdn1 => new OrdnReaderDto
                        {
                            DocEntry = rdn1.ORDN.DocEntry,
                            DocNum = rdn1.ORDN.DocNum,
                            DocType = rdn1.ORDN.DocType,
                            CANCELED = rdn1.ORDN.CANCELED,
                            Printed = rdn1.ORDN.Printed,
                            DocStatus = rdn1.ORDN.DocStatus,
                            DocDate = rdn1.ORDN.DocDate,
                            DocDueDate = rdn1.ORDN.DocDueDate,
                            CardCode = rdn1.ORDN.CardCode,
                            CardName = rdn1.ORDN.CardName,
                            VatSum = rdn1.ORDN.VatSum,
                            DocTotal = rdn1.ORDN.DocTotal,
                            PaidToDate = rdn1.ORDN.PaidToDate,
                            NumAtCard = rdn1.ORDN.NumAtCard,
                            BaseEntry = rdn1.BaseEntry
                        }))).ToList(),
                        orinReaders = x.RDR1
                                .SelectMany(rdr1 => rdr1.DLN1
                                    .SelectMany(dln1 => dln1.INV1
                                        .SelectMany(inv1 => inv1.RIN1
                                            .Select(rin1 => new OrinReaderDto
                                            {
                                                DocEntry = rin1.ORIN.DocEntry,
                                                DocNum = rin1.ORIN.DocNum,
                                                DocType = rin1.ORIN.DocType,
                                                CANCELED = rin1.ORIN.CANCELED,
                                                Printed = rin1.ORIN.Printed,
                                                DocStatus = rin1.ORIN.DocStatus,
                                                DocDate = rin1.ORIN.DocDate,
                                                DocDueDate = rin1.ORIN.DocDueDate,
                                                CardCode = rin1.ORIN.CardCode,
                                                NumAtCard = rin1.ORIN.NumAtCard,
                                                VatSum = rin1.ORIN.VatSum,
                                                DocTotal = rin1.ORIN.DocTotal,
                                                PaidToDate = rin1.ORIN.PaidToDate,
                                                BaseEntry = inv1.OINV.DocNum,
                                                //BaseLine = rin1.BaseLine
                                            })
                                        )
                                    )
                                )
                                .ToList()

                    })
                    .FirstOrDefaultAsync();


                return trazabilidad;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la trazabilidad {ex.ToString()}");

            }
        }

    


    
    }

}
