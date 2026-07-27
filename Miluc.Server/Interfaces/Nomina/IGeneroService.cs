using System.Collections.Generic;
using System.Threading.Tasks;
using Miluc.Shared.DTOs.Nomina.GeneroDto;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface IGeneroService
    {
        Task<List<GeneroReaderDto>> GetGeneroAsync();
    }
}