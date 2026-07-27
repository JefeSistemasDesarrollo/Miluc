using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.TipoContratoDto;
using System.Linq.Expressions;

namespace Miluc.Server.Servicios.Nomina
{
    public class TipoContratoService(NominaDbContext _contex) : ITipoContratoService
    {
        public async Task<List<TipoContratoReaderDto>> GetAllTipoContratoAsync()
        {
            try
            {

                var tipoContratos = await _contex.TipoContrato.AsNoTracking()
                .Select(t => new TipoContratoReaderDto

                {
                    TipoContratoId = t.TipoContratoId,
                    NombreContrato = t.NombreContrato
                }).ToListAsync();

                if (tipoContratos == null || tipoContratos.Count == 0)
                {
                    throw new Exception("No se encontraron tipos de contrato.");
                }

                return tipoContratos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener los Tipo de contrato: {ex.Message}", ex);
            }
        }
    }
}

