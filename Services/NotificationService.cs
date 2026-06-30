using DiaspoDirect.Components.Account;
using DiaspoDirect.Data;

namespace DiaspoDirect.Services;

public class NotificationService(GmailEmailSender email, IConfiguration config, ILogger<NotificationService> logger)
{
    private readonly string _adminEmail = config["Email:AdminAddress"] ?? "ceo@flaubertgroup.com";

    public async Task NotifyPaymentReceivedAsync(Payment payment, SendPrescription prescription, string customerEmail, string customerName)
    {
        try
        {
            await email.SendEmailAsync(
                _adminEmail,
                $"New Payment Received — {prescription.RecipientFirstLastName ?? "—"} (${payment.AmountUsd:F2})",
                Template("New Payment Received", $"""
                    <p>A new Stripe payment has been confirmed.</p>
                    {Row("Customer", $"{customerName} &lt;{customerEmail}&gt;")}
                    {Row("Recipient", prescription.RecipientFirstLastName ?? "—")}
                    {Row("Country", prescription.CountryName)}
                    {Row("Prescription", prescription.PrescriptionFileName)}
                    {Row("Amount (XOF)", $"{prescription.AmountCFA:N0} XOF")}
                    {Row("Amount (USD)", $"${payment.AmountUsd:F2}")}
                    {Row("Stripe ID", payment.StripePaymentIntentId)}
                    {Row("Date", payment.CreatedAt.ToString("MMM dd, yyyy HH:mm UTC"))}
                    <p style="margin-top:24px">
                        <a href="https://diaspodirect.com/admin/provider-payments"
                           style="background:#1b6ec2;color:#fff;padding:10px 20px;border-radius:4px;text-decoration:none">
                            Assign Provider →
                        </a>
                    </p>
                """));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send payment received notification for Payment {PaymentId}", payment.Id);
        }
    }


    public async Task NotifyRecipientPrescriptionProcessedAsync(SendPrescription prescription, string recipientEmail, string recipientName, string providerName)
    {
        if (string.IsNullOrWhiteSpace(recipientEmail)) return;

        try
        {
            await email.SendEmailAsync(
                recipientEmail,
                $"Your prescription has been processed — DiaspoDirect",
                Template("Prescription Processed", $"""
                    <p>Dear {recipientName},</p>
                    <p>Your prescription has been processed and fulfilled by our pharmacy partner.</p>
                    {Row("Prescription", prescription.PrescriptionFileName ?? "—")}
                    {Row("Country", prescription.CountryName)}
                    {Row("Pharmacy", providerName)}
                    {Row("Date", DateTime.UtcNow.ToString("MMM dd, yyyy"))}
                    <p style="margin-top:16px">Thank you for using DiaspoDirect.</p>
                """));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send recipient prescription processed notification to {Email}", recipientEmail);
        }
    }

    public async Task NotifyProviderAssignedAsync(Provider provider, ProviderPayment providerPayment, SendPrescription prescription)
    {
        try
        {
            await email.SendEmailAsync(
                _adminEmail,
                $"Provider Assigned — {provider.ProviderName} for {prescription.RecipientFirstLastName ?? "—"}",
                Template("Provider Assigned", $"""
                    <p>A provider has been assigned to a prescription.</p>
                    {Row("Provider", provider.ProviderName)}
                    {Row("Type", provider.ProviderType.ToString())}
                    {Row("Prescription", prescription.PrescriptionFileName)}
                    {Row("Recipient", prescription.RecipientFirstLastName ?? "—")}
                    {Row("Country", prescription.CountryName)}
                    {Row("Amount (XOF)", $"{providerPayment.AmountXOF:N0} XOF")}
                    {Row("Assigned At", providerPayment.CreatedAt.ToString("MMM dd, yyyy HH:mm UTC"))}
                """));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send provider assigned admin notification for ProviderPayment {Id}", providerPayment.Id);
        }

        if (string.IsNullOrWhiteSpace(provider.Email)) return;

        try
        {
            await email.SendEmailAsync(
                provider.Email,
                "New Prescription Assignment — DiaspoDirect",
                Template($"New Assignment: {prescription.RecipientFirstLastName ?? "—"}", $"""
                    <p>Dear {provider.ProviderName},</p>
                    <p>You have been assigned a new prescription from DiaspoDirect. Please prepare the order and await payment confirmation.</p>
                    {Row("Recipient", prescription.RecipientFirstLastName ?? "—")}
                    {Row("Country", prescription.CountryName)}
                    {Row("Prescription File", prescription.PrescriptionFileName)}
                    {Row("Amount to Receive", $"{providerPayment.AmountXOF:N0} XOF")}
                    {Row("Payment Method", providerPayment.PaymentMethod.ToString())}
                    <p style="margin-top:16px">You will receive another email once payment has been sent.</p>
                """));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send provider assigned email to {Email}", provider.Email);
        }
    }

    public async Task NotifySenderProviderPaidAsync(SendPrescription prescription, Provider provider, ProviderPayment providerPayment, string senderEmail, string senderName)
    {
        if (string.IsNullOrWhiteSpace(senderEmail)) return;

        try
        {
            await email.SendEmailAsync(
                senderEmail,
                $"Prescription fulfilled — {prescription.RecipientFirstLastName ?? "—"}",
                Template("Prescription Fulfilled", $"""
                    <p>Dear {senderName},</p>
                    <p>The prescription you sent for <strong>{prescription.RecipientFirstLastName ?? "—"}</strong> has been fulfilled. The pharmacy has been paid and the order is complete.</p>
                    {Row("Recipient", prescription.RecipientFirstLastName ?? "—")}
                    {Row("Country", prescription.CountryName)}
                    {Row("Pharmacy", provider.ProviderName)}
                    {Row("Amount Paid (XOF)", $"{providerPayment.AmountXOF:N0} XOF")}
                    {Row("Date", providerPayment.PaidAt?.ToString("MMM dd, yyyy") ?? DateTime.UtcNow.ToString("MMM dd, yyyy"))}
                    <p style="margin-top:16px">Thank you for using DiaspoDirect.</p>
                """));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send sender provider paid notification to {Email}", senderEmail);
        }
    }

    public async Task NotifyProviderPaidAsync(Provider provider, ProviderPayment providerPayment, SendPrescription prescription, string customerEmail, string customerName)
    {
        try
        {
            await email.SendEmailAsync(
                customerEmail,
                "Your Prescription Has Been Delivered — DiaspoDirect",
                Template("Prescription Delivered", $"""
                    <p>Dear {customerName},</p>
                    <p>Great news! Your prescription order has been fulfilled and payment sent to the provider.</p>
                    {Row("Recipient", prescription.RecipientFirstLastName ?? "—")}
                    {Row("Country", prescription.CountryName)}
                    {Row("Provider", provider.ProviderName)}
                    {Row("Amount Paid (XOF)", $"{providerPayment.AmountXOF:N0} XOF")}
                    {Row("Reference", providerPayment.ReferenceNumber ?? "—")}
                    {Row("Paid On", providerPayment.PaidAt?.ToString("MMM dd, yyyy") ?? "—")}
                    <p style="margin-top:16px">Thank you for trusting DiaspoDirect.</p>
                """));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send provider paid customer notification to {Email}", customerEmail);
        }

        try
        {
            await email.SendEmailAsync(
                _adminEmail,
                $"Provider Payment Confirmed — {provider.ProviderName}",
                Template("Provider Payment Confirmed", $"""
                    <p>A provider payment has been marked as paid.</p>
                    {Row("Provider", provider.ProviderName)}
                    {Row("Prescription", prescription.PrescriptionFileName)}
                    {Row("Recipient", prescription.RecipientFirstLastName ?? "—")}
                    {Row("Amount (XOF)", $"{providerPayment.AmountXOF:N0} XOF")}
                    {Row("Method", providerPayment.PaymentMethod.ToString())}
                    {Row("Reference", providerPayment.ReferenceNumber ?? "—")}
                    {Row("Receipt", string.IsNullOrWhiteSpace(providerPayment.ReceiptUrl) ? "—" : $"<a href='{providerPayment.ReceiptUrl}'>View Receipt</a>")}
                    {Row("Notes", providerPayment.Notes ?? "—")}
                    {Row("Paid On", providerPayment.PaidAt?.ToString("MMM dd, yyyy HH:mm UTC") ?? "—")}
                """));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send provider paid admin notification for ProviderPayment {Id}", providerPayment.Id);
        }

        if (string.IsNullOrWhiteSpace(provider.Email)) return;

        try
        {
            await email.SendEmailAsync(
                provider.Email,
                "Payment Confirmed — DiaspoDirect",
                Template("Payment Confirmed", $"""
                    <p>Dear {provider.ProviderName},</p>
                    <p>Your payment for the following prescription has been sent.</p>
                    {Row("Prescription", prescription.PrescriptionFileName)}
                    {Row("Recipient", prescription.RecipientFirstLastName ?? "—")}
                    {Row("Amount", $"{providerPayment.AmountXOF:N0} XOF")}
                    {Row("Method", providerPayment.PaymentMethod.ToString())}
                    {Row("Reference", providerPayment.ReferenceNumber ?? "—")}
                    {Row("Date", providerPayment.PaidAt?.ToString("MMM dd, yyyy") ?? "—")}
                    <p style="margin-top:16px">Thank you for your service.</p>
                """));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send provider paid email to {Email}", provider.Email);
        }
    }

    private static string Row(string label, string value) =>
        $"""<p style="margin:6px 0"><strong style="color:#555;min-width:140px;display:inline-block">{label}:</strong> {value}</p>""";

    private static string Template(string title, string body) => $"""
        <!DOCTYPE html>
        <html>
        <body style="margin:0;padding:0;background:#f4f4f4;font-family:Arial,sans-serif">
          <table width="100%" cellpadding="0" cellspacing="0">
            <tr><td align="center" style="padding:30px 15px">
              <table width="600" cellpadding="0" cellspacing="0" style="background:#fff;border-radius:8px;overflow:hidden;box-shadow:0 2px 8px rgba(0,0,0,0.08)">
                <tr><td style="background:#1b6ec2;padding:24px 32px">
                  <h1 style="margin:0;color:#fff;font-size:20px;font-weight:700">DiaspoDirect</h1>
                  <p style="margin:4px 0 0;color:#c8dcf5;font-size:13px">Connecting diaspora with families back home</p>
                </td></tr>
                <tr><td style="padding:32px">
                  <h2 style="margin:0 0 20px;color:#1b1b1b;font-size:18px;border-bottom:2px solid #f0f0f0;padding-bottom:12px">{title}</h2>
                  {body}
                </td></tr>
                <tr><td style="background:#f9f9f9;padding:16px 32px;color:#aaa;font-size:11px;text-align:center;border-top:1px solid #eee">
                  © DiaspoDirect · This is an automated message, please do not reply.
                </td></tr>
              </table>
            </td></tr>
          </table>
        </body>
        </html>
        """;
}
