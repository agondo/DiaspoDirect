using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaspoDirect.Data;

public class MyPrescription
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string UserId { get; set; } = "";

    [Required, MaxLength(100)]
    public string FirstName { get; set; } = "";

    [Required, MaxLength(100)]
    public string LastName { get; set; } = "";

    [Required, MaxLength(30)]
    public string PhoneNumber { get; set; } = "";

    [MaxLength(200)]
    public string? RecipientFirstLastName { get; set; }

    [MaxLength(50)]
    public string? RecipientPhone { get; set; }

    [Required, MaxLength(255)]
    public string PrescriptionFileName { get; set; } = "";

    [Required]
    public string PrescriptionFileUrl { get; set; } = "";

    [Column(TypeName = "numeric(18,2)")]
    public decimal AmountCFA { get; set; }

    [Column(TypeName = "numeric(18,2)")]
    public decimal? AmountUSD { get; set; }

    [Required, MaxLength(50)]
    public string Status { get; set; } = "Pending";

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedDate { get; set; }

    public string? Notes { get; set; }

    [MaxLength(255)]
    public string? StripePaymentIntentId { get; set; }

    [Column(TypeName = "numeric(10,2)")]
    public decimal? ServiceFeeUsd { get; set; }

    [Column(TypeName = "numeric(10,2)")]
    public decimal? TotalAmountUsd { get; set; }

    public DateTime? PaidAt { get; set; }

    public Guid? PharmacyId { get; set; }
    public Pharmacy? Pharmacy { get; set; }
}
