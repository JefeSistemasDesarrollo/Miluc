using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Sap.Octg;
using Miluc.Shared.DTOs.Sap.Credito;

namespace Miluc.Server.Servicios.SapService
{
    public class SapOctgService (SapDbContex sapDbContex) : ISapOctgService
    {
        public async Task<List<DiasCreditoDto>> GetAllDiasCreditoAsync()
        {
            try
            {

                var diasCredito = await sapDbContex.OCTG
                    .Select(x => new DiasCreditoDto
                    {
                        GroupNum = x.GroupNum,
                        PymntGroup = x.PymntGroup
                    }).ToListAsync();   

                if (diasCredito == null)
                {
                    throw new Exception("No se encontraron días de crédito");
                }


                return diasCredito;


            }
            catch (Exception ex) 
            {

                // Manejar la excepción, por ejemplo, registrándola o lanzándola nuevamente
                throw new Exception("Error al obtener los días de crédito", ex);

            }
        }
    }
}
