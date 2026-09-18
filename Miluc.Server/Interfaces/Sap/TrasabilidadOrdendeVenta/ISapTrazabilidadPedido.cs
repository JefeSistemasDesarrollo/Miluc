using Miluc.Shared.DTOs.Sap;
using Miluc.Shared.DTOs.Sap.Devolucion;
using Miluc.Shared.DTOs.Sap.Factura;
using Miluc.Shared.DTOs.Sap.InformesComercial;
using Miluc.Shared.DTOs.Sap.NotaCredito;
using Miluc.Shared.DTOs.Sap.NotaDeEntrega;
using Miluc.Shared.DTOs.Sap.TrazabilidadOrdenesVenta;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Interfaces.Sap.TrasabilidadOrdendeVenta
{
    public interface ISapTrazabilidadPedido
    {
        Task<TrazabilidadOrdenReaderDto> ObtenerTrazabilidadOrden(int DocEntry);

        Task<ResponseAPI<List<TrazabilidadOrdenReaderDto>>> ListaTrazabilidad(string fechaInicial,string fechaFinal);
        Task<ResponseAPI<List<TrazabiliadPorDetalleOrdenReaderDto>>> ListaTrazabilidadExcel(string fechaInicial,string fechaFinal);


        Task<List<Dln1DetalleReaderDto>> ObtenerDetalleNotaDeEntrega(int DocEntry);
        Task<List<Rdn1DetalleDevolucionReaderDto>> ObtenerDetalleDevolucion(int DocEntry);
        Task<List<Inv1DetalleReaderDto>> ObtenerDetalleFactura(int DocEntry);
        Task<List<Rin1DetalleNotaCredito>> ObtenerDetalleNotaCredito(int DocEntry);

        //frecuencia de ventas 
        Task<List<FrecuienciaDeVentasReaderDto>> FrecuenciaPorVendedorDetalle(int DocEntry);
    }
}
