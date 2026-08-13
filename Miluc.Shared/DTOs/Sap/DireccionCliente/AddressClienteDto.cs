using System.ComponentModel.DataAnnotations;

namespace Miluc.Shared.DTOs.Sap.DireccionCliente
{
    public class AddressClienteDto
    {
        public string AddressName { get; set; }
        
        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(254, ErrorMessage = "La dirección no puede superar los 254 caracteres.")]

        public string Street { get; set; }

        [StringLength(100, ErrorMessage = "El barrio o bloque no puede superar los 100 caracteres.")]
        public string Block { get; set; }
        public string  ZipCode { get; set; }
        public string City { get; set; }
        
        
        [StringLength(100, ErrorMessage = "El departamento no puede superar los 100 caracteres.")]

        public string County { get; set; }
        public string Country { get; set; }
        public string  AddressType { get; set; }
        public string  U_HBT_MunMed { get; set; }
        public string   U_HBT_DirMM { get; set; }
    }
}
