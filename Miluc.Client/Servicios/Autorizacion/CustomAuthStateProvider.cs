using Microsoft.AspNetCore.Components.Authorization;
using Miluc.Shared.DTOs.Autorizacion;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Miluc.Client.Servicios.Autorizacion
{
    public class CustomAuthStateProvider(HttpClient http) : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var response = await http.GetAsync("api/auth/me");

                if (!response.IsSuccessStatusCode)
                    return new AuthenticationState(_anonymous);

                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<UserSession>>();

                if (result == null || !result.EsCorrecto || result.Valor == null)
                    return new AuthenticationState(_anonymous);

                var userSession = result.Valor;

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userSession.UserName ?? ""),
            new Claim(ClaimTypes.Email, userSession.Email ?? ""),
            new Claim("IdUsuario", userSession.IdUsuario.ToString()),

            // ISO format recomendado
            new Claim("fechaExpiracion", userSession.FechaExpiracion.ToUniversalTime().ToString("o"))
        };

                if (userSession.Roles != null)
                {
                    foreach (var rol in userSession.Roles)
                        claims.Add(new Claim(ClaimTypes.Role, rol));
                }

                if (userSession.Permisos != null)
                {
                    foreach (var permiso in userSession.Permisos)
                        claims.Add(new Claim("Permission", permiso));
                }

                var identity = new ClaimsIdentity(claims, "CookieAuth");

                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
            catch
            {
                return new AuthenticationState(_anonymous);
            }
        }
        //public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        //{
        //    try
        //    {
        //        // 1. Preguntamos a la API por la sesión actual. 
        //        // Es mejor que este endpoint devuelva el DTO con roles y permisos.
        //        var response = await http.GetAsync("api/auth/me");

        //        if (!response.IsSuccessStatusCode)
        //            return new AuthenticationState(_anonymous);

        //        var userSession = await response.Content.ReadFromJsonAsync<UserSession>();

        //        if (userSession == null)
        //            return new AuthenticationState(_anonymous);

        //        // 2. Creamos los Claims reales basados en tu DB
        //        var claims = new List<Claim>
        //        {
        //            new(ClaimTypes.Name, userSession.UserName),
        //            new(ClaimTypes.Email, userSession.Email ?? ""),
        //            new("IdUsuario", userSession.IdUsuario.ToString()),
        //            new("fechaExpiracion", userSession.FechaExpiracion.ToString())
        //        };

        //        // Agregamos Roles
        //        foreach (var rol in userSession.Roles)
        //            claims.Add(new Claim(ClaimTypes.Role, rol));

        //        // Agregamos Permisos (como claims personalizados)
        //        foreach (var permiso in userSession.Permisos)
        //            claims.Add(new Claim("Permission", permiso));

        //        var identity = new ClaimsIdentity(claims, "CookieAuth");
        //        return new AuthenticationState(new ClaimsPrincipal(identity));
        //    }
        //    catch
        //    {
        //        return new AuthenticationState(_anonymous);
        //    }
        //}

        // Se llama al hacer Login exitoso
        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        // Se llama al hacer Logout o por Inactividad

        public void NotifyLogout()
        {
            // Creamos un estado totalmente vacío (Anónimo)
            var anonymousState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

            // Avisamos a Blazor que el estado cambió
            NotifyAuthenticationStateChanged(anonymousState);
        }
    }
}
