using System.ComponentModel.DataAnnotations;

namespace Miluc.Server.Models.LogsErrores
{
    public class LogsErrores
    {

        [Key]
        public int IdLogs { get; set; }
        public string Mensaje { get; set; } = null!;

        public string? StackTrace { get; set; }
        public string? Ruta { get; set; }
        public string? Metodo { get; set; }
        public string? Usuario { get; set; }
        public string? DireccionIp { get; set; }
        public string? Origen { get; set; }
        public string? Nivel { get; set; }
        public DateTime Fecha { get; set; }
    }
}
