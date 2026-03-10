# PaymentAppp

Full-Stack приложение
«Платежная форма»

### Шаги запуска

```bash

# 1. Восстановить зависимости
dotnet restore

# 2. Применить миграции (создаётся payments.db в папке проекта)
dotnet ef database update
# необязательно(но на всякий лучше сделать) т.к. при первом запуске БД создаётся автоматически через db.Database.EnsureCreated()

# 3. Запустить приложение
dotnet run

# Приложение доступно по адресу:
# https://localhost:5000
```

## Механизм защиты (HMAC-SHA256)

Каждый `POST /api/payments` должен сопровождаться заголовком `X-Signature`.

**Алгоритм вычисления:**
```
signature = HMAC-SHA256( body_bytes, utf8(SecretKey) )
            → hex string (lowercase)
```

**Пример на C#:**
```csharp
using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(jsonBody));
var signature = Convert.ToHexString(hash).ToLowerInvariant();
```
