using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.EmpresaDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class EmpresaClienteService(HttpClient httpClient) : IEmpresaClientSevice
    {


        public async Task<ResponseAPI<List<EmpresaReaderDto>>> GetEmpresasAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina, string ? correo = null)
        {
            try
            {
                // 1. Construir la URL base con los parámetros obligatorios de paginación
                var url = $"/api/Empresa/Empresa?paginaActual={paginaActual}&cantidadPorPagina={cantidadPorPagina}";

                // 2. Agregar el texto de búsqueda si no está vacío (Asegúrate de usar el nombre de parámetro que espera tu backend, por ejemplo 'filtro' o 'textoBusqueda')
                if (!string.IsNullOrWhiteSpace(textoBusqueda))
                {
                    url += $"&filtro={Uri.EscapeDataString(textoBusqueda)}";
                }

                // 3. Realizar la petición con la URL completa
                var response = await httpClient.GetAsync(url);

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
                        Mensaje = "Error al obtener Empresas: respuesta es nula",
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
                    Mensaje = $"Error al Obtener Empresas: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }

        public async Task<ResponseAPI<bool>> UpsertEmpresaAsync(UpsertEmpresaDto empresaUpdateDto)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"/api/Empresa/{empresaUpdateDto.EmpresaId}",empresaUpdateDto);

                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error en la solicitud: {response.ReasonPhrase}",
                        Valor = false,
                        CantRegistros = 0
                    };
                }

                var resultado = await response.Content
                    .ReadFromJsonAsync<ResponseAPI<bool>>();

                if (resultado == null)
                {
                    return new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = "La respuesta del servidor es nula.",
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
                    Mensaje = $"Error al guardar la empresa: {ex.Message}",
                    Valor = false,
                    CantRegistros = 0
                };
            }
        }





        public async Task<ResponseAPI<EmpresaReaderDto>> GetEmpresaByIdAsync(int empresaId)
        {
            try
            {
                // 1. Usamos await en lugar de .Result
                // 2. Usamos la ruta REST estándar /api/Empresa/{id}
                var response = await httpClient.GetAsync($"/api/Empresa/{empresaId}");

                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseAPI<EmpresaReaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error en la solicitud ({response.StatusCode}): {response.ReasonPhrase}",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                var resultado = await response.Content.ReadFromJsonAsync<ResponseAPI<EmpresaReaderDto>>();

                if (resultado == null)
                {
                    return new ResponseAPI<EmpresaReaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener empresa: respuesta nula",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                return resultado;
            }
            catch (Exception ex)
            {
                return new ResponseAPI<EmpresaReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener empresa: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }
        public async Task<ResponseAPI<bool>> DeleteEmpresaAsync(int id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"/api/Empresa/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error en la solicitud: {response.ReasonPhrase}",
                        Valor = false,
                        CantRegistros = 0
                    };
                }
                var resultado = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();
                if (resultado == null)
                {
                    return new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al eliminar Empresa : respuesta es nula",
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
                    Mensaje = $"Error al eliminar Empresa{ex.Message}",
                    Valor = false,
                    CantRegistros = 0
                };
            }
        }
    }
}
    


       






