using DiaspoDirect.Data;
using DiaspoDirect.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace DiaspoDirect.Controllers;

[ApiController]
[Route("api/stripe")]
public class StripeWebhookController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly StripeCheckoutService _stripeCheckoutService;

    public StripeWebhookController(
        IConfiguration configuration,
        StripeCheckoutService stripeCheckoutService)
    {
        _configuration = configuration;
        _stripeCheckoutService = stripeCheckoutService;
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                _configuration["Stripe:WebhookSecret"]
            );

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Session;
                if (session != null)
                {
                    await _stripeCheckoutService.MarkPaidAsync(
                        session.Id,
                        session.PaymentIntentId
                    );
                }
            }

            return Ok();
        }
        catch (StripeException ex)
        {
            return BadRequest($"Stripe webhook error: {ex.Message}");
        }
    }
}
