namespace BackendWebApi.Models.Identity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UsuarioRol
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int IdUsuario { get; set; }

    [Required]
    public int IdRol { get; set; }

    public DateTime FechaAsignacion { get; set; } = DateTime.Now;

    [ForeignKey("IdUsuario")]
    public virtual Usuario Usuario { get; set; } = null!;

    [ForeignKey("IdRol")]
    public virtual Rol Rol { get; set; } = null!;
}