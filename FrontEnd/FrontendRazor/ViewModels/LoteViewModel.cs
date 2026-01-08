namespace FrontendRazor.ViewModels;

using System.ComponentModel.DataAnnotations;

public class LoteViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un expediente")]
    [Display(Name = "Expediente")]
    public int IdExpediente { get; set; }

    [Required(ErrorMessage = "El código es requerido")]
    [Display(Name = "Código de Lote")]
    public string CodigoLote { get; set; } = string.Empty;

    [Required(ErrorMessage = "La capacidad es requerida")]
    [Display(Name = "Capacidad")]
    [Range(1, int.MaxValue, ErrorMessage = "La capacidad debe ser mayor a 0")]
    public int Capacidad { get; set; }

    [Display(Name = "Estado")]
    public string? Estado { get; set; }

    [Display(Name = "Observaciones")]
    public string? Observaciones { get; set; }

    // Para mostrar en la vista
    public string? ExpedienteNombre { get; set; }

    public string? CreadoPor { get; set; }
    public DateTime? FechaCreado { get; set; }
    public string? ActualizadoPor { get; set; }
    public DateTime? FechaActualizado { get; set; }
    public bool Habilitado { get; set; } = true;
}