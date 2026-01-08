namespace BackendWebApi.Models.Core;

using System.ComponentModel.DataAnnotations;

public class Expediente
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string CodigoExpediente { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    [MaxLength(100)]
    public string? Responsable { get; set; }

    [MaxLength(100)]
    public string? TipoCultivo { get; set; }

    [MaxLength(100)]
    public string? Variedad { get; set; }

    [MaxLength(100)]
    public string? Origen { get; set; }

    [MaxLength(50)]
    public string? Certificacion { get; set; }

    [MaxLength(20)]
    public string? Estado { get; set; }

    [Required]
    public DateTime FechaIngreso { get; set; }

    public string? Observaciones { get; set; }

    [Required, MaxLength(50)]
    public string CreadoPor { get; set; } = string.Empty;

    public DateTime FechaCreado { get; set; } = DateTime.Now;

    [MaxLength(50)]
    public string? ActualizadoPor { get; set; }

    public DateTime? FechaActualizado { get; set; }
    public bool Habilitado { get; set; } = true;

    public virtual ICollection<Lote> Lotes { get; set; } = new List<Lote>();
}
