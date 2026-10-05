using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.CargosDto;
using Miluc.Shared.DTOs.Nomina.EmpresaDto;
using Miluc.Shared.DTOs.Nomina.TipoContratoDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class CargoClientService(HttpClient httpClient) : ICargosClientService
    {


        public async Task<ResponseAPI<List<CargosReaderDto>>> GetCargosAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina,string? correo = null)
        {
            try
            {

                var url = $"/api/Cargos/Cargos?filtro={Uri.EscapeDataString(textoBusqueda ?? string.Empty)}&page={paginaActual}&cantidad={cantidadPorPagina} ";

                var responsePeticion = await httpClient.GetAsync(url);
                if (!responsePeticion.IsSuccessStatusCode)
                {
                    return new ResponseAPI<List<CargosReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al obtener los cargos: {responsePeticion.ReasonPhrase}",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<List<CargosReaderDto>>>();

                if (resultado == null || resultado.Valor == null)
                {
                    return new ResponseAPI<List<CargosReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener los cargos: respuesta nula",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                return new ResponseAPI<List<CargosReaderDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "Cargos obtenidos correctamente",
                    Valor = resultado.Valor,
                    CantRegistros = resultado.CantRegistros
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<CargosReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener los Cargos: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }
        public async Task<ResponseAPI<bool>> UpsertCargoAsync(UpsertCargos cargoUpdateDto)
        {
            try
            {
                var responsePeticion = await httpClient.PutAsJsonAsync($"/api/Cargos/{cargoUpdateDto.CargoId}", cargoUpdateDto);
                if (!responsePeticion.IsSuccessStatusCode)
                {
                    return new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al guardar el cargo: {responsePeticion.ReasonPhrase}",
                        Valor = false,
                        CantRegistros = 0
                    };
                }
                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<bool>>();
                if (resultado == null)
                {
                    return new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al guardar el cargo: respuesta nula",
                        Valor = false,
                        CantRegistros = 0
                    };
                }
                return resultado;
            }
            catch (Exception ex)
            {
                return new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al guardar el cargo: {ex.Message}",
                    Valor = false,
                    CantRegistros = 0
                };
            }
        }
        public async Task<ResponseAPI<bool>> DeleteCargoAsync(int id)
        {
            try
            {
                var responsePeticion = await httpClient.DeleteAsync($"/api/Cargos/{id}");
                if (!responsePeticion.IsSuccessStatusCode)
                {
                    return new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al eliminar el cargo: {responsePeticion.ReasonPhrase}",
                        Valor = false,
                        CantRegistros = 0
                    };
                }
                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<bool>>();
                if (resultado == null)
                {
                    return new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al eliminar el cargo: respuesta nula",
                        Valor = false,
                        CantRegistros = 0
                    };
                }
                return resultado;
            }
            catch (Exception ex)
            {
                return new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al eliminar el cargo: {ex.Message}",
                    Valor = false,
                    CantRegistros = 0
                };
            }
        }

        public async Task<ResponseAPI<CargosReaderDto>> GetCargoByIdAsync(int cargoId)
        {
            try
            {
                // 1. Usamos await en lugar de .Result
                // 2. Usamos la ruta REST estándar /api/Cargos/{id}
                var response = await httpClient.GetAsync($"/api/Cargos/{cargoId}");

                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseAPI<CargosReaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error en la solicitud ({response.StatusCode}): {response.ReasonPhrase}",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                var resultado = await response.Content.ReadFromJsonAsync<ResponseAPI<CargosReaderDto>>();

                if (resultado == null)
                {
                    return new ResponseAPI<CargosReaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener cargo: respuesta nula",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                return resultado;
            }
            catch (Exception ex)
            {
                return new ResponseAPI<CargosReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener cargo: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }
    }
    }

