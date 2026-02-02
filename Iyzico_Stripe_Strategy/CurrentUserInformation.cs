using System.Security.Claims;

namespace Iyzico_Stripe_Strategy
{
    public class CurrentUserInformation : ICurrentUserInformation
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
        public string? CorrelationId => Items["CorrelationId"]?.ToString()!;
        public bool? IsAuthenticated => User.Identity?.IsAuthenticated;
        public string? Role => User.FindFirstValue(ClaimTypes.Role);
        public bool IsHttps => Request.IsHttps;
    }
}