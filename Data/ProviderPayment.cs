using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaspoDirect.Data;

public enum ProviderPaymentMethod
{
    Wave = 1,
    OrangeMoney = 2,
    MTNMobileMoney = 3,
    Cash = 4,
    BankTransfer = 5
}

public enum ProviderPaymentStatus
{
    Assigned = 1,
    Pending = 2,
    Paid = 3,
    Failed = 4,
    Cancelled = 5
}

public class ProviderPayment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid PaymentId { get; set; }
    public Payment Payment { get; set; } = default!;

    public Guid ProviderId { get; set; }
    public Provider Provider { get; set; } = default!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal AmountXOF { get; set; }

    public ProviderPaymentMethod PaymentMethod { get; set; }

    [MaxLength(100)]
    public string? ReferenceNumber { get; set; }

    public string? PaidByUserId { get; set; }
    public ApplicationUser? PaidByUser { get; set; }

    public DateTime? PaidAt { get; set; }

    public ProviderPaymentStatus Status { get; set; } = ProviderPaymentStatus.Pending;

    [MaxLength(500)]
    public string? ReceiptUrl { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
