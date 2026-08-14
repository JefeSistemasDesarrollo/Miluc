using Miluc.Shared.DTOs.Sap.Cliente;

namespace Miluc.Server.Interfaces.Sap.Ocrd
{
    public interface ISapOcrdService
    {
        public Task<(List<SapClienteReaderDto> Data, int TotalRegistros)> GetallClienteAsync(string ? buscar= null, int ?pagina=null,int ? cantidad=null, int ? codVendedorSAP=null);

        public Task<SapClienteReaderDto> GetByIdClienteAsync(string ? cardcode=null );


        public Task<SapClienteReaderDto> CreateClienteAsync(SapClienteCreateEditDto sapClienteCreateDto);
         
        public Task<SapClienteReaderDto> EditarClienteAsunc(SapClienteCreateEditDto sapClienteEditDto);
        public Task<bool> EliminarClienteAsync(string cardCode);
    }
}
