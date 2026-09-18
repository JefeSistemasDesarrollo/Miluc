using ClosedXML.Excel;
using Microsoft.JSInterop;
using Miluc.Client.Interfaces.Exportar;
using System.Reflection;

namespace Miluc.Client.Servicios.ExportarArchivo
{
    public class ExportarService : IExportarService
    {

        private readonly IJSRuntime _jsRuntime;
        public ExportarService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task DescargarExcelAsync<T>(IEnumerable<T> datos,string nombreArchivo)
        {
            if (datos == null)
                throw new ArgumentNullException(nameof(datos));

            var lista = datos.ToList();

            if (!lista.Any())
                throw new InvalidOperationException(
                    "No hay datos para generar el archivo Excel.");

            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Datos");

            var propiedades = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Encabezados
            for (int columna = 0; columna < propiedades.Length; columna++)
            {
                worksheet.Cell(1, columna + 1).Value =
                    propiedades[columna].Name;
            }

            // Datos
            for (int fila = 0; fila < lista.Count; fila++)
            {
                var elemento = lista[fila];

                for (int columna = 0; columna < propiedades.Length; columna++)
                {
                    var valor = propiedades[columna].GetValue(elemento);

                    worksheet.Cell(fila + 2, columna + 1).Value =
                        valor?.ToString() ?? string.Empty;
                }
            }

            // Formato
            var rangoEncabezado = worksheet.Range(1,1,1,propiedades.Length);

            rangoEncabezado.Style.Font.Bold = true;

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            var bytes = stream.ToArray();

            var base64 = Convert.ToBase64String(bytes);

            if (!nombreArchivo.EndsWith(".xlsx",
                StringComparison.OrdinalIgnoreCase))
            {
                nombreArchivo += ".xlsx";
            }

            await _jsRuntime.InvokeVoidAsync(
                "descargarArchivo",
                nombreArchivo,
                base64);
        }
    }
}
