using DiaspoDirect.Data;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;

namespace DiaspoDirect.Services;

public class StripeCheckoutService(ApplicationDbContext db)
{
    public async Task<string> CreateCheckoutSessionAsync(
        SendPrescription prescription,
        string userId,
        decimal amountCfa,
        decimal amountUsd,
        decimal feeUsd,
        string successUrl,
        string cancelUrl)
    {
        var totalUsd = amountUsd + feeUsd;

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = ["card"],
            LineItems =
            [
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = (long)Math.Round(totalUsd * 100),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"Prescription - {prescription.RecipientFirstLastName ?? prescription.FirstName + " " + prescription.LastName}",
                            Description = $"{amountCfa:N0} CFA + ${feeUsd:F2} service fee"
                        }
                    },
                    Quantity = 1
                }
            ],
            Mode = "payment",
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl,
            Metadata = new Dictionary<string, string>
            {
                ["prescription_id"] = prescription.Id.ToString(),
                ["user_id"] = userId
            }
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        var transaction = new Transactions
        {
            PrescriptionId = prescription.Id,
            UserId = userId,
            AmountCfa = amountCfa,
            AmountUsd = amountUsd,
            FeeUsd = feeUsd,
            TotalUsd = totalUsd,
            StripeSessionId = session.Id,
            Status = "Pending"
        };

        db.Transactions.Add(transaction);
        await db.SaveChangesAsync();

        return session.Url;
    }

    public async Task<Transactions?> GetBySessionIdAsync(string sessionId)
        => await db.Transactions.FirstOrDefaultAsync(t => t.StripeSessionId == sessionId);

    public async Task MarkPaidAsync(string sessionId, string paymentIntentId)
    {
        var tx = await db.Transactions.FirstOrDefaultAsync(t => t.StripeSessionId == sessionId);
        if (tx is null) return;

        tx.Status = "Paid";
        tx.StripePaymentIntentId = paymentIntentId;
        tx.PaidAt = DateTime.UtcNow;

        var prescription = await db.SendPrescriptions.FindAsync(tx.PrescriptionId);
        if (prescription is not null)
        {
            prescription.Status = "Paid";
            prescription.UpdatedDate = DateTime.UtcNow;
        }

        db.Payments.Add(new Payment
        {
            PrescriptionId = tx.PrescriptionId,
            UserId = tx.UserId,
            AmountUsd = tx.TotalUsd ?? 0m,
            StripeSessionId = sessionId,
            StripePaymentIntentId = paymentIntentId,
            Status = "Paid",
            CreatedAt = DateTime.UtcNow
        });

        await db.SaveChangesAsync();
    }
}
