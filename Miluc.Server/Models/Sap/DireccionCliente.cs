using System.ComponentModel.DataAnnotations;

namespace Miluc.Server.Models.Sap
{
    public class DireccionCliente
    {
        public string CardCode { get; set; }
        public string Address { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string AdresType { get; set; }
        public OcrdClienteSap Cliente { get; set; }
    }
}
