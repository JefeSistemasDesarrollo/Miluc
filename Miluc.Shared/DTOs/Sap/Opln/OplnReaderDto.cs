namespace Miluc.Shared.DTOs.Sap.Opln
{
    public class OplnReaderDto
    {
        public Int16 ListNum { get; set; } //Clave primaria de la tabla OPLN, representa el número de lista de precios en SAP
        public string? ListName { get; set; } //Nombre descriptivo de la lista de precios, utilizado para identificarla en SAP
    }
}
