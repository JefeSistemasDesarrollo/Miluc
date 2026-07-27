using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Miluc.Client.Servicios.Autorizacion
{
    public class CookieHandler : DelegatingHandler
    {
        public CookieHandler()
        {
            // Esto le dice al handler: "Tu siguiente paso es el manejador del navegador"
            InnerHandler = new HttpClientHandler();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Incluye las credenciales (cookies) en la petición
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
