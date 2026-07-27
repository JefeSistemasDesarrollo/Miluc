namespace Miluc.Server.Models.Sap
{
    public class HBT_ACTIVIDADECO
    {
        public string? Code 
        {
            get; set;
        }
        public string? Name 
        {
            get; set;
        }
        public string? U_Descripcion 
        {
            get; set; 
        }

        public ICollection<OcrdClienteSap>? OCRD 
        {
            get; set; 
        } // Relación con clientes  
    }
}
