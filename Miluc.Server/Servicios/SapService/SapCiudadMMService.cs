using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Sap.CiudadMM;
using Miluc.Server.Models.Sap;

namespace Miluc.Server.Servicios.SapService
{
    public class SapCiudadMMService(SapDbContex _dbContex) : ISapCiudadMMService
    {
        public async Task<(List<BPCO_MU> Data, int TotalRegistros)> GetAllBPCO_MUAsync(string? buscar = null, int pagina = 1, int? cantidad = null)
        {
            try
            {
                var lista = await _dbContex.BPCO_MU.Where(x => string.IsNullOrEmpty(buscar) || x.Code.Contains(buscar) || x.Name.Contains(buscar))
                    .Skip((pagina - 1) * (cantidad ?? 10))
                    .Take(cantidad ?? 10)
                    .ToListAsync();

                return (lista, lista.Count);
            }
            catch (Exception ex)
            {

                throw new Exception("Error al obtener los datos de BPCO_MU", ex);
            }
        }

    }
}
