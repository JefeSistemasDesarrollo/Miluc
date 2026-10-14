namespace Miluc.Server.Models
{
    public class AsignarFiltros
    {
        public int AsignarFiltrosId { get; set; }
        public int IdUsuario { get; set; }

        public string ? NombreModulo { get; set; } 
        public string ? Descripcion { get; set; } 
        public string Filtro { get; set; } 

        public Usuario Usuario { get; set; } = null!;
    }
}
