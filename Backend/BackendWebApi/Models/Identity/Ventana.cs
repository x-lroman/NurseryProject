namespace BackendWebApi.Models.Identity;

using System.ComponentModel.DataAnnotations;

public class Ventana
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string NombreVentana { get; set; } = string.Empty;

    [Required, MaxLength(255)]
    public string Ruta { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Icono { get; set; }

    public int? Orden { get; set; }
    public bool Activo { get; set; } = true;

    public virtual ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();
}