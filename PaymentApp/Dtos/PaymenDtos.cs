using System.ComponentModel.DataAnnotations;
using PaymentApp.Models;

namespace PaymentApp.Dtos;

public class CreatePaymentRequest
{
    [Required(ErrorMessage = "Wallet number is required")]
    [StringLength(50, MinimumLength = 1)]
    public string WalletNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Account is required")]
    [StringLength(100, MinimumLength = 1)]
    public string Account { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid phone format")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be positive")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Currency is required")]
    public Currency Currency { get; set; }

    [StringLength(500)]
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
