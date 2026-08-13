using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Autorizacion;
using Miluc.Server.Models;
using Miluc.Server.Models.AutorizacionModel;
using Miluc.Shared.DTOs.Autorizacion;
using Miluc.Shared.DTOs.Usuarios;
using Miluc.Shared.Models.Response;
using System.Security.Cryptography;
using System.Text;

namespace Miluc.Server.Servicios.Autorizacion
{
    public class AuthService(MilucDbContext context,IEmailService _emailService, ITokenService tokenService, IConfiguration config, IOtpService _otpService) : IAuthService
    {

        public async Task<ResponseAPI<UserSession?>> LoginAsync(LoginAccesoRequest request,string ipAddress,string userAgent)
        {
            try
            {
                var usuario = await context.Usuarios
                    .Include(u => u.UsuarioRoles)
                        .ThenInclude(ur => ur.Rol)
                            .ThenInclude(r => r.RolPermisos)
                                .ThenInclude(rp => rp.Permiso)
                    .FirstOrDefaultAsync(u => u.UserName.ToLower() == request.Usuario.ToLower());  //&& u.Activo

                if (usuario == null || !VerificarPasswordHash(request.Password, usuario.PasswordHash, usuario.Salt))
                {
                    return new ResponseAPI<UserSession?>
                    {
                        EsCorrecto = false,
                        Mensaje = "Usuario o contraseña incorrectos"
                    };
                }
                if (usuario.Activo == false)
                {
                    // Asegúrese de que el servicio esté inyectado y el usuario no sea nulo
                    await _emailService.SendAsync(
                        usuario.Email,
                        "Usuario Inactivo",
                        "<h1>Cuenta Desactivada</h1><p>El usuario esta inactivo por favor comuniquese con el administrador del sistema</p>",
                        true
                    );
                    return new ResponseAPI<UserSession?>
                    {
                        EsCorrecto = false,
                        Mensaje = "El usuario esta inactivo por favor comuniquese con el administrador del sistema"
                    };
                }
                if (usuario.DebeCambiarPassword)
                {
                    return new ResponseAPI<UserSession?>
                    {
                        EsCorrecto = true,
                        Mensaje = "Debe cambiar contraseña",
                        Valor = new UserSession
                        {
                            IdUsuario = usuario.IdUsuario,
                            DebeCambiarPassword = true
                        }
                    };
                }

                // 2factor
                if (usuario.TwoFactorEnabled)
                {
                    await _otpService.GenerarYEnviarOtpAsync(usuario.IdUsuario, usuario.Email, ipAddress, userAgent);

                    return new ResponseAPI<UserSession?>
                    {
                        EsCorrecto = true,
                        Mensaje = "Se envió código de verificación",
                        Valor = new UserSession
                        {
                            IdUsuario = usuario.IdUsuario,
                            UserName = usuario.UserName,
                            Email = usuario.Email,
                            Requiere2FA = true
                        }
                    };

                }

                // LOGIN NORMAL SIN 2FA
                return await GenerarSesionCompleta(usuario, ipAddress, userAgent);
            }
            catch (Exception ex)
            {
                return new ResponseAPI<UserSession?>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message
                };
            }
        }
        //separamos el el codigo para enviar primero el login y despues si generara el token 
        private async Task<ResponseAPI<UserSession?>> GenerarSesionCompleta(Usuario usuario,string ipAddress,string userAgent)
        {
            var roles = usuario.UsuarioRoles
                .Select(r => r.Rol.Nombre)
                .ToList();

            var permisos = usuario.UsuarioRoles
                .SelectMany(ur => ur.Rol.RolPermisos)
                .Select(rp => rp.Permiso.Nombre)
                .Distinct()
                .ToList();


            // EXPIRACION
            int AccessTokenExpirationMinutes = config.GetValue<int>("Jwt:AccessTokenExpirationMinutes");
            int RefreshTokenExpiration = config.GetValue<int>("Jwt:RefreshTokenExpiration");

            var fechaExpiracion = DateTime.UtcNow.AddMinutes(AccessTokenExpirationMinutes);
            //var fecharefreshTokenExpiration = DateTime.UtcNow.AddMinutes(RefreshTokenExpiration);

            // GENERAR JWT con permisos 
            var jwtToken = tokenService.GenerarToken(usuario, roles, permisos);

            // REFRESH TOKEN
            //string refreshTokenPlano = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            var randomBytes = RandomNumberGenerator.GetBytes(64);
            string refreshTokenPlano = WebEncoders.Base64UrlEncode(randomBytes);

            var refreshTokenEntity = new RefreshToken
            {
                IdRefreshToken = Guid.NewGuid(),
                IdUsuario = usuario.IdUsuario,
                TokenHash = SHA256.HashData(
                Encoding.UTF8.GetBytes(refreshTokenPlano)),
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(RefreshTokenExpiration),
                RemoteIpAddress = ipAddress,
                UserAgent = userAgent,
                Activo = true,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
            };

            context.RefreshTokens.Add(refreshTokenEntity);
            await context.SaveChangesAsync();

            var session = new UserSession
            {
                IdUsuario = usuario.IdUsuario,
                UserName = usuario.UserName,
                Email = usuario.Email,
                Token = jwtToken,
                RefreshToken = refreshTokenPlano,
                Roles = roles,
                Permisos = permisos,
                FechaExpiracion = fechaExpiracion,
                CodVendedorSAP = usuario.CodVendedorSAP ?? -1,
                Requiere2FA = false
            };

            return new ResponseAPI<UserSession?>
            {
                EsCorrecto = true,
                Mensaje = "Login exitoso",
                Valor = session
            };
        }
        public async Task<ResponseAPI<UserSession?>> VerifyOtpAsync(int idUsuario, string codigo,string ipAddress,string userAgent)
        {
            var valido = await _otpService.ValidarOtpAsync(idUsuario, codigo, ipAddress, userAgent);

            if (!valido)
            {
                return new ResponseAPI<UserSession?>
                {
                    EsCorrecto = false,
                    Mensaje = "Código inválido o expirado"
                };
            }
            var usuario=await context.Usuarios.
                           Include(x=>x.UsuarioRoles)
                                .ThenInclude(x=>x.Rol)          
                                .ThenInclude(x=>x.RolPermisos)
                                .ThenInclude(x=>x.Permiso)
                                .FirstOrDefaultAsync(u=>u.IdUsuario==idUsuario);

            if (usuario == null)
            {
                return new ResponseAPI<UserSession?>
                {
                    EsCorrecto = false,
                    Mensaje = "Usuario no encontrado"
                };
            }
            return await GenerarSesionCompleta(usuario!, ipAddress, userAgent);
        }

        public async Task<ResponseAPI<UserSession?>> RefreshTokenAsync(string tokenPlano,string ipAddress,string userAgent)
        {
            try
            {
                byte[] hashedInput =
                    SHA256.HashData(Encoding.UTF8.GetBytes(tokenPlano));

                var refreshTokenEntity = await context.RefreshTokens
                    .Include(t => t.Usuario)
                        .ThenInclude(u => u.UsuarioRoles)
                            .ThenInclude(ur => ur.Rol)
                                .ThenInclude(r => r.RolPermisos)
                                    .ThenInclude(rp => rp.Permiso)
                    .FirstOrDefaultAsync(t => t.TokenHash == hashedInput);

                if (refreshTokenEntity == null ||!refreshTokenEntity.Activo ||refreshTokenEntity.Expires < DateTime.UtcNow)
                {
                    return new ResponseAPI<UserSession?>
                    {
                        EsCorrecto = false,
                        Mensaje = "Refresh token inválido o expirado"
                    };
                }

                int AccessTokenExpirationMinutes =config.GetValue<int>("Jwt:AccessTokenExpirationMinutes");
                int RefreshTokenExpiration = config.GetValue<int>("Jwt:RefreshTokenExpiration");

                // ROLES
               
                var roles = refreshTokenEntity.Usuario.UsuarioRoles
                    .Select(r => r.Rol.Nombre)
                    .ToList();

                // PERMISOS
        
                var permisos = refreshTokenEntity.Usuario.UsuarioRoles
                    .SelectMany(ur => ur.Rol.RolPermisos)
                    .Select(rp => rp.Permiso.Nombre)
                    .Distinct()
                    .ToList();

                // NUEVO JWT
  
                var jwtToken = tokenService.GenerarToken(
                    refreshTokenEntity.Usuario,
                    roles,
                    permisos);

                var fechaExpiracionUserSession =DateTime.UtcNow.AddMinutes(AccessTokenExpirationMinutes);

                // ROTACION REFRESH TOKEN

                string newRawToken =Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

                context.RefreshTokens.Remove(refreshTokenEntity);

                var newTokenEntity = new RefreshToken
                {
                    IdRefreshToken = Guid.NewGuid(),
                    IdUsuario = refreshTokenEntity.IdUsuario,
                    TokenHash = SHA256.HashData(
                        Encoding.UTF8.GetBytes(newRawToken)),
                    Created = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.AddMinutes(RefreshTokenExpiration), //CORRECION con el refresh token del aptseting
                    RemoteIpAddress = ipAddress,
                    UserAgent = userAgent,
                    Activo = true,
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                };

                await context.RefreshTokens.AddAsync(newTokenEntity);
                await context.SaveChangesAsync();

                // SESSION
                var session = new UserSession
                {
                    IdUsuario = refreshTokenEntity.IdUsuario,
                    UserName = refreshTokenEntity.Usuario.UserName,
                    Email = refreshTokenEntity.Usuario.Email,
                    Token = jwtToken,
                    RefreshToken = newRawToken,
                    Roles = roles,
                    Permisos = permisos,
                    FechaExpiracion = fechaExpiracionUserSession
                };

                return new ResponseAPI<UserSession?>
                {
                    EsCorrecto = true,
                    Mensaje = "Token renovado correctamente",
                    Valor = session
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<UserSession?>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores=new List<string> { ex.Message },
                };
            }
        }
      
        public async Task<bool> LogoutAsync(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken)) return false;

            try
            {
                // 1. Hasheamos el token recibido (porque en la DB solo hay hashes)
                byte[] hashedToken = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(refreshToken));

                // 2. Buscamos el token activo en la DB
                var tokenEntity = await context.RefreshTokens
                    .FirstOrDefaultAsync(t => t.TokenHash == hashedToken);

                if (tokenEntity != null)
                {
                    // 3. Invalidación lógica (Regla de oro: un token usado o cerrado no vuelve a servir)
                    tokenEntity.Activo = false;
                    tokenEntity.Revoked = DateTime.UtcNow;

                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error interno al realizar el logout: {ex.Message}", ex);

            }
        }
        // Método privado para comparar los hashes de forma segura
        private bool VerificarPasswordHash(string password, byte[] hashAlmacenado, byte[] saltAlmacenado)
        {
            using var hmac = new HMACSHA512(saltAlmacenado);
            var hashCalculado = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Comparación bit a bit
            return hashCalculado.SequenceEqual(hashAlmacenado);
        }
        public async Task<ResponseAPI<bool>> CambiarPasswordAsync(CambiarPasswordRequest request)
        {
            try
            {
                if (request.Password != request.ConfirmarPassword)
                {
                    return new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = "Las contraseñas no coinciden"
                    };
                }

                var usuario = await context.Usuarios
                    .FirstOrDefaultAsync(x => x.IdUsuario == request.IdUsuario);

                if (usuario == null)
                {
                    return new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = "Usuario no encontrado"
                    };
                }

                if (!string.IsNullOrWhiteSpace(request.Password))
                {
                    using var hmac = new System.Security.Cryptography.HMACSHA512();

                    usuario.Salt = hmac.Key;
                    usuario.PasswordHash = hmac.ComputeHash(
                        System.Text.Encoding.UTF8.GetBytes(request.Password)
                    );
                }

                usuario.DebeCambiarPassword = false;
                usuario.FechaActualizacion = DateTime.UtcNow;

                await context.SaveChangesAsync();

                return new ResponseAPI<bool>
                {
                    EsCorrecto = true,
                    Mensaje = "Contraseña actualizada correctamente",
                    Valor = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message
                };
            }
        }

        public Task<bool> RegisterAsync(string username, string password, string email)
        {
            throw new NotImplementedException();
        }

        //public async Task<bool> RegisterAsync(string username, string password, string email)
        //{
        //    // 1. Validaciones de existencia (Evitar duplicados)
        //    if (await context.Usuarios.AnyAsync(u => u.UserName.ToLower() == username.ToLower()))
        //        return false;

        //    if (await context.Usuarios.AnyAsync(u => u.Email.ToLower() == email.ToLower()))
        //        return false;

        //    // 2. Transacción para asegurar integridad si agregamos roles después
        //    using var transaction = await context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        // 3. Generar Hash y Salt usando HMACSHA512
        //        using var hmac = new System.Security.Cryptography.HMACSHA512();

        //        var usuario = new Usuario
        //        {
        //            UserName = username,
        //            Email = email,
        //            // Inicializamos Nombres y Apellidos vacíos o con el username para evitar nulos en BD
        //            Nombres = username,
        //            Apellidos = string.Empty,
        //            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
        //            Salt = hmac.Key,
        //            Activo = true,
        //            FechaCreacion = DateTime.UtcNow
        //        };

        //        context.Usuarios.Add(usuario);
        //        await context.SaveChangesAsync();

        //        await transaction.CommitAsync();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        await transaction.RollbackAsync();
        //        return false;
        //    }
        //}



    }
}



//public async Task<ResponseAPI<UserSession?>> LoginAsync(LoginAccesoRequest request,string ipAddress,string userAgent)
//{
//    try
//    {
//        var usuario = await context.Usuarios
//            .Include(u => u.UsuarioRoles)
//                .ThenInclude(ur => ur.Rol)
//                    .ThenInclude(r => r.RolPermisos)
//                        .ThenInclude(rp => rp.Permiso)
//            .FirstOrDefaultAsync(u => u.UserName == request.Usuario && u.Activo);

//        if (usuario == null)
//        {
//            return new ResponseAPI<UserSession?>
//            {
//                EsCorrecto = false,
//                Mensaje = "Usuario o contraseña incorrectos",
//                Errores = new List<string> { "Usuario o contraseña incorrectos" }
//            };
//        }

//        // Validar password
//        if (!VerificarPasswordHash(request.Password, usuario.PasswordHash, usuario.Salt))
//        {
//            return new ResponseAPI<UserSession?>
//            {
//                EsCorrecto = false,
//                Mensaje = "Usuario o contraseña incorrectos",
//                Errores=new List<string> { "Usuario o contraseña incorrectos" }

//            };
//        }

//        // OBTENER ROLES
//        var roles = usuario.UsuarioRoles
//            .Select(r => r.Rol.Nombre)
//            .ToList();

//        // OBTENER PERMISOS
//        var permisos = usuario.UsuarioRoles
//            .SelectMany(ur => ur.Rol.RolPermisos)
//            .Select(rp => rp.Permiso.Nombre)
//            .Distinct()
//            .ToList();

//        // EXPIRACION
//        int AccessTokenExpirationMinutes =config.GetValue<int>("Jwt:AccessTokenExpirationMinutes");
//        int RefreshTokenExpiration = config.GetValue<int>("Jwt:RefreshTokenExpiration");

//        var fechaExpiracion =DateTime.UtcNow.AddMinutes(AccessTokenExpirationMinutes);
//        //var fecharefreshTokenExpiration = DateTime.UtcNow.AddMinutes(RefreshTokenExpiration);

//        // GENERAR JWT con permisos 
//        var jwtToken = tokenService.GenerarToken(usuario, roles, permisos);


//        // REFRESH TOKEN

//        string refreshTokenPlano =Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

//        var refreshTokenEntity = new RefreshToken
//        {
//            IdRefreshToken = Guid.NewGuid(),
//            IdUsuario = usuario.IdUsuario,
//            TokenHash = SHA256.HashData(
//                Encoding.UTF8.GetBytes(refreshTokenPlano)),
//            Created = DateTime.UtcNow,
//            Expires = DateTime.UtcNow.AddMinutes(RefreshTokenExpiration),
//            RemoteIpAddress = ipAddress,
//            UserAgent = userAgent,
//            Activo = true,
//            FechaCreacion = DateTime.Now,
//            FechaActualizacion = DateTime.Now,
//        };

//        context.RefreshTokens.Add(refreshTokenEntity);
//        await context.SaveChangesAsync();


//        // SESSION

//        UserSession userSession = new UserSession
//        {
//            IdUsuario = usuario.IdUsuario,
//            UserName = usuario.UserName,
//            Email = usuario.Email,
//            Token = jwtToken,
//            RefreshToken = refreshTokenPlano,
//            Roles = roles,
//            Permisos = permisos,
//            FechaExpiracion = fechaExpiracion
//        };

//        return new ResponseAPI<UserSession?>
//        {
//            Mensaje = "Usuario encontrado con éxito",
//            EsCorrecto = true,
//            Valor = userSession
//        };
//    }
//    catch (Exception ex)
//    {
//        return new ResponseAPI<UserSession?>
//        {
//            EsCorrecto = false,
//            Mensaje = ex.Message
//        };
//    }
//}