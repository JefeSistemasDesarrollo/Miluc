using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Miluc.Client.Interfaces;
using Miluc.Shared.DTOs.Autorizacion;
using Miluc.Shared.DTOs.Usuarios;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Autorizacion
{
    public class AuthFrontService(HttpClient http,AuthenticationStateProvider authStateProvider,NavigationManager navigation) : IAuthClientService
    {
        private readonly HttpClient _http = http;
        public UserSession? CurrentSession { get; private set; }
        private readonly AuthenticationStateProvider _authStateProvider = authStateProvider;
        private readonly NavigationManager _navigation = navigation;


        public async Task InitializeAsync()
        {
            try
            {
                var response = await _http.GetAsync("api/auth/me");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseAPI<UserSession>>();

                    if (result != null && result.EsCorrecto)
                    {
                        CurrentSession = result.Valor;
                    }
                }
                else
                {
                    CurrentSession = null;
                }
            }
            catch
            {
                CurrentSession = null;
            }
        }
        public async Task<ResponseAPI<UserSession>> Login(LoginAccesoRequest loginRequest)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/auth/login", loginRequest);

                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<UserSession>>();

                if (result.Valor != null && result.EsCorrecto)
                {
                    CurrentSession = result.Valor;

                    if (_authStateProvider is CustomAuthStateProvider customProvider)
                    {
                        customProvider.NotifyAuthenticationStateChanged();
                    }

                    return result;
                }

                return result ?? new ResponseAPI<UserSession>
                {
                    EsCorrecto = false,
                    Mensaje = "Error en login"
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<UserSession>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message
                };
            }
        }
        public async Task<ResponseAPI<UserSession>> RefreshSession()
        {
            try
            {
                var response = await _http.PostAsync("api/auth/refresh", null);

                if (!response.IsSuccessStatusCode)
                {
                    await Logout();
                    return new ResponseAPI<UserSession>
                    {
                        EsCorrecto = false,
                        Mensaje = "Refresh inválido"
                    };
                }

                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<UserSession>>();

                if (result != null && result.EsCorrecto)
                {
                    CurrentSession = result.Valor;

                    if (_authStateProvider is CustomAuthStateProvider customProvider)
                    {
                        customProvider.NotifyAuthenticationStateChanged();
                    }

                    return result;
                }

                await Logout();

                return new ResponseAPI<UserSession>
                {
                    EsCorrecto = false,
                    Mensaje = "No se pudo refrescar la sesión"
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<UserSession>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message
                };
            }
        }

        public async Task Logout()
        {
            try
            {
                // 1. Intentamos avisar a la API para que:
                // - Invalide el Refresh Token en SQL (Activo = 0)
                // - Borre las Cookies del navegador
                await _http.PostAsync("api/auth/logout", null);
            }
            catch (Exception ex)
            {
                // Logueamos el error pero no detenemos el proceso
                Console.WriteLine($"Error comunicando con la API para logout: {ex.Message}");
            }
            finally
            {
                // 2. IMPORTANTE: Limpiamos el estado en el Cliente
                // Esto cambia el ClaimsPrincipal a Anónimo instantáneamente
                if (_authStateProvider is CustomAuthStateProvider customProvider)
                {
                    customProvider.NotifyLogout();
                }
                // 3. Redirigimos al Login (Punto 8 de tus reglas)
                _navigation.NavigateTo("/login");
            }
        }

        public bool HasPermission(string permiso)
        {
            if (CurrentSession == null)
                return false;

            if (CurrentSession.Permisos == null)
                return false;

            return CurrentSession.Permisos.Contains(permiso);
        }

        public async Task<ResponseAPI<bool>> CambiarPassword(CambiarPasswordRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/auth/cambiar-password",request);

            return await response.Content
                .ReadFromJsonAsync<ResponseAPI<bool>>()
                ?? new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = "Error del servidor"
                };
        }


        public async Task<ResponseAPI<UserSession>> VerifyOtp(VerifyOtpRequest request)
        {

            try
            {
                var response = await _http.PostAsJsonAsync("api/auth/verify-otp", request);

                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<UserSession>>();

                if (result != null && result.EsCorrecto)
                {
                    CurrentSession = result.Valor;

                    if (_authStateProvider is CustomAuthStateProvider customProvider)
                    {
                        customProvider.NotifyAuthenticationStateChanged();
                    }

                    return result;
                }

                return result ?? new ResponseAPI<UserSession>
                {
                    EsCorrecto = false,
                    Mensaje = "Error al verificar el codigo"
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<UserSession>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message
                };
            }

        }
    }
}
