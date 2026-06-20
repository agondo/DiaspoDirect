using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaspoDirect.Data;

public class Payment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid PrescriptionId { get; set; }

    [Required]
    public string UserId { get; set; } = "";

    [Column(TypeName = "numeric(18,2)")]
    public decimal AmountUsd { get; set; }

    [Required]
    public string StripeSessionId { get; set; } = "";

    [Required]
    public string StripePaymentIntentId { get; set; } = "";

    [Required, MaxLength(50)]
    public string Status { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public SendPrescription Prescription { get; set; } = null!;
}
