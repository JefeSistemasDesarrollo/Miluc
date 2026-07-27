using Miluc.Shared.DTOs.Nomina.ParentescoDto;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface IParentescoService
    {
        Task <List<ParentescoReaderDto>> GetAllParentescosAsync();
    }
}
