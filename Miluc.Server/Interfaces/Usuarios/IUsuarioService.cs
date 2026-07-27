using Miluc.Shared.DTOs.Usuarios;

namespace Miluc.Server.Interfaces.Usuarios
{
    public interface IUsuarioService
    {

        // Devuelve los datos y el total (para el paginador) en una tupla
        Task<(List<UsuarioReadDto> Data, int TotalRegistros)> GetAllUsuariosAsync(string? buscar = null, int pagina = 1, int? cantidad = null);
        //Task<List<UsuarioReadDto>> GetAllUsuariosAsync(); //
        Task<UsuarioReadDto?> GetByIdUsuarioAsync(int id);
        Task<UsuarioReadDto> CreateUsuarioAsync(UsuarioCreateDto dto);
        Task<UsuarioReadDto?> UpdateUsuarioAsync(UsuarioUpdateDto dto);
        Task<bool> DeleteUsuarioAsync(int id);

    }
}
