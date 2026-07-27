using Miluc.Shared.DTOs.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.MatrizSocioDemograficaDto
{
    public class MatrizSociodemograficaReaderDto
    {
        public bool ConsentimientoInformado {get; set;}

        public int MatrizSociodemograficaID { get; set; }
        public int EmpleadoId { get; set; }
        public string NombreEmpleado { get; set; }
        
        public int Edad { get; set; }
        public string Nacionalidad { get; set; }
        public int PaisNacimientoId { get; set; }
        public string NombrePais { get; set; }
        public int GeneroId { get; set; }
        public string NombreGenero { get; set; }
        public bool Fuma { get; set; }
        public int FumaVecesAlMes { get; set; }
        public bool Alcohol { get; set; }
        public int BebeVecesAlAño { get; set; }
        public int DeporteId { get; set; }
        public string NombreDeporte { get; set; }
        public int CondicionMedicaId { get; set; }
        public string NombreCondicionMedica { get; set; }
        public string Hobbies { get; set; }
        public int MedioDeTransporteId { get; set; }
        public string NombreMedioDeTransporte { get; set; }
        public int ClaseDeViviendaId { get; set; }
        public string NombreClaseVivienda { get; set; }
        public int TipoDeViviendaId { get; set; }
        public string NombreTipoDeVivienda { get; set; }
        public int NivelAcademicoId { get; set; }
        public string NombreNivelAcademico { get; set; }
        public int AñoFinalizacionEducacion { get; set; }
        public string EntidadEducativa { get; set; }

        public string TituloObtenido { get; set; }
        public string UltimaEmpresaTrabajo { get; set; }
        public DateTime? FechaUltimoEmpleo { get; set; }
        public string CargoDesempeñado { get; set; }
        public decimal UltimoSalario { get; set; }
        public string PersonaEnCasoDeEmergencia { get; set; }
        public string TelefonoEmergencia { get; set; }
        public string DireccionPersonaEmergencia { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public bool Activo { get; set; }

       


    }
}
