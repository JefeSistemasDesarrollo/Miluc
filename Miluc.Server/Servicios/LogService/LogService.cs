using Miluc.Server.Data;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Models.LogsErrores;

namespace Miluc.Server.Servicios.LogService
{
    public class LogService( MilucDbContext _context) : ILogService
    {
        public async Task GuardarErrorAsync(string ?message= null,string ? StackTrace=null,  string? usuario = null, string? metodo = null, string? ruta = null, string? ip = null, string? origen = null, string nivel = "Error")
        {
            try
            {
                var log = new LogsErrores
                {
                    Mensaje = message,
                    StackTrace = StackTrace,
                    Ruta = ruta,
                    Metodo = metodo,
                    Usuario = usuario,
                    DireccionIp = ip,
                    Origen = origen,
                    Nivel = nivel,
                    Fecha = DateTime.Now
                };

               await _context.LogsErrores.AddAsync(log);

                await _context.SaveChangesAsync();
            }
            catch (Exception dbEx)
            {
                await GuardarEnArchivoAsync(dbEx, dbEx, usuario, metodo, ruta, ip, origen);
            }
        }

        private async Task GuardarEnArchivoAsync(
                    Exception ex,
                    Exception dbEx,
                    string? usuario,
                    string? metodo,
                    string? ruta,
                    string? ip,
                    string? origen)
        {
            try
            {
                var carpeta = Path.Combine("C:\\", "logs");

                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                var archivo = Path.Combine(carpeta, $"error-{DateTime.Now:yyyyMMdd}.txt");

                var contenido = $@"
                            ---------------------------------------
                            Fecha: {DateTime.Now}
                            Usuario: {usuario}
                            Metodo: {metodo}
                            Ruta: {ruta}
                            IP: {ip}
                            Origen: {origen}
                            ERROR:
                            {ex.Message}
                            STACK:
                            {ex.StackTrace}
                            ERROR DB:
                            {dbEx.Message}
                            ---------------------------------------
                            ";

                await File.AppendAllTextAsync(archivo, contenido);
            }
            catch
            {
                Console.WriteLine(ex.Message);
            }
        }

       
    }
}
