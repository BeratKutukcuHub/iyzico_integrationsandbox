using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Iyzico_Stripe_Strategy.Domain;

namespace Iyzico_Stripe_Strategy.Middlewares
{
    public static class AuthTokenGenerater
    {
        const string Secret = "my-super-secret-key-guarding-panel";
        public static string GenerateToken(Identity identity, string role)
        {
            var payload = JsonSerializer.Serialize(new Auth
            {
                Id = identity.Id,
                Username = identity.Username,
                PhoneNumber = identity.PhoneNumber,
                Role = role,
                Expire = DateTime.Now.AddMinutes(15)
            });
            var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payload));

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Secret));
            var signature = Convert.ToBase64String(
                hmac.ComputeHash(Encoding.UTF8.GetBytes("jwt" + payloadBase64))
            );

            return $"{payloadBase64}.{signature}";
        }
        public static ClaimsPrincipal? Validate(string token)
        {
            var parts = token.Split('.');
            if (parts.Length != 2) return null;

            var payloadBase64 = parts[0];
            var signature = parts[1];

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Secret));
            var expectedSignature = Convert.ToBase64String(
                hmac.ComputeHash(Encoding.UTF8.GetBytes("jwt" + payloadBase64))
            );

            if (signature != expectedSignature) return null;

            var json = Encoding.UTF8.GetString(Convert.FromBase64String(payloadBase64));
            var data = JsonSerializer.Deserialize<Auth>(json)!;
            
            if (data.Expire < DateTime.UtcNow) return null;
            
            return new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, data.Id.ToString()),
                new Claim(ClaimTypes.Role, data.Role),
                new Claim(ClaimTypes.Name, data.Username),
                new Claim(ClaimTypes.MobilePhone, data.PhoneNumber)
            }));
        }
    }
}