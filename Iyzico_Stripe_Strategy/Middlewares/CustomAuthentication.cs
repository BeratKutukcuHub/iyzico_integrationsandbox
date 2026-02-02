
using System.Buffers.Text;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Iyzico_Stripe_Strategy.Domain.Hash;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Iyzico_Stripe_Strategy.Middlewares
{
    public class CustomAuthentication : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public CustomAuthentication(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock) { }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("X-MyToken", out var headerValue))
                return AuthenticateResult.Fail("No Token");
            if (!headerValue.ToString().StartsWith("MyToken "))
                return AuthenticateResult.Fail("Invalid Token Type");
            var result = AuthTokenGenerater.Validate(headerValue.ToString().Replace("MyToken ", ""));
            return result is null ? AuthenticateResult.Fail("Illegal Token") : 
            AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(result), Scheme.Name));
        }
    }
}