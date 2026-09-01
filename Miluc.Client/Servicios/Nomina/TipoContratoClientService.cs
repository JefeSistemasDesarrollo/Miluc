using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.ContratoLaboralDetalleDto;

using Miluc.Shared.DTOs.Nomina.TipoContratoDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class TipoContratoClientService(HttpClient httpClient) : ITipoContratoClientService
    {
        public async Task<ResponseAPI<List<TipoContratoReaderDto>>> GetTipoContratoAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina)
        {
            try
            {
                // Corregido: Se añade el signo '?' para iniciar los parámetros de la URL correctamente
                var url = $"/api/ContratoLaboral/TipoContrato?busqueda={Uri.EscapeDataString(textoBusqueda ?? string.Empty)}&page={paginaActual}&cantidad={cantidadPorPagina}";

                var responsePeticion = await httpClient.GetAsync(url);

                if (!responsePeticion.IsSuccessStatusCode)
                {
                    return new ResponseAPI<List<TipoContratoReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al obtener los tipos de contrato: {responsePeticion.ReasonPhrase}",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<List<TipoContratoReaderDto>>>();

                if (resultado == null || resultado.Valor == null)
                {
                    return new ResponseAPI<List<TipoContratoReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener los tipos de contrato: respuesta nula",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                return new ResponseAPI<List<TipoContratoReaderDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "Tipos de contrato obtenidos correctamente",
                    Valor = resultado.Valor,
                    CantRegistros = resultado.CantRegistros
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<TipoContratoReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener los tipos de contrato: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }
    }
}









