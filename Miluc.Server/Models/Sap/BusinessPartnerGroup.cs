using System.ComponentModel.DataAnnotations;

namespace Miluc.Server.Models.Sap
{
    public class BusinessPartnerGroup
    {

        public Int16 ?   GroupCode { get; set; }
        public string? GroupName { get; set; }
        public ICollection<OcrdClienteSap> OCRD { get; set; }
    }
}
