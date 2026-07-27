namespace Miluc.Shared.DTOs.Autorizacion
{
    public class OtpResponseDto
    {
        public bool Requiere2FA { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}
