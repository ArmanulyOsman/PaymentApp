using System.ComponentModel.DataAnnotations;
using PaymentService.API.Models;

namespace PaymentService.Dtos;

//todo add validations
public class CreatePaymentRequest
{
    public string WalletNumber { get; set; } = string.Empty;

    public string Account { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public decimal Amount { get; set; }

    public Currency Currency { get; set; }

    public string? Comment { get; set; }
}

public class PaymentResponse
{
    public Guid Id { get; set; }
    public string WalletNumber { get; set; } = string.Empty;
    public string Account { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string? Comment { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class PaymentsListResponse
{
    public List<PaymentResponse> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

public class DailyStats
{
    public string Date { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal TotalAmount { get; set; }
}

public class PaymentStatsResponse
{
    public decimal TotalAmount { get; set; }
    public int TotalCount { get; set; }
    public List<DailyStats> DailyStats { get; set; } = new();
}
