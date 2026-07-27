namespace Miluc.Shared.DTOs.Sap.DireccionCliente
{
    public class AddressClienteDto
    {
        public string AddressName { get; set; }
        public string Street { get; set; }
        public string Block { get; set; }
        public string  ZipCode { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string  AddressType { get; set; }
        public string  U_HBT_MunMed { get; set; }
        public string   U_HBT_DirMM { get; set; }
    }
}
