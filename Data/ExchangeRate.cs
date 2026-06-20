using System.ComponentModel.DataAnnotations.Schema;

namespace DiaspoDirect.Data;

public class ExchangeRate
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateOnly RateDate { get; set; }

    [Column(TypeName = "numeric(10,2)")]
    public decimal BaseRate { get; set; }

    [Column(TypeName = "numeric(10,2)")]
    public decimal DiaspoDirectRate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
