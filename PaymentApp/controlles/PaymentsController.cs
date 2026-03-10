using Microsoft.AspNetCore.Mvc;
using PaymentApp.Dtos;
using PaymentApp.Services;
using System.Text;
using System.Text.Json;

namespace PaymentApp.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ISignatureService _signatureService;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(
        IPaymentService paymentService,
        ISignatureService signatureService,
        ILogger<PaymentsController> logger)
    {
        _paymentService = paymentService;
        _signatureService = signatureService;
        _logger = logger;
    }

    /// <summary>POST /api/payments - Create a new payment</summary>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromHeader(Name = "X-Signature")] string? signature,
        CancellationToken ct)
    {
        Request.EnableBuffering();
        var rawBody = await new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true).ReadToEndAsync(ct);
        Request.Body.Position = 0;

        if (!_signatureService.Validate(rawBody, signature ?? string.Empty))
        {
            return Unauthorized(new { error = "Invalid X-Signature header" });
        }

        CreatePaymentRequest? request;
        try
        {
            request = JsonSerializer.Deserialize<CreatePaymentRequest>(rawBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch
        {
            return BadRequest(new { error = "Invalid JSON body" });
        }

        if (request is null)
            return BadRequest(new { error = "Request body is empty" });

        if (!TryValidateModel(request))
            return BadRequest(ModelState);

        try
        {
            var result = await _paymentService.CreatePaymentAsync(request, ct);
            return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment");
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    /// <summary>GET /api/payments - get payments list</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sort = "desc",
        CancellationToken ct = default)
    {
        var result = await _paymentService.GetPaymentsAsync(page, pageSize, sort, ct);
        return Ok(result);
    }

    /// <summary>GET /api/payments/stats — Aggregated statistics</summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats(CancellationToken ct)
    {
        var result = await _paymentService.GetStatsAsync(ct);
        return Ok(result);
    }
}
