namespace BackendWebApi.Models.Core;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class PlantaObs
{
    [Key]
    public long Id { get; set; }

    [Required]
    public int IdBandeja { get; set; }

    [Required, MaxLength(50)]
    public string CodigoPlanta { get; set; } = string.Empty;

    public string? Observaciones { get; set; }

    [Required, MaxLength(50)]
    public string CreadoPor { get; set; } = string.Empty;

    public DateTime FechaCreado { get; set; } = DateTime.Now;

    [MaxLength(50)]
    public string? ActualizadoPor { get; set; }

    public DateTime? FechaActualizado { get; set; }
    public bool Habilitado { get; set; } = true;

    [ForeignKey("IdBandeja")]
    public virtual BandejaObs BandejaObs { get; set; } = null!;
}