namespace PaymentApp.Models;

public enum PaymentStatus
{
    Created,
    Rejected
}

public enum Currency
{
    RUB,
    USD,
    EUR
}

public class Payment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string WalletNumber { get; set; } = string.Empty;
    public string Account { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }
    public string? Comment { get; set; }
    public PaymentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
