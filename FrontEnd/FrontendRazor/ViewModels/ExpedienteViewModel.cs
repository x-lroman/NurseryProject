namespace FrontendRazor.ViewModels;

using System.ComponentModel.DataAnnotations;

public class ExpedienteViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El código es requerido")]
    [Display(Name = "Código de Expediente")]
    public string CodigoExpediente { get; set; } = string.Empty;

    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [Display(Name = "Responsable")]
    public string? Responsable { get; set; }

    [Display(Name = "Tipo de Cultivo")]
    public string? TipoCultivo { get; set; }

    [Display(Name = "Variedad")]
    public string? Variedad { get; set; }

    [Display(Name = "Origen")]
    public string? Origen { get; set; }

    [Display(Name = "Certificación")]
    public string? Certificacion { get; set; }

    [Display(Name = "Estado")]
    public string? Estado { get; set; }

    [Required]
    [Display(Name = "Fecha de Ingreso")]
    [DataType(DataType.Date)]
    public DateTime FechaIngreso { get; set; } = DateTime.Now;

    [Display(Name = "Observaciones")]
    public string? Observaciones { get; set; }

    public string? CreadoPor { get; set; }
    public DateTime? FechaCreado { get; set; }
    public string? ActualizadoPor { get; set; }
    public DateTime? FechaActualizado { get; set; }
    public bool Habilitado { get; set; } = true;
}
