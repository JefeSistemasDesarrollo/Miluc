using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Sap.ConexionSap;
using Miluc.Shared.DTOs.Sap.ConexionSapServiceLayer;
using Miluc.Shared.Models.Response;
using Newtonsoft.Json;
using System.Text;


namespace Miluc.Server.Servicios.SapService
{
    public class SapConexionService(MilucDbContext _contex) : IConexionServiceLayer
    {
        public async Task<ResponseAPI<ConexionSapServiceLayerDto>> ConexionSapService()
        {
            try
            {
                var conexion = await _contex.SisConfiguracionesGenerales
                    .Where(x => x.Modulo == "SAP")
                    .Select(x => new ConexionSapServiceLayerDto
                    {
                        URLServiceLayer = x.UrlServiceLayer,
                        UserName = x.UserNameServiceLayer,
                        Password = x.PasswordServiceLayer,
                        CompanyDB = x.CompanyDB,
                        Language = x.LanguageServiceLayer
                    })
                    .FirstOrDefaultAsync();

                if (conexion == null)
                {
                    return new ResponseAPI<ConexionSapServiceLayerDto>
                    {
                        EsCorrecto = false,
                        Mensaje = "No existe configuración SAP.",
                        Valor = null
                    };
                }

                // SOLO enviar datos requeridos por SAP
                var loginRequest = new
                {
                    CompanyDB = conexion.CompanyDB,
                    UserName = conexion.UserName,
                    Password = conexion.Password
                };

                var json =  JsonConvert.SerializeObject(loginRequest);

                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                using var client = new HttpClient(handler);

                client.Timeout = TimeSpan.FromSeconds(60);

                var response = await client.PostAsync(
                    $"{conexion.URLServiceLayer}/Login",
                    new StringContent(json, Encoding.UTF8, "application/json")
                );

                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseAPI<ConexionSapServiceLayerDto>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error SAP: {content}",
                        Valor = null
                    };
                }

                // Obtener cookies
                if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
                {
                    foreach (var cookie in cookies)
                    {
                        if (cookie.StartsWith("B1SESSION"))
                        {
                            conexion.B1SESSION = cookie
                                .Split(';')[0]
                                .Split('=')[1];
                        }

                        if (cookie.StartsWith("ROUTEID"))
                        {
                            conexion.ROUTEID = cookie
                                .Split(';')[0]
                                .Split('=')[1];
                        }
                    }
                }

                return new ResponseAPI<ConexionSapServiceLayerDto>
                {
                    EsCorrecto = true,
                    Mensaje = "Conexión exitosa.",
                    Valor = conexion,
                    CantRegistros = 1
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<ConexionSapServiceLayerDto>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al conectar con SAP: {ex.Message}",
                    Valor = null
                };
            }
        }

        public async Task<ResponseAPI<bool>> LogoutAsync(string urlServiceLayer, string sessionId, string routeId)
        {
            try
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                using var client = new HttpClient(handler);

                client.DefaultRequestHeaders.Add(
                    "Cookie",
                    $"B1SESSION={sessionId}; ROUTEID={routeId}"
                );

                var response = await client.PostAsync(
                    $"{urlServiceLayer}/Logout",
                    null
                );

                return new ResponseAPI<bool>
                {
                    EsCorrecto = response.IsSuccessStatusCode,
                    Valor = response.IsSuccessStatusCode,
                    Mensaje = response.IsSuccessStatusCode
                        ? "Logout exitoso"
                        : "Error cerrando sesión"
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = ex.Message
                };
            }
        }

    }
}
