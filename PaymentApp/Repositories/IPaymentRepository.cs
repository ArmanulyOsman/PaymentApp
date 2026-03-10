using PaymentApp.Models;

namespace PaymentApp.Repositories;

public interface IPaymentRepository
{

    Task<Payment> CreateAsync(Payment payment, CancellationToken ct = default);
    Task<(List<Payment> Items, int TotalCount)> GetAllAsync(int page, int pageSize, string? sortOrder = "desc", CancellationToken ct = default);
    Task<(decimal TotalAmount, int TotalCount, List<(DateTime Date, int Count, decimal Amount)> Daily)> GetStatsAsync(CancellationToken ct = default);
    
}
