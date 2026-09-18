using Miluc.Shared.DTOs.Sap.Devolucion;
using Miluc.Shared.DTOs.Sap.Factura;
using Miluc.Shared.DTOs.Sap.NotaCredito;
using Miluc.Shared.DTOs.Sap.NotaDeEntrega;
using Miluc.Shared.DTOs.Sap.TrazabilidadOrdenesVenta;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.SapInterfaces.TrazabilidadPedidos
{
    public interface ISapTrazabilidadPedidos
    {
        Task<ResponseAPI<TrazabilidadOrdenReaderDto>> ObtenerTrazabilidadOrden(int DocNum);
        Task<ResponseAPI<List<Dln1DetalleReaderDto>>> ObtenerDetalleNota(int DocEntry);
        Task<ResponseAPI<List<Rdn1DetalleDevolucionReaderDto>>> ObtenerDetalleDevolucion(int DocEntry);
        Task<ResponseAPI<List<Inv1DetalleReaderDto>>> ObtenerDetalleFactura(int DocEntry);
        Task<ResponseAPI<List<Rin1DetalleNotaCredito>>> ObtenerDetalleNotaCredito(int DocEntry);

    }
}
