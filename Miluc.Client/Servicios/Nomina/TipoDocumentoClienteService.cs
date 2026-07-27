using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.NewFolder;
using Miluc.Shared.DTOs.Nomina.TipoDocumento;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class TipoDocumentoClienteService (HttpClient _httpClient) : ITipoDocumentoClienteService
    {
     
        public async Task<ResponseAPI<List<TipoDocumentoReaderDto>>> GetTipoDocumentosAsync()
        {
            try
            {
                var TipoDocumentoClientes = await _httpClient.GetAsync("api/TipoDocumento");

                if (TipoDocumentoClientes.IsSuccessStatusCode)
                {
                    var TipoDocumentoClientesContent = await TipoDocumentoClientes.Content.ReadFromJsonAsync<ResponseAPI<List<TipoDocumentoReaderDto>>>();


                    return TipoDocumentoClientesContent ?? new ResponseAPI<List<TipoDocumentoReaderDto>>()
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener los tipos de documento",
                        CantRegistros = 0
                    };

                }
                else
                {
                    return new ResponseAPI<List<TipoDocumentoReaderDto>>()
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener los tipos de documento",
                        CantRegistros = 0
                    };

                }


            }
            catch (Exception ex)
            {

                return new ResponseAPI<List<TipoDocumentoReaderDto>>()
                {
                    EsCorrecto = false,
                    Mensaje = "Error al obtener los tipos de documento",
                    Errores = new List<string>() { ex.Message },
                    CantRegistros = 0
                };

            }
        }
    }
}
