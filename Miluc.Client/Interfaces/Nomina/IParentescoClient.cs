using Miluc.Shared.DTOs.Nomina.ParentescoDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface IParentescoClient
    {
        public Task<ResponseAPI<List<ParentescoReaderDto>>> GetAllParentescosAsync();
    }
}
