using System.Security.Claims;
using Iyzico_Stripe_Strategy.Services.Interfaces;

namespace Iyzico_Stripe_Strategy.Services
{
    public sealed class CurrentUserInformation : ICurrentUserInformation
    {
        
        private readonly IHttpContextAccessor _httpContext;
        private ConnectionInfo ConnectionInfo => _httpContext.HttpContext!.Connection;
        private HttpRequest Request => _httpContext.HttpContext!.Request;
        private IDictionary<object, object?> Items => _httpContext.HttpContext!.Items;
        private ClaimsPrincipal User => _httpContext.HttpContext!.User;
        public CurrentUserInformation(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        public string? IpAddress => ConnectionInfo.RemoteIpAddress!.ToString();
        public string? UserAgent => Request.Headers["User-Agent"]!;
        public string? CorrelationId => Items["CorrelationId"]?.ToString();
        public bool? IsAuthenticated => User.Identity?.IsAuthenticated;
        public string? Role => User.FindFirstValue(ClaimTypes.Role);
        public bool IsHttps => Request.IsHttps;
        public Guid? UserId => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId) ? userId : null;
        public UserInformation Infos => new UserInformation(
            IpAddress,
            UserAgent,
            CorrelationId,
            IsAuthenticated,
            Role,
            IsHttps,
            UserId,
            User.FindFirstValue(ClaimTypes.Name),
            User.FindFirstValue(ClaimTypes.MobilePhone),
            User.FindFirstValue(ClaimTypes.Email),
            User.FindFirstValue("City"),
            User.FindFirstValue(ClaimTypes.Country),
            User.FindFirstValue(ClaimTypes.Name),
            User.FindFirstValue(ClaimTypes.Surname)
        );
    }
    public record UserInformation(string? IpAddress,
    string? UserAgent,
    string? CorrelationId,
    bool? IsAuthenticated,
    string? Role,
    bool IsHttps,
    Guid? UserId,
    string? Username,
    string? PhoneNumber,
    string? Email,
    string? City,
    string? Country,
    string? Name,
    string? Surname);
}               