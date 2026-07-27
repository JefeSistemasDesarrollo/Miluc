using System;
using System.Collections.Generic;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.EmpleadoDto
{
    public class EmpleadoReaderDto
    {
        public int EmpleadoId { get; set; }

        public string CodigoMunicipio { get; set; }//fk a municipio
        public string NombreMunicipio { get;set;  }
        public int TipoDocumentoId { get; set; }//fk
        public string Documento { get; set; }
        public DateTime FechaExpedicionDoc { get; set; }
        public string PrimerNombre { get; set; } = string.Empty;
        public string SegundoNombre { get; set; } = string.Empty;
       
        public string PrimerApellido { get; set; } = string.Empty;
        public string SegundoApellido { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public int EstadoCivilId { get; set; }//fk
        public string Celular { get; set; }
        public string? CelularAlterno { get; set; }
        public string CorreoElectronico { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Barrio { get; set; }= string.Empty;
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public bool Activo { get; set; }
    }

}
