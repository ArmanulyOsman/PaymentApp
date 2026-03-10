using Microsoft.EntityFrameworkCore;
using PaymentApp.DB;
using PaymentApp.Models;

namespace PaymentApp.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDbContext _context;

    public PaymentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Payment> CreateAsync(Payment payment, CancellationToken ct = default)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync(ct);
        return payment;
    }

    public async Task<(List<Payment> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, string? sortOrder = "desc", CancellationToken ct = default)
    {
        var query = _context.Payments.AsQueryable();

        query = sortOrder == "asc"
            ? query.OrderBy(p => p.CreatedAt)
            : query.OrderByDescending(p => p.CreatedAt);

        var total = await query.CountAsync(ct);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<(decimal TotalAmount, int TotalCount, List<(DateTime Date, int Count, decimal Amount)> Daily)>
        GetStatsAsync(CancellationToken ct = default)
    {
        var payments = await _context.Payments.ToListAsync(ct);

        var totalAmount = payments.Sum(p => p.Amount);
        var totalCount = payments.Count;

        var daily = payments
            .GroupBy(p => p.CreatedAt.Date)
            .Select(g => (Date: g.Key, Count: g.Count(), Amount: g.Sum(p => p.Amount)))
            .OrderByDescending(x => x.Date)
            .ToList();

        return (totalAmount, totalCount, daily);
    }
}
