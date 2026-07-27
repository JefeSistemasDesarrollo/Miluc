namespace Miluc.Server.Models.Sap
{
    //responsabilidad fiscla 

    public class OK1_FE_RESPONFIS
    {
        public string ?Code { get; set; }
        public string ?Name { get; set; }
        public string ?U_descripcion { get; set; }

        public char ? U_Inactivo { get; set; }

        //public ICollection<OcrdClienteSap> OCRD { get; set; }
    }
}
