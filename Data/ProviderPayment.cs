using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaspoDirect.Data;

public class ProviderPayment
{
    [Key]
    public Guid ProviderPaymentId { get; set; } = Guid.NewGuid();

    public Guid ProviderId { get; set; }

    public Guid PrescriptionId { get; set; }

    [Column(TypeName = "numeric(18,2)")]
    public decimal AmountCFA { get; set; }

    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    [MaxLength(100)]
    public string? PaymentMethod { get; set; }

    [MaxLength(100)]
    public string? ReferenceNumber { get; set; }

    [Required, MaxLength(50)]
    public string Status { get; set; } = "Pending";

    public Provider Provider { get; set; } = null!;
}
