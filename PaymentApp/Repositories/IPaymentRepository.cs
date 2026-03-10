using PaymentApp.Models;

namespace PaymentApp.Repositories;

public interface IPaymentRepository
{

    Task<Payment> CreateAsync(Payment payment, CancellationToken ct = default);
    Task<(List<Payment> Items, int TotalCount)> GetAllAsync(int page, int pageSize, string? sortOrder = "desc", CancellationToken ct = default);
    
}
