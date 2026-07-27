namespace Miluc.Server.Models.Nomina
{
    public class MatrizSociodemografica
    {
        public int MatrizSociodemograficaID { get; set; }
        public int EmpleadoId { get; set; }
        public bool ConSentimientoInformado { get; set; }
        public int Edad { get; set; }
        public string Nacionalidad { get; set; }
        public int PaisNacimientoId { get; set; }
        public int GeneroId { get; set; }
        public  bool Fuma {  get; set; }
        public int FumaVecesAlMes { get; set; }
        public  bool Alcohol {  get; set; }
        public int BebeVecesAlAño {  get; set; }
        public int DeporteId { get; set; }
        public int CondicionMedicaId { get; set; }
        public string Hobbies { get; set; }
        public int MedioTransporteId { get; set; }
        public int ClaseDeViviendaId { get; set; }
        public int TipoViviendaId { get; set; }
        public int NivelAcademicoId { get; set; }
        public int AñoFinalizacionEducacion { get; set; }
        public string EntidadEducativa { get; set; }

        public string TituloObtenido { get; set; }
        public  string UltimaEmpresaTrabajo { get; set; }
        public DateTime? FechaUltimoEmpleo { get; set; }
        public string CargoDesempeñado { get; set; }
        public decimal UltimoSalario { get; set; }
        public string PersonaEnCasoDeEmergencia { get; set; }
        public string TelefonoEmergencia { get; set; }
        public string DireccionPersonaEmergencia { get;set; }
        public DateTime FechaCreacion {  get; set; }
        public DateTime FechaActualizacion { get; set; }
        public bool Activo {  get; set; }

        //relaciones
        public Genero Genero { get; set; }
        public Pais Pais {  get; set; }
        public Deporte Deporte { get; set; }
        public CondicionMedica CondicionMedica { get; set; }
        public MedioTransporte MedioTransporte { get; set; }
        public TipoVivienda TipoVivienda { get; set; }
        public ClaseVivienda ClaseVivienda { get; set; }
        public NivelAcademico NivelAcademico { get;set; }
        public Empleado Empleado { get; set; }






    }  

}
