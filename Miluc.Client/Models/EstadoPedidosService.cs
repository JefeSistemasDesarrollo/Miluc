namespace Miluc.Client.Models
{
    public class EstadoPedidosService
    {
        public DateTime? FechaInicioContabilizacion { get; set; } = DateTime.Now.Date;
        public DateTime? FechaFinalContabilizacion { get; set; } = DateTime.Now.Date;

        public DateTime? FechaEntregaInicio { get; set; } = DateTime.Now.Date;
        public DateTime? FechaEntregaFinal { get; set; } = DateTime.Now.Date;
        public bool CheckFechasContabilizacion { get; set; } = false;
        public bool CheckFechasEntrega { get; set; } = false;
        public bool CheckEstado { get; set; }
        public bool CheckCanceladas { get; set; }
        public bool CheckImpresas { get; set; }
        public char? EstadoOrden { get; set; }
        public char? Canceladas { get; set; }
        public char? Impresas { get; set; }

        public string TextoBusqueda { get; set; } = string.Empty;
        public int CantidadPorPagina { get; set; } = 12;
        public int PaginaActual { get; set; } = 1;

        public List<string> ListEstadado = new List<string> { "Todas", "Abierta", "Cerrada" };
        public List<string> ListCanceladas = new List<string> { "Todas", "Canceladas", "No Canceladas" };
        public List<string> ListImpresas = new List<string> { "Todas", "Impresass", "No Impresa" };
    }
}
