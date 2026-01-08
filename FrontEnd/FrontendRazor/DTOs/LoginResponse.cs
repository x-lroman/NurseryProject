namespace FrontendRazor.DTOs;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public UsuarioDto Usuario { get; set; } = new();
}

public class UsuarioDto
{
    public int Id { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}

public class PermisoDto
{
    public string Ventana { get; set; } = string.Empty;
    public string Ruta { get; set; } = string.Empty;
    public string? Icono { get; set; }
    public bool PuedeVer { get; set; }
    public bool PuedeCrear { get; set; }
    public bool PuedeEditar { get; set; }
    public bool PuedeEliminar { get; set; }
}