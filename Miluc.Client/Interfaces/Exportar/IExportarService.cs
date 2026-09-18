namespace Miluc.Client.Interfaces.Exportar
{
    public interface IExportarService
    {
        Task DescargarExcelAsync<T>(IEnumerable<T> datos,string nombreArchivo);

    }
}
