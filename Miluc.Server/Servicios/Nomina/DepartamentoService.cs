using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Shared.DTOs.Nomina.NewFolder;

namespace Miluc.Server.Servicios.Nomina
{
    public class DepartamentoService(NominaDbContext _contex) : IDepartamentoService
    {
        public async Task<List<DepartamentoReaderDto>> GetDepartamentosAsync()
        {
            try
            {
                var departamentos = await _contex.Departamento.AsNoTracking()
                .Select(d => new DepartamentoReaderDto
                {
                    Codigo = d.Codigo,
                    Nombre = d.Nombre,
                    Activo = d.Activo
                }).ToListAsync();
                return departamentos;

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los departamentos", ex);
            }
        }
    }
}
