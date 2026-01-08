namespace BackendWebApi.Models.Identity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Permiso
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int IdRol { get; set; }

    [Required]
    public int IdVentana { get; set; }

    public bool PuedeVer { get; set; }
    public bool PuedeCrear { get; set; }
    public bool PuedeEditar { get; set; }
    public bool PuedeEliminar { get; set; }

    [ForeignKey("IdRol")]
    public virtual Rol Rol { get; set; } = null!;

    [ForeignKey("IdVentana")]
    public virtual Ventana Ventana { get; set; } = null!;
}