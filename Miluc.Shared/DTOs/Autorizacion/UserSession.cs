namespace Miluc.Shared.DTOs.Autorizacion
{
    public class UserSession
    {
        public int IdUsuario { get; set; }
        public string UserName { get; set; } = null!;
        public string? Email { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public bool Requiere2FA { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public bool DebeCambiarPassword { get; set; }

        public int? CodVendedorSAP { get; set; }
        public List<string> Roles { get; set; } = new();
        public List<string> Permisos { get; set; } = new();
    }
}
