using PaymentApp.Dtos;
using PaymentApp.Models;
using PaymentApp.Repositories;

namespace PaymentApp.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _repository;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(IPaymentRepository repository, ILogger<PaymentService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<PaymentResponse> CreatePaymentAsync(CreatePaymentRequest request, CancellationToken ct = default)
    {
        var payment = new Payment
        {
            WalletNumber = request.WalletNumber.Trim(),
            Account = request.Account.Trim(),
            Email = request.Email.Trim().ToLowerInvariant(),
            Phone = request.Phone?.Trim(),
            Amount = request.Amount,
            Currency = request.Currency,
            Comment = request.Comment?.Trim(),
            Status = PaymentStatus.Created,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(payment, ct);
        _logger.LogInformation("Payment {Id} created for user: {Account}", created.Id, created.Account);

        return MapToResponse(created);
    }

    public async Task<PaymentsListResponse> GetPaymentsAsync(
        int page, int pageSize, string? sortOrder, CancellationToken ct = default)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var (items, total) = await _repository.GetAllAsync(page, pageSize, sortOrder, ct);

        return new PaymentsListResponse
        {
            Items = items.Select(MapToResponse).ToList(),
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        };
    }

    private static PaymentResponse MapToResponse(Payment p) => new()
    {
        Id = p.Id,
        WalletNumber = p.WalletNumber,
        Account = p.Account,
        Email = p.Email,
        Phone = p.Phone,
        Amount = p.Amount,
        Currency = p.Currency.ToString(),
        Comment = p.Comment,
        Status = p.Status.ToString(),
        CreatedAt = p.CreatedAt
    };
}
