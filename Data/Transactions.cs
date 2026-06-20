using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaspoDirect.Data;

public class Transactions
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public Guid PrescriptionId { get; set; }

    [Required, MaxLength(450)]
    public string UserId { get; set; } = "";

    [Column(TypeName = "numeric(12,2)")]
    public decimal? AmountCfa { get; set; }

    [Column(TypeName = "numeric(12,2)")]
    public decimal? AmountUsd { get; set; }

    [Column(TypeName = "numeric(12,2)")]
    public decimal? FeeUsd { get; set; }

    [Column(TypeName = "numeric(12,2)")]
    public decimal? TotalUsd { get; set; }

    [MaxLength(255)]
    public string? StripeSessionId { get; set; }

    [MaxLength(255)]
    public string? StripePaymentIntentId { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? PaidAt { get; set; }
}
