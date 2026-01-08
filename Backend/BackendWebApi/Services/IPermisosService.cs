namespace BackendWebApi.Services;

using BackendWebApi.DTOs;

public interface IPermisosService
{
    Task<List<PermisoDto>> GetPermisosByUsuarioAsync(int usuarioId);
    Task<bool> TienePermisoAsync(int usuarioId, string ventana, string accion);
}
