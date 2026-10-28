namespace Miluc.Server.Interfaces.Sap.Oinv
{
    public interface ISapOinvServices
    {
        Task<int> ObtenerNumeroPedidoDesdeFactura(int docEntry);
    }
}
