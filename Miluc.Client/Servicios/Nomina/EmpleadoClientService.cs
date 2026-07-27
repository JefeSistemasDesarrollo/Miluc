using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.EmpleadoDto;
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

                if (responsePeticion == null)
                {
                    responsePeticion = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                }

                // se deserealiza la respuesta de la petición HTTP a un objeto ResponseAPI<EmpleadoReaderDto>
                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<EmpleadoReaderDto>>();


                return resultado.Valor != null ? resultado : new ResponseAPI<EmpleadoReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "Error al crear el empleado",
                    Valor = null,
                    CantRegistros = 0
                };

            }
            catch (Exception ex)
            {
                return new ResponseAPI<EmpleadoReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al crear el empleado: {ex.Message}",
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
                var response = await httpClient.PutAsJsonAsync(
                    $"api/Empleado/{empleado.EmpleadoId}",
                    empleado
                );

                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseAPI<EmpleadoReaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = await response.Content.ReadAsStringAsync()
                    };
                }

                var result = await response.Content
                    .ReadFromJsonAsync<ResponseAPI<EmpleadoReaderDto>>();

                return result ?? new ResponseAPI<EmpleadoReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "Respuesta vacía del servidor"
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<EmpleadoReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message
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

