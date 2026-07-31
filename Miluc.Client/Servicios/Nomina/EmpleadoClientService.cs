using Miluc.Client.Interfaces.Nomina;
using Miluc.Client.Pages.Nomina.Empleado;
using Miluc.Shared.DTOs.Nomina.EmpleadoDto;
using Miluc.Shared.DTOs.Nomina.EpsDto;

/*sing Miluc.Shared.DTOs.Nomina.PaginacionNomina;*/
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class EmpleadoClientService(HttpClient httpClient) : IEmpleadoClientService
    {
        public async Task<ResponseAPI<EmpleadoReaderDto>> createEmpleadosAsync(EmpleadoCreateDto empleado)
        {

            try
            {


                var responsePeticion = await httpClient.PostAsJsonAsync("api/Empleado/crearEmpleado", empleado);

                // Deserializamos la respuesta que viene de la API
                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<EmpleadoReaderDto>>();

                // Si la deserialización fue exitosa, devolvemos la respuesta del servidor (sea éxito o error)
                if (resultado != null)
                {
                    return resultado;
                }

                // Solo si la respuesta vino vacía/nula asignamos un error genérico
                return new ResponseAPI<EmpleadoReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "No se recibió respuesta válida del servidor.",
                    Valor = null,
                    CantRegistros = 0
                };

            }
            catch (Exception ex)
            {
                return new ResponseAPI<EmpleadoReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error de comunicación: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }
        

        




        public async Task<ResponseAPI<EmpleadoReaderDto>> GetEmpleadoByIdAsync(int idempleado)
        {

            try
            {

                ResponseAPI<EmpleadoReaderDto> responseAPI = new ResponseAPI<EmpleadoReaderDto>();

                var response = await httpClient.GetFromJsonAsync<ResponseAPI<EmpleadoReaderDto>>($"api/Empleado/{idempleado}");

                if (response.Valor != null)
                {
                    responseAPI.Valor = response.Valor;
                    responseAPI.Mensaje = response.Mensaje;
                    responseAPI.EsCorrecto = response.EsCorrecto;


                }








                return responseAPI;


            }
            catch (Exception ex)
            {

                return new ResponseAPI<EmpleadoReaderDto>
                {
                    Errores = new List<string> { $"Error {ex}" },
                    Mensaje = ex.Message,
                    EsCorrecto = false


                };

            }

        }

        public async Task<ResponseAPI<EmpleadoReaderDto>> UpdateEmpleadosAsync(EmpleadoUpdateDto empleado)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"api/Empleado/{empleado.EmpleadoId}", empleado);
                var resultado = await response.Content.ReadFromJsonAsync<ResponseAPI<EmpleadoReaderDto>>();

                return resultado ?? new ResponseAPI<EmpleadoReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "No se recibió respuesta del servidor."
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<EmpleadoReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error de red: {ex.Message}"
                };
            }
        }


        public async Task<ResponseAPI<bool>> DeleteEmpleadosAsync(int id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/Empleado/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al eliminar el empleado: {response.ReasonPhrase}",
                        Valor = false
                    };
                }

                // Si la respuesta es exitosa, devolver respuesta positiva
                return new ResponseAPI<bool>
                {
                    EsCorrecto = true,
                    Mensaje = "Empleado eliminado correctamente",
                    Valor = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al eliminar el empleado: {ex.Message}",
                    Valor = false
                };
            }
        }



        public async Task<ResponseAPI<List<EmpleadoReaderDto>>> GetEmpleadosAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina)
        {
            try
            {
                //var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7222/api/Empleado?filtro=&page=1&cantidad=");


                var responsePeticion = await httpClient.GetAsync($"api/Empleado?filtro={textoBusqueda}&page={paginaActual}&cantidad={cantidadPorPagina} ");

                if (!responsePeticion.IsSuccessStatusCode)
                {
                    return new ResponseAPI<List<EmpleadoReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al obtener los empleados: {responsePeticion.ReasonPhrase}",
                        Valor = null,
                        CantRegistros = 0
                    };
                }
                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<List<EmpleadoReaderDto>>>();

                if (resultado == null || resultado.Valor == null)
                {
                    return new ResponseAPI<List<EmpleadoReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener los empleados: respuesta nula",
                        Valor = null,
                        CantRegistros = 0
                    };
                }


                return resultado;




            }
            catch (Exception ex)
            {

                return new ResponseAPI<List<EmpleadoReaderDto>> { EsCorrecto = false, Mensaje = $"Error al obtener los empleados: {ex.Message}", Valor = null, CantRegistros = 0 };
            }
        } } }

