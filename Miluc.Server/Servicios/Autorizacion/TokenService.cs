using Microsoft.IdentityModel.Tokens;
using Miluc.Server.Interfaces.Autorizacion;
using Miluc.Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Miluc.Server.Servicios.Autorizacion
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        //public string GenerarToken(Usuario usuario)
        //{
        //    // 1. Definimos los Claims (La información que viajará dentro del token)
        //    var claims = new List<Claim>
        //{
        //    new(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
        //    new(ClaimTypes.Name, usuario.UserName),
        //    new(ClaimTypes.Email, usuario.Email ?? "")

        //};
        //    // 2. Agregamos los roles del usuario al Token
        //    if (usuario.UsuarioRoles != null)
        //    {
        //        foreach (var userRol in usuario.UsuarioRoles)
        //        {
        //            claims.Add(new Claim(ClaimTypes.Role, userRol.Rol.Nombre));

        //        }
        //    }

        //    // 3. Creamos la llave de seguridad
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    int AccessTokenExpirationMinutes = config.GetValue<int>("Jwt:AccessTokenExpirationMinutes");

        //    // 4. Configuramos el cuerpo del Token
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(claims),
        //        Expires = DateTime.UtcNow.AddMinutes(AccessTokenExpirationMinutes), // El token dura 30 minutos
        //        SigningCredentials = creds,
        //        Issuer = config["Jwt:Issuer"],
        //        Audience = config["Jwt:Audience"]
        //    };

        //    // 5. Generamos el string final
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var token = tokenHandler.CreateToken(tokenDescriptor);

        //    return tokenHandler.WriteToken(token);
        //}
        public string GenerarToken(Usuario usuario, List<string> roles, List<string> permisos)
        {
            var claims = new List<Claim>
           {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.UserName),
                new Claim(ClaimTypes.Email, usuario.Email ?? "")
           };

            // Roles
            if (roles != null && roles.Any())
            {
                claims.AddRange(
                    roles.Select(r => new Claim(ClaimTypes.Role, r))
                );
            }
            // Permisos
            if (permisos != null && permisos.Any())
            {
                claims.AddRange(
                    permisos.Select(p => new Claim("Permiso", p))
                );
            }    // Llave
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["Jwt:Key"]!)
            );
            var creds = new SigningCredentials(
                key,SecurityAlgorithms.HmacSha256);

            int expiration = config.GetValue<int>("Jwt:AccessTokenExpirationMinutes");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiration),
                SigningCredentials = creds,
                Issuer = config["Jwt:Issuer"],
                Audience = config["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }
    }
}
