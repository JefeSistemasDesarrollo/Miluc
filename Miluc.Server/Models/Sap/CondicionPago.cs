using System.ComponentModel.DataAnnotations;

namespace Miluc.Server.Models.Sap
{
    public class CondicionPago
    {
        [Key]
        public Int16 GroupNum { get; set; }
        public string PymntGroup { get; set; }
        public ICollection<OcrdClienteSap> Clientes { get; set; }
    }
}
