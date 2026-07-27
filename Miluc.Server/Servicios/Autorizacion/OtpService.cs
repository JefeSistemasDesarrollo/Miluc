using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Autorizacion;
using Miluc.Server.Models;

using System.Security.Cryptography;
using System.Text;

namespace Miluc.Server.Servicios.Autorizacion
{
    public class OtpService : IOtpService
    {

        private readonly MilucDbContext _context;
        private readonly IEmailService _emailService;

        private const int OTP_EXPIRATION_MINUTES = 5;
        private const int MAX_INTENTOS = 5;

        public OtpService(MilucDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        public async Task GenerarYEnviarOtpAsync(int idUsuario,string email,string ip,string userAgent)
        {
            try
            {
                await InvalidarOtpsActivosAsync(idUsuario);

                var codigo = GenerarCodigoOtp();
                var hash = GenerarHashSeguro(codigo);

                var otp = new UsuarioOTP
                {
                    IdUsuario = idUsuario,
                    CodigoHash = hash,
                    Expira = DateTime.UtcNow.AddMinutes(OTP_EXPIRATION_MINUTES),
                    RemoteIpAddress = ip,
                    UserAgent = userAgent
                    // FechaCreacion se asigna automáticamente
                };

                await _context.UsuarioOTP.AddAsync(otp);
                await _context.SaveChangesAsync();

                await _emailService.SendAsync(
                    email,
                    "Código de verificación",
                    $"Tu código es: <b>{codigo}</b>. Expira en {OTP_EXPIRATION_MINUTES} minutos.",
                    true
                );

            }
            catch (Exception ex)
            {


                throw;

            }


        }

        
        // VALIDAR OTP
        public async Task<bool> ValidarOtpAsync(int idUsuario,string codigoIngresado,string ip,string userAgent)
        {
            var otp = await _context.UsuarioOTP
                .Where(x =>
                    x.IdUsuario == idUsuario &&
                    !x.Usado &&
                    x.Expira > DateTime.UtcNow)
                .OrderByDescending(x => x.FechaCreacion)
                .FirstOrDefaultAsync();

            if (otp == null)
                return false;

            // Validación de IP y UserAgent
            if (otp.RemoteIpAddress != ip || otp.UserAgent != userAgent)
                return false;

            var hashIngresado = GenerarHashSeguro(codigoIngresado);

            // Comparación segura contra timing attacks
            if (!CryptographicOperations.FixedTimeEquals(hashIngresado, otp.CodigoHash))
            {
                otp.Intentos++;

                if (otp.Intentos >= MAX_INTENTOS)
                    otp.Usado = true;

                await _context.SaveChangesAsync();
                return false;
            }

            otp.Usado = true;

            await _context.SaveChangesAsync();
            return true;
        }

        // INVALIDAR CODIGO PARA QUE NO LO PUEDAN SEGUIR UTILIZANDO 
        public async Task InvalidarOtpsActivosAsync(int idUsuario)
        {
            var activos = await _context.UsuarioOTP
                .Where(x => x.IdUsuario == idUsuario && !x.Usado)
                .ToListAsync();

            foreach (var item in activos)
                item.Usado = true;

            await _context.SaveChangesAsync();
        }

        private string GenerarCodigoOtp()
        {
            int numero = RandomNumberGenerator.GetInt32(100000, 1000000);
            return numero.ToString();
        }

        private byte[] GenerarHashSeguro(string codigo)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(codigo));
        }
    }

}
