using DiaspoDirect.Data;
using DiaspoDirect.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DiaspoDirect.Controllers;

[ApiController]
[Route("api/payments")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly StripeCheckoutService _stripeCheckoutService;
    private readonly ApplicationDbContext _db;

    public PaymentsController(StripeCheckoutService stripeCheckoutService, ApplicationDbContext db)
    {
        _stripeCheckoutService = stripeCheckoutService;
        _db = db;
    }

    [HttpPost("create-checkout-session/{prescriptionId:guid}")]
    public async Task<IActionResult> CreateCheckoutSession(
        Guid prescriptionId,
        [FromBody] CreateCheckoutRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null) return Unauthorized();

        var prescription = await _db.SendPrescriptions.FindAsync(prescriptionId);
        if (prescription is null) return NotFound("Prescription not found.");
        if (prescription.UserId != userId) return Forbid();

        if (prescription.AmountUSD is null)
            return BadRequest("Amount USD is missing.");

        var amountCfa = prescription.AmountCFA;
        var amountUsd = prescription.AmountUSD.Value;
        var feeUsd = 1.50m;

        var sessionUrl = await _stripeCheckoutService.CreateCheckoutSessionAsync(
            prescription,
            userId,
            amountCfa,
            amountUsd,
            feeUsd,
            request.SuccessUrl,
            request.CancelUrl);

        return Ok(new { url = sessionUrl });
    }

    [HttpGet("transaction/{sessionId}")]
    public async Task<IActionResult> GetTransaction(string sessionId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null) return Unauthorized();

        var tx = await _stripeCheckoutService.GetBySessionIdAsync(sessionId);
        if (tx is null) return NotFound();
        if (tx.UserId != userId) return Forbid();

        return Ok(tx);
    }
}

public record CreateCheckoutRequest(
    string SuccessUrl,
    string CancelUrl);
