using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.Autorizacion;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Shared.DTOs.Autorizacion;
using Miluc.Shared.DTOs.Usuarios;
using Miluc.Shared.Models.Response;
using System.Security.Claims;

namespace Miluc.Server.Controllers.AuthController
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuthController(IAuthService authService, IConfiguration config, ILogService log) : Controller
    {
        [HttpGet("me")]

        public async Task<ActionResult<ResponseAPI<UserSession>>> GetCurrentUser()
        {
            try
            {
                // 1. Obtener ticket de autenticación
                var authResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                if (!authResult.Succeeded)
                {
                    return Unauthorized(new ResponseAPI<UserSession>
                    {
                        EsCorrecto = false,
                        Mensaje = "Sesión no válida o expirada"
                    });
                }
                // 2. Extraer expiración real de la cookie
                var expiraUtc = authResult.Properties?.ExpiresUtc?.UtcDateTime ?? DateTime.MinValue;

                // 3. Construir sesión desde los Claims
                var userSession = new UserSession
                {
                    IdUsuario = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0"),
                    UserName = User.Identity?.Name ?? "Desconocido",
                    Email = User.FindFirst(ClaimTypes.Email)?.Value ?? "",
                    Roles = User.FindAll(ClaimTypes.Role)
                                .Select(c => c.Value)
                                .ToList(),
                    Permisos = User.FindAll("Permission").Select(c => c.Value).ToList(),
                    FechaExpiracion = expiraUtc
                };

                return Ok(new ResponseAPI<UserSession>
                {
                    EsCorrecto = true,
                    Mensaje = "Usuario autenticado",
                    Valor = userSession
                });
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                          message: ex.Message,
                          StackTrace: ex.StackTrace,
                          usuario: User.Identity?.Name ?? "Sistema",
                          metodo: "HttpGet",
                          ruta: "/api/AuthController/me",
                          ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                          origen: $"AuthController.me");
                return StatusCode(500, new ResponseAPI<UserSession>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<ResponseAPI<UserSession>>> Login([FromBody] LoginAccesoRequest request)
        {
            try
            {
                string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                string userAgent = Request.Headers["User-Agent"].ToString();

                var session = await authService.LoginAsync(request, ip, userAgent);

                if (!session.EsCorrecto || session.Valor == null)
                {
                    return Unauthorized(new ResponseAPI<UserSession>
                    {
                        EsCorrecto = false,
                        Mensaje = "Usuario o contraseña incorrectos"
                    });
                }

                // Si requiere 2FA NO crear cookies todavía
                if (session.Valor.Requiere2FA)
                    return Ok(session);

                if(session.Valor.DebeCambiarPassword)
                    return Ok(session);

                await CrearCookies(session.Valor);

                session.Valor.RefreshToken = null;

                return Ok(session);
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: "Sistema",
                    metodo: "HttpPost",
                    ruta: "/api/Auth/login",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "AuthController.Login");

                return StatusCode(500, new ResponseAPI<UserSession>
                {
                    EsCorrecto = false,
                    Mensaje = "Error interno"
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("verify-otp")]
        public async Task<ActionResult<ResponseAPI<UserSession>>> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            try
            {
                string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                string userAgent = Request.Headers["User-Agent"].ToString();

                var response = await authService.VerifyOtpAsync(
                    request.IdUsuario,
                    request.Codigo,
                    ip,
                    userAgent);

                if (!response.EsCorrecto || response.Valor == null)
                {
                    return Unauthorized(response);
                }

                await CrearCookies(response.Valor);

                response.Valor.RefreshToken = null;

                return Ok(response);
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: "Sistema",
                    metodo: "HttpPost",
                    ruta: "/api/Auth/verify-otp",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "AuthController.VerifyOtp");

                return StatusCode(500, new ResponseAPI<UserSession>
                {
                    EsCorrecto = false,
                    Mensaje = "Error interno"
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("cambiar-password")]
        public async Task<ActionResult<ResponseAPI<bool>>> CambiarPassword(
    [FromBody] CambiarPasswordRequest request)
        {
            try
            {
                var result = await authService.CambiarPasswordAsync(request);

                if (!result.EsCorrecto)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: "Sistema",
                    metodo: "HttpPost",
                    ruta: "/api/auth/cambiar-password",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "AuthController.CambiarPassword");

                return StatusCode(500, new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = "Error interno del servidor"
                });
            }
        }

        private async Task CrearCookies(UserSession session)
        {
            int accessMinutes = config.GetValue<int>("Jwt:AccessTokenExpirationMinutes");
            int refreshMinutes = config.GetValue<int>("Jwt:RefreshTokenExpiration");

            session.FechaExpiracion = DateTime.UtcNow.AddMinutes(accessMinutes);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, session.IdUsuario.ToString()),
                new(ClaimTypes.Name, session.UserName),
                new(ClaimTypes.Email, session.Email ?? ""),
                new("IdUsuario", session.IdUsuario.ToString())
            };

            session.Roles.ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));
            session.Permisos.ForEach(p => claims.Add(new Claim("Permission", p)));

        await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme)),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = session.FechaExpiracion
                });

            Response.Cookies.Append("OpcionErp_RefreshToken",
                session.RefreshToken!,
                new CookieOptions
                {
                    //Domain = "dominio.com",
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(refreshMinutes)
                });
        }

        private List<Claim> BuildClaims(UserSession session)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, session.IdUsuario.ToString()),
                new(ClaimTypes.Name, session.UserName),
                new(ClaimTypes.Email, session.Email ?? ""),
                new("IdUsuario", session.IdUsuario.ToString()),
                new Claim("fechaExpiracion", session.FechaExpiracion.ToUniversalTime().ToString("o"))
            };
            session.Roles.ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));
            session.Permisos.ForEach(p => claims.Add(new Claim("Permission", p)));
            return claims;
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<ResponseAPI<UserSession>>> Refresh()
        {
            try
            {
                var oldRefreshToken = Request.Cookies["OpcionErp_RefreshToken"];

                if (string.IsNullOrEmpty(oldRefreshToken))
                {
                    return Unauthorized(new ResponseAPI<UserSession>
                    {
                        EsCorrecto = false,
                        Mensaje = "Refresh token no encontrado"
                    });
                }

                string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                string ua = Request.Headers["User-Agent"].ToString();

                // Servicio
                var response = await authService.RefreshTokenAsync(oldRefreshToken, ip, ua);

                if (response == null || !response.EsCorrecto || response.Valor == null)
                {
                    return Unauthorized(new ResponseAPI<UserSession>
                    {
                        EsCorrecto = false,
                        Mensaje = "Refresh token inválido o expirado"
                    });
                }

                int accessMinutes = config.GetValue<int>("Jwt:AccessTokenExpirationMinutes");
                int refreshMinutes = config.GetValue<int>("Jwt:RefreshTokenExpiration");
               response.Valor.FechaExpiracion = DateTime.UtcNow.AddMinutes(accessMinutes);
                // Claims nuevos
                var claims = BuildClaims(response.Valor);
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                // Cookie identidad
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = response.Valor.FechaExpiracion
                    });
                // Cookie refresh (rotación)
                Response.Cookies.Append("OpcionErp_RefreshToken", response.Valor.RefreshToken!, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(refreshMinutes)
                });
                // Seguridad
                response.Valor.RefreshToken = null;

                return Ok(new ResponseAPI<UserSession>
                {
                    EsCorrecto = true,
                    Mensaje = "Token renovado correctamente",
                    Valor = response.Valor
                });
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                         message: ex.Message,
                          StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: "HttpPost",
                        ruta: "/api/Authcontroller",
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: $"HttpPost.refresh");
                return StatusCode(500, new ResponseAPI<UserSession>
                {
                    EsCorrecto = false,
                    Mensaje = "Error interno",
                    Errores = new List<string> { ex.Message }
                });
            }
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                // 1. Extraemos el Refresh Token de la cookie HttpOnly
                var refreshToken = Request.Cookies["OpcionErp_RefreshToken"];

                if (!string.IsNullOrEmpty(refreshToken))
                {
                    // 2. Lo invalidamos en la base de datos (SQL)
                    await authService.LogoutAsync(refreshToken);
                }
                // 3. Borramos la Cookie de Identidad de ASP.NET (La de los 15 minutos)
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                // 4. Borramos físicamente la cookie del Refresh Token del navegador
                Response.Cookies.Delete("OpcionErp_RefreshToken");

                return Ok(new { mensaje = "Sesión terminada exitosamente" });
            }
            catch (Exception ex) 
            {
                await log.GuardarErrorAsync(
                         message: ex.Message,
                          StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: "HttpPost",
                        ruta: "/api/Authcontroller",
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: $"Authcontroller.Logout");
                throw new Exception($"Error interno {ex.Message}");
            }
        }


    }
}
