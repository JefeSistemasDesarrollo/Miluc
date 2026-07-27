namespace Miluc.Shared.DTOs.Autorizacion
{
    public class VerifyOtpRequest
    {
        public int IdUsuario { get; set; }
        public string Codigo { get; set; } = string.Empty;
    }
}
