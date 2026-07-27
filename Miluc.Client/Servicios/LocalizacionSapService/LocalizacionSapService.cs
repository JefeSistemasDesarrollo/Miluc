using Miluc.Client.Interfaces.SapInterfaces.LocalizacionSap;
using Miluc.Shared.DTOs.Sap.ActividadEconomica;
using Miluc.Shared.DTOs.Sap.Cliente.HBT_RESPFISCAL;
using Miluc.Shared.DTOs.Sap.Cliente.RegimeNTributario;
using Miluc.Shared.DTOs.Sap.RegimenFiscal;
using Miluc.Shared.DTOs.Sap.Retenciones;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.LocalizacionSapService
{
    public class LocalizacionSapService(HttpClient _httpClient) : ILocalizacionClientSap
    {
        public async Task<ResponseAPI<(List<HBT_codigosPostalesDto> data, int cantidad)>> GetAllCodigosPostales(string? buscar = null, int? pagina = null, int? cantidad = null)
        {
            try
            {

                //var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7222/api/SapLocalizacionCliente/codigos-postales?buscar=&pagina=1&cantidad=");


                var response = await _httpClient.GetFromJsonAsync<ResponseAPI<List<HBT_codigosPostalesDto>>>($"api/SapLocalizacionCliente/codigos-postales?buscar={buscar}&pagina={pagina}&cantidad={cantidad}");
                if (response.Valor == null)
                {

                    return new ResponseAPI<(List<HBT_codigosPostalesDto> data, int cantidad)>
                    {
                        EsCorrecto = response.EsCorrecto,
                        Valor = (null, 0),
                        Mensaje = response.Mensaje,
                        Errores = response.Errores,
                        CantRegistros = 0
                    };
                    //return new ResponseAPI<List<HBT_codigosPostalesDto>>
                    //{
                    //    EsCorrecto = false,

                    //    Mensaje = "",
                    //    Errores = response.Errores,
                    //    CantRegistros = 0
                    //};
                }
                else
                {
                    return new ResponseAPI<(List<HBT_codigosPostalesDto> data, int cantidad)>
                    {
                        EsCorrecto = response.EsCorrecto,
                        Valor = (response.Valor, response.CantRegistros),
                        Mensaje = response.Mensaje,
                        Errores = response.Errores,
                        CantRegistros = response.CantRegistros
                    };
                }

            }
            catch (Exception ex)
            {
                return new ResponseAPI<(List<HBT_codigosPostalesDto> data, int cantidad)>
                {
                    EsCorrecto = false,
                    Valor = (null, 0),
                    Mensaje = "Error al obtener los códigos postales: " + ex.Message,
                    Errores = new List<string> { ex.Message },
                    CantRegistros = 0
                };
            }
        }

        public async Task<ResponseAPI<(List<HbtMunicipiosDto> data, int cantidad)>> GetAllMunicipiosAsync(string? buscar = null, int? pagina = null, int? cantidad = null)
        {
            try
            {

                var response = await _httpClient.GetFromJsonAsync<ResponseAPI<List<HbtMunicipiosDto>>>($"api/SapLocalizacionCliente/municipios?buscar={buscar}&pagina={pagina}&cantidad={cantidad}");


                if (response == null)
                {
                    return new ResponseAPI<(List<HbtMunicipiosDto> data, int cantidad)>
                    {
                        EsCorrecto = response.EsCorrecto,
                        Valor = (null, 0),
                        Mensaje = response.Mensaje,
                        Errores = response.Errores,
                        CantRegistros = 0
                    };
                }
                else
                {
                    return new ResponseAPI<(List<HbtMunicipiosDto> data, int cantidad)>
                    {
                        EsCorrecto = response.EsCorrecto,
                        Valor = (response.Valor, response.CantRegistros),
                        Mensaje = response.Mensaje,
                        Errores = response.Errores,
                        CantRegistros = response.CantRegistros
                    };
                    //   return ;
                }
            }
            catch (Exception ex)
            {
                return new ResponseAPI<(List<HbtMunicipiosDto> data, int cantidad)>
                {
                    EsCorrecto = false,
                    Valor = (null, 0),
                    Mensaje = "Error al obtener los municipios: " + ex.Message,
                    Errores = new List<string> { ex.Message },
                    CantRegistros = 0
                };


            }
        }

        public async Task<ResponseAPI<List<RegimenTributarioDto>>> GetAllRegimenTributarioAsync()
        {

            try
            {
                var httpResponse = await _httpClient.GetAsync("api/SapLocalizacionCliente/regimen-tributario");
                // 2. Extraemos el JSON sin importar si el código fue 200, 404 o 500
                var resultado = await httpResponse.Content.ReadFromJsonAsync<ResponseAPI<List<RegimenTributarioDto>>>();
                if (httpResponse.IsSuccessStatusCode && resultado != null && resultado.EsCorrecto)
                {
                    // Todo salió bienn 200 
                    return resultado;
                }
                else
                {
                    return new ResponseAPI<List<RegimenTributarioDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = resultado.Mensaje,
                        Errores = resultado.Errores
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<RegimenTributarioDto>>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                };
            }
        }
        public async Task<ResponseAPI<List<OkiResponsabilidadesFiscalesDto>>> GetAllResponsabilidadesFiscales(string? buscar = null, int? pagina = null, int? cantidad = null)
        {
            try
            {


                //var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7222/api/SapLocalizacionCliente/responsabilidades-fiscales?buscar=&pagina=1&cantidad=");

                var response = await _httpClient.GetFromJsonAsync<ResponseAPI<List<OkiResponsabilidadesFiscalesDto>>>($"api/SapLocalizacionCliente/responsabilidades-fiscales?buscar={buscar}&pagina={pagina}&cantidad={cantidad}");

                if (response == null)
                {
                    return new ResponseAPI<List<OkiResponsabilidadesFiscalesDto>>
                    {
                        EsCorrecto = response.EsCorrecto,
                        Valor = response.Valor,
                        Mensaje = response.Mensaje,
                        Errores = response.Errores,
                        CantRegistros = 0
                    };
                }

                return response;



            }
            catch (Exception ex)
            {

                return new ResponseAPI<List<OkiResponsabilidadesFiscalesDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Error al obtener las responsabilidades fiscales: " + ex.Message,
                    Errores = new List<string> { ex.Message },
                    CantRegistros = 0
                };


            }
        }
        public async Task<ResponseAPI<List<TiposDocumentoDto>>> GetAllTiposDocumentoAsync()
        {

            try
            {

                var response = await _httpClient.GetFromJsonAsync<ResponseAPI<List<TiposDocumentoDto>>>("api/SapLocalizacionCliente/tipos-documento");


                if (response.Valor == null)
                {
                    return new ResponseAPI<List<TiposDocumentoDto>>
                    {
                        EsCorrecto = response.EsCorrecto,

                        Mensaje = response.Mensaje,
                        Errores = response.Errores,
                        CantRegistros = 0
                    };
                }
                else
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<TiposDocumentoDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Error al obtener los tipos de documento: " + ex.Message,
                    Errores = new List<string> { ex.Message },
                    CantRegistros = 0
                };

            }
        }
        public async Task<ResponseAPI<List<HBT_RegimenFiscalDto>>> GetRegimenFiscal()
        {
            try
            {

                //var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7222/api/SapLocalizacionCliente/regimen-fiscal");
                var response = await _httpClient.GetFromJsonAsync<ResponseAPI<List<HBT_RegimenFiscalDto>>>("api/SapLocalizacionCliente/regimen-fiscal");


                if (response.Valor == null)
                {
                    return new ResponseAPI<List<HBT_RegimenFiscalDto>>
                    {
                        EsCorrecto = response.EsCorrecto,

                        Mensaje = response.Mensaje,
                        Errores = response.Errores,
                        CantRegistros = 0
                    };
                }
                else
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<HBT_RegimenFiscalDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Error al obtener regimen fiscal: " + ex.Message,
                    Errores = new List<string> { ex.Message },
                    CantRegistros = 0
                };

            }
        }
        public async Task<ResponseAPI<List<RetencionDto>>> GetRetenciones()
        {
            try
            {
                //var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7222/api/SapLocalizacionCliente/retenciones?buscar=&pagina=1&cantidad=");

                var response = await _httpClient.GetFromJsonAsync<ResponseAPI<List<RetencionDto>>>($"api/SapLocalizacionCliente/retenciones");
                if (response.Valor == null)
                {
                    return new ResponseAPI<List<RetencionDto>>
                    {
                        EsCorrecto = false,

                        Mensaje = "",
                        Errores = response.Errores,
                        CantRegistros = 0
                    };
                }
                else
                {
                    return response;
                }

            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<RetencionDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Error al obtener los códigos postales: " + ex.Message,
                    Errores = new List<string> { ex.Message },
                    CantRegistros = 0
                };
            }
        }
        public async Task<ResponseAPI<(List<HbtActividadEconomicaDto> data, int cantidad)>> GetActividadEconomica(string? buscar = null, int? pagina = null, int? cantidad = null)
        {
            try
            {
                //var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7222/api/SapLocalizacionCliente/actividad-economica?buscar=&pagina=&cantidad=");
                if (buscar == null)
                {
                    buscar = "";

                }
                if (pagina == null)
                {
                    pagina = 1;

                }
                if (cantidad == null)
                {
                    cantidad = 10;
                }

                var response = await _httpClient.GetFromJsonAsync<ResponseAPI<List<HbtActividadEconomicaDto>>>($"api/SapLocalizacionCliente/actividad-economica?buscar={buscar}&pagina={pagina}&cantidad={cantidad}");

                if (response.Valor == null)
                {
                    return new ResponseAPI<(List<HbtActividadEconomicaDto> data, int cantidad)>
                    {
                        EsCorrecto = response.EsCorrecto,
                        Valor = (null, 0),
                        Mensaje = response.Mensaje,
                        Errores = response.Errores,
                        CantRegistros = 0
                    };

                }
               
                    return new ResponseAPI<(List<HbtActividadEconomicaDto> data, int cantidad)>
                    {
                        EsCorrecto = response.EsCorrecto,
                        Valor = (response.Valor, response.CantRegistros),
                        Mensaje = response.Mensaje,
                        Errores = response.Errores,
                        CantRegistros = response.CantRegistros
                    };
                


                //var httpResponse = await _httpClient.GetAsync($"api/SapLocalizacionCliente/actividad-economica?buscar={buscar}&pagina={pagina}&cantidad={cantidad}");
                // 2. Extraemos el JSON sin importar si el código fue 200, 404 o 500
                //var resultado = await httpResponse.Content.ReadFromJsonAsync<ResponseAPI<(List<HbtActividadEconomicaDto> data, int cantidad)>>>();

                //    if (httpResponse.IsSuccessStatusCode && resultado != null && resultado.EsCorrecto)
                //{
                //    // Todo salió bienn 200 
                //    return resultado;
                //}
                //else
                //{
                //    return new ResponseAPI<List<HbtActividadEconomicaDto>>
                //    {
                //        EsCorrecto = false,
                //        Mensaje = resultado.Mensaje,
                //        Errores = resultado.Errores

                //    };
                //}
            }
            catch (Exception ex)
            {
                return new ResponseAPI<(List<HbtActividadEconomicaDto> data, int cantidad)>
                {
                    EsCorrecto = false,
                    Valor = (null, 0),
                    Mensaje = "Error al obtener la actividad económica: " + ex.Message,
                    Errores = new List<string> { ex.Message },
                    CantRegistros = 0
                };
            }
        }

        public async Task<ResponseAPI<List<SapResponsabilidadFiscalDto>>> GetResponsabilidadFiscal1(string? buscar = null, int? pagina = null, int? cantidad = null)
        {
            try
            {

                var response = await _httpClient.GetFromJsonAsync<ResponseAPI<List<SapResponsabilidadFiscalDto>>>($"api/SapLocalizacionCliente/responsabilidad-fiscal1?buscar={buscar}&pagina={pagina}&cantidad={cantidad}");

                if (response.Valor == null)
                {
                    return new ResponseAPI<List<SapResponsabilidadFiscalDto>>
                    {
                        EsCorrecto = response.EsCorrecto,
                        Mensaje = response.Mensaje,
                        Errores = response.Errores,
                        CantRegistros = 0
                    };
                }
                else
                {
                    return response;
                }

                //var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7222/api/SapLocalizacionCliente/responsabilidad-fiscal1?buscar=&pagina=&cantidad=");

                ///api/SapLocalizacionCliente/responsabilidad-fiscal1
                //var httpResponse = await _httpClient.GetAsync("api/SapLocalizacionCliente/responsabilidad-fiscal1");
                //var resultado = await httpResponse.Content.ReadFromJsonAsync<ResponseAPI<List<SapResponsabilidadFiscalDto>>>();
                //if (httpResponse.IsSuccessStatusCode && resultado != null && resultado.EsCorrecto)
                //{
                //    return resultado;
                //}
                //else
                //{
                //    return new ResponseAPI<List<SapResponsabilidadFiscalDto>>
                //    {
                //        EsCorrecto = false,
                //        Mensaje = resultado.Mensaje,
                //        Errores = resultado.Errores,
                //    };
                //}

            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<SapResponsabilidadFiscalDto>>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                };
            }

        }
    }
}

