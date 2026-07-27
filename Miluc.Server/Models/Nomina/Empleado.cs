using Microsoft.Identity.Client;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace Miluc.Server.Models.Nomina
{
    public class Empleado
    { 
        public int EmpleadoId { get; set; }

        public string CodigoMunicipio { get; set; }//fk a municipio
        public int TipoDocumentoId { get; set; }
        public string Documento { get; set; }
        public DateTime FechaExpedicionDoc {  get; set; }
        public string PrimerNombre { get; set; }
        public string? SegundoNombre { get; set; }
                  
        public string PrimerApellido { get; set; }    
        public string? SegundoApellido { get; set; }
        public DateTime FechaNacimiento {  get; set; }
        public int EstadoCivilId { get; set; }
        public string Celular {  get; set; }
        public  string? CelularAlterno { get; set; }
        public string CorreoElectronico { get; set; }
        public string Direccion { get; set; }
        public string? Barrio { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion {  get; set;}
        public bool Activo { get; set; }




        public EstadoCivil EstadoCivil { get; set; }// Navigation a municipio
        public TipoDocumento TipoDocumento { get; set; }
        public Municipio Municipio { get; set; }// Navigation a municipio
        


        public ICollection< ContratoLaboral >ContratoLaboral { get; set; } // Navigation a ContratoLaboral


        public virtual AfiliacionSeguridadSocial? AfiliacionSeguridadSocial { get; set; }//navegacion  AfiliacionSeguridadSocial
        public ICollection<MatrizSociodemografica> MatrizSociodemografica { get; set; }
        
        public ICollection<InformacionFamiliar> InformacionFamiliar {  get; set; }
        public ICollection<EsquemaVacunacion> EsquemaVacunacion { get; set; }






    }
}
