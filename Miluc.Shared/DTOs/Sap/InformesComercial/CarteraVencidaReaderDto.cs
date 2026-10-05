using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Miluc.Shared.DTOs.Sap.InformesComercial
{
    public class CarteraVencidaReaderDto
    {
        public string? CodigoCliente { get; set; }
        public string? NombreCliente { get; set; }
        public string? Sucursal { get; set; }
        public string? Canal { get; set; }
        public string? Vendedor { get; set; }
        public int? Documento { get; set; }
        public DateTime? FechaContabilizacion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public decimal? TotalDocumento { get; set; }
        public decimal? Pagado { get; set; }
        public decimal SaldoPendiente { get; set; }
        public string ? CondicionPago { get; set; }
        public int  DiasDeCredito { get; set; }
       
        public int ?  DiasVencidos { get; set; }

        //public int? DiasVencimiento { get; set; }
        public string ? RangoDiasVencidos { get; set; }
    }
}
