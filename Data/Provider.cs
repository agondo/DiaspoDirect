using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaspoDirect.Data;

public enum ProviderType
{
    Pharmacy,
    School,
    Clinic,
    Utility
}

public class Provider
{
    [Key]
    public Guid ProviderId { get; set; } = Guid.NewGuid();

    [Required, MaxLength(200)]
    public string ProviderName { get; set; } = "";

    [Required]
    public ProviderType ProviderType { get; set; }

    [MaxLength(50)]
    public string? Phone { get; set; }

    public string? Address { get; set; }

    [MaxLength(100)]
    public string? PaymentMethod { get; set; }

    [MaxLength(50)]
    public string? MobileMoneyNumber { get; set; }

    [MaxLength(100)]
    public string? BankAccountNumber { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public ICollection<ProviderPayment> ProviderPayments { get; set; } = new List<ProviderPayment>();
}
