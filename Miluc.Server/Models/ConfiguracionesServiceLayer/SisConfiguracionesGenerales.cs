namespace Miluc.Server.Models.ConfiguracionesServiceLayer
{
    public class SisConfiguracionesGenerales
    {
        public int ConfiguracionesGeneralesId { get; set; }
        public string? UrlServiceLayer { get; set; }
        public string? UserNameServiceLayer { get; set; }
        public string? PasswordServiceLayer { get; set; }
        public string? CompanyDB { get; set; }
        public string? LanguageServiceLayer { get; set; }
        public string? Modulo { get; set; }
    }
}
