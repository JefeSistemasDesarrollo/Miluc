namespace Miluc.Shared.DTOs.Sap.DireccionCliente
{
    public class DireccionesCrd1ReaderDto
    {
        public string? CardCode { get; set; }
        public string? Address { get; set; }//DIRECCION PRINCIPAL DEL CLIENTE
        public string? Street { get; set; }//calle
        public string? Block { get; set; }//bloque o manzana
        public string? ZipCode { get; set; }//la relacion entre ciudad medios magneticos y cliente
        public string? City { get; set; }//CIUDAD DEL CLIENTE
        public string? County { get; set; }//DEPARTAMENTO DEL CLIENTE

        public string? Country { get; set; }//PAIS DEL CLIENTE
        public string? State { get; set; } = string.Empty; //DEPARTAMENTO O ESTADO DEL CLIENTE
        public char? AdresType { get; set; }
        public char? U_HBT_DirMM { get; set; }
    }
}
