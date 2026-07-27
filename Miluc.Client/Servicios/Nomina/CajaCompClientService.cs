using Miluc.Client.Interfaces.Nomina.SeguridadSocial;
using Miluc.Shared.DTOs.Nomina.CajaCompensacionDto;
using Miluc.Shared.DTOs.Nomina.EmpleadoDto;
using Miluc.Shared.DTOs.Nomina.EpsDto;
using Miluc.Shared.DTOs.Nomina.MunicipioDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class CajaCompClientService(HttpClient httpClient) : ICajaCompensacionClientService
    {
        public async Task<ResponseAPI<List<CajaCompensacionReaderDto>>> GetCajaCompensacionAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina)
        {
            {

                try
                {
                    var responsePeticion = await httpClient.GetAsync($"/api/CajaCompensacion?filtro={Uri.EscapeDataString(textoBusqueda ?? string.Empty)}&page={paginaActual}&cantidad={cantidadPorPagina}");
                   if (!responsePeticion.IsSuccessStatusCode)
                        return new ResponseAPI<List<CajaCompensacionReaderDto>>
                        {
                            EsCorrecto = false,
                            Mensaje = "Error al obtener las cajas de compensación",
                            Valor = null,
                            CantRegistros = 0
                        };
                    

                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<List<CajaCompensacionReaderDto>>>();

                if (resultado == null || resultado.Valor == null)
                {
                    return new ResponseAPI<List<CajaCompensacionReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener las cajas: respuesta nula",
                        Valor = null,
                        CantRegistros = 0
                    };
                }


                return new ResponseAPI<List<CajaCompensacionReaderDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "Cajas obtenidas correctamente",
                    Valor = resultado.Valor ,
                    CantRegistros = resultado.CantRegistros
                };
            }
                catch (Exception ex)
                {
                    return new ResponseAPI<List<CajaCompensacionReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al obtener las cajas de compensación: {ex.Message}",
                        Valor = null,
                        CantRegistros = 0
                    };
                }
            }
        }
    }
}

