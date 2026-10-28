using Miluc.Shared.Models.Validaciones.Enums;

namespace Miluc.Shared.DTOs.Sap.TrazabilidadOrdenesVenta
{
    public class DocumentoContexto
    {
        public TipoDocumentoSap TipoDocumentoSap { get; set; }
        //identificador sap 
        public int DocEntry { get; set; }
        public int Documento { get; set; }
        //cliente 
        public string CardCode { get; set; } = string.Empty;
        public string CardName { get; set; }
        //pantalla desde donde inicio
        public string RutaOrigen { get; set; } = string.Empty;
        // Identificador opcional para recuperar filtros
        public string? ContextoId { get; set; }

    }
}
