using PaymentApp.Dtos;

namespace PaymentApp.Services;

public interface IPaymentService
{

    Task<PaymentResponse> CreatePaymentAsync(CreatePaymentRequest request, CancellationToken ct = default);
    Task<PaymentsListResponse> GetPaymentsAsync(int page, int pageSize, string? sortOrder, CancellationToken ct = default);
    Task<PaymentStatsResponse> GetStatsAsync(CancellationToken ct = default);

}
