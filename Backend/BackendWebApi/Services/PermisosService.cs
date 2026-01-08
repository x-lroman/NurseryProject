namespace BackendWebApi.Services;

using Microsoft.EntityFrameworkCore;
using BackendWebApi.Data;
using BackendWebApi.DTOs;

public class PermisosService : IPermisosService
{
    private readonly ApplicationDbContext _context;

    public PermisosService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PermisoDto>> GetPermisosByUsuarioAsync(int usuarioId)
    {
        var permisos = await _context.UsuariosRoles
            .Where(ur => ur.IdUsuario == usuarioId)
            .SelectMany(ur => ur.Rol.Permisos)
            .Include(p => p.Ventana)
            .GroupBy(p => p.IdVentana)
            .Select(g => new PermisoDto
            {
                Ventana = g.First().Ventana.NombreVentana,
                Ruta = g.First().Ventana.Ruta,
                Icono = g.First().Ventana.Icono,
                PuedeVer = g.Any(p => p.PuedeVer),
                PuedeCrear = g.Any(p => p.PuedeCrear),
                PuedeEditar = g.Any(p => p.PuedeEditar),
                PuedeEliminar = g.Any(p => p.PuedeEliminar)
            })
            .Where(p => p.PuedeVer)
            .ToListAsync();

        return permisos;
    }

    public async Task<bool> TienePermisoAsync(int usuarioId, string ventana, string accion)
    {
        var permisos = await GetPermisosByUsuarioAsync(usuarioId);
        var permiso = permisos.FirstOrDefault(p => p.Ventana.Equals(ventana, StringComparison.OrdinalIgnoreCase));

        if (permiso == null) return false;

        return accion.ToLower() switch
        {
            "ver" => permiso.PuedeVer,
            "crear" => permiso.PuedeCrear,
            "editar" => permiso.PuedeEditar,
            "eliminar" => permiso.PuedeEliminar,
            _ => false
        };
    }
}