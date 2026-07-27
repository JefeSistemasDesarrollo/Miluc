using System.Text.Json.Serialization;

namespace Miluc.Server.Models.Sap
{
    public class OSLP
    {
        public int SlpCode { get; set; }
        public string SlpName { get; set; }
        public char Active { get; set; }
        public string? Email { get; set; }
        [JsonIgnore]
        public ICollection<OcrdClienteSap?> OCRD { get; set; }
        [JsonIgnore]
        public ICollection<ORDR> ORDR { get; set; }
    }
}
