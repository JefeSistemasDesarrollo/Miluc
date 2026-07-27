using System.ComponentModel.DataAnnotations;

namespace Miluc.Server.Models.Sap
{
    public class OCRG
    {

        //[Key]
        public Int16 GroupCode { get; set; }
        public string GroupName { get; set; }
    }
}
