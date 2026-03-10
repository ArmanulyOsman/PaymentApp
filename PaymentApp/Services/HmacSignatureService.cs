using System.Security.Cryptography;
using System.Text;

namespace PaymentApp.Services;

public interface ISignatureService
{
    bool Validate(string body, string signature);
    string Compute(string body);
}

public class HmacSignatureService : ISignatureService
{
    private readonly string _secretKey;
    private readonly ILogger<HmacSignatureService> _logger;

    public HmacSignatureService(IConfiguration config, ILogger<HmacSignatureService> logger)
    {
        _secretKey = config["Hmac:SecretKey"] ?? throw new InvalidOperationException("HMAC secret key not configured");
        _logger = logger;
    }

    public bool Validate(string body, string signature)
    {
        if (string.IsNullOrWhiteSpace(signature))
        {
            _logger.LogWarning("Request missing X-Signature header");
            return false;
        }

        var expected = Compute(body);
        var isValid = string.Equals(expected, signature, StringComparison.OrdinalIgnoreCase);

        if (!isValid)
            _logger.LogWarning("Signature mismatch. Expected: {Expected}, Got: {Got}", expected, signature);

        return isValid;
    }

    public string Compute(string body)
    {
        var keyBytes = Encoding.UTF8.GetBytes(_secretKey);
        var bodyBytes = Encoding.UTF8.GetBytes(body);

        using var hmac = new HMACSHA256(keyBytes);
        var hash = hmac.ComputeHash(bodyBytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
