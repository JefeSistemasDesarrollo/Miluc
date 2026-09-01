using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.EmpresaDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class EmpresaClienteService(HttpClient httpClient) : IEmpresaClientSevice
    {
        public async Task<ResponseAPI<List<EmpresaReaderDto>>> GetEmpresasAsync()
        { 
            try
            //https://localhost:7222/api/ContratoLaboral/Empresa
            {
                var response = await httpClient.GetAsync($"/api/ContratoLaboral/Empresa");
                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseAPI<List<EmpresaReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error en la solicitud: {response.ReasonPhrase}",
                        Valor = null,
                        CantRegistros = 0
                    };

                }
                var resultado = await response.Content.ReadFromJsonAsync<ResponseAPI<List<EmpresaReaderDto>>>();
                if (resultado == null)
                {
                    return new ResponseAPI<List<EmpresaReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al obtener Empresas : respuesta es nula",
                        Valor = null,
                        CantRegistros = 0
                    };
                }
                return resultado;
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<EmpresaReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al Obtener Empresas{ex.Message}",
                    Valor = null,
                    CantRegistros = 0

                };
            }
        }
    }
}






