using Miluc.Server.Interfaces.LogErrores;

namespace Miluc.Server.Servicios.LogService
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context, ILogService logService)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex) 
            {
                var metodo = context.Request.Method;
                var ruta=context.Request.Path.ToString();

                //ip
                var ip = context.Connection.RemoteIpAddress?.ToString();

                var usuario = context.User?.Identity.Name;

                if (string.IsNullOrEmpty(usuario))
                {
                    usuario = context.User?.Claims
                        .FirstOrDefault(c => c.Type == "unique_name")?.Value;
                }

                await logService.GuardarErrorAsync(
                   ex.Message,
                   ex.StackTrace,
                   usuario,
                   metodo,
                   ruta,
                   ip,
                   "MiddlewareGlobal",
                   "Error");

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Error interno del servidor");

            }
        }
    }
}
