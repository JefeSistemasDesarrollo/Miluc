using Miluc.Shared.DTOs.Usuarios;

namespace Miluc.Server.Interfaces.Usuarios
{
    public interface ITipoUsuario
    {
        Task<List<TipoUsuarioReadDto>> GetAllTipoUsuarioAsync();
        Task<TipoUsuarioReadDto?> GetByIdTipoUsuarioAsync(int id);
        Task<bool> CreateTipoUsuarioAsync(TipoUsuarioCreateDto dto);
        Task<bool?> UpdateTipoUsuarioAsync(TipoUsuarioUpdateDto dto);
        Task<bool> DeleteTipoUserAsync(int idUsuario);
    }
}
