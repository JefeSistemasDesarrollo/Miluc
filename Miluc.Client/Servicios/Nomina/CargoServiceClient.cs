using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.Cargos;
using Miluc.Shared.DTOs.Nomina.TipoContratoDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class CargoServiceClient(HttpClient httpClient) : ICargos
    {

        public async Task<ResponseAPI<List<CargosDto>>> GetCargosAsyc(string textoBusqueda, int paginaActual, int cantidadPorPagina)
        {
            try
            {
               
                var url = $"api/ContratoLaboral/Cargos?filtro={Uri.EscapeDataString(textoBusqueda ?? string.Empty)}&page={paginaActual}&cantidad={cantidadPorPagina}";

                var responsePeticion = await httpClient.GetAsync(url);
                if (!responsePeticion.IsSuccessStatusCode)
                {
                    return new ResponseAPI<List<CargosDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al obtener los cargos: {responsePeticion.ReasonPhrase}",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<List<CargosDto>>>();

                if (resultado == null || resultado.Valor == null)
                {
                    return new ResponseAPI<List<CargosDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener los cargos: respuesta nula",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                return new ResponseAPI<List<CargosDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "Cargos obtenidos correctamente",
                    Valor = resultado.Valor,
                    CantRegistros = resultado.CantRegistros
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<CargosDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener los Cargos: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }
    }
    }
