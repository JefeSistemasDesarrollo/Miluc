using Microsoft.AspNetCore.Mvc;

namespace Miluc.Server.Controllers
{
    [ApiController]
    [Route("[controller]")] // Esto hace que la ruta sea /productos
    public class ProductosController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Monitor Gamer", "Teclado Mecánico", "Mouse óptico", "Laptop Pro" };
        }
    }
}
