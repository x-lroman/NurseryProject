namespace BackendWebApi.Models.Identity;

using System.ComponentModel.DataAnnotations;

public class Rol
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string NombreRol { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Descripcion { get; set; }

    public bool Activo { get; set; } = true;

    public virtual ICollection<UsuarioRol> UsuariosRoles { get; set; } = new List<UsuarioRol>();
    public virtual ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();
}
