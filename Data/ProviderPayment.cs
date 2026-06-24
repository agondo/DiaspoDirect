using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaspoDirect.Data;

public enum ProviderPaymentMethod
{
    Wave,
    Orange,
    Cash
}

public enum ProviderPaymentStatus
{
    Pending,
    Paid,
    Failed
}

public class ProviderPayment
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid PaymentId { get; set; }

    public Guid ProviderId { get; set; }

    [Column(TypeName = "numeric(18,2)")]
    public decimal AmountXOF { get; set; }

    public ProviderPaymentMethod PaymentMethod { get; set; }

    [MaxLength(100)]
    public string? ReferenceNumber { get; set; }

    public string? PaidBy { get; set; }

    public DateTime? PaidAt { get; set; }

    public ProviderPaymentStatus Status { get; set; } = ProviderPaymentStatus.Pending;

    [MaxLength(500)]
    public string? ReceiptUrl { get; set; }

    public Payment Payment { get; set; } = null!;
    public Provider Provider { get; set; } = null!;
    public ApplicationUser? PaidByUser { get; set; }
}
