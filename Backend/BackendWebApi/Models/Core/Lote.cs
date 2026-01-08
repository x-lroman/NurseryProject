namespace BackendWebApi.Models.Core;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Lote
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int IdExpediente { get; set; }

    [Required, MaxLength(50)]
    public string CodigoLote { get; set; } = string.Empty;

    [Required]
    public int Capacidad { get; set; }

    [MaxLength(20)]
    public string? Estado { get; set; }

    public string? Observaciones { get; set; }

    [Required, MaxLength(50)]
    public string CreadoPor { get; set; } = string.Empty;

    public DateTime FechaCreado { get; set; } = DateTime.Now;

    [MaxLength(50)]
    public string? ActualizadoPor { get; set; }

    public DateTime? FechaActualizado { get; set; }
    public bool Habilitado { get; set; } = true;

    [ForeignKey("IdExpediente")]
    public virtual Expediente Expediente { get; set; } = null!;

    public virtual ICollection<Contenedor> Contenedores { get; set; } = new List<Contenedor>();
}