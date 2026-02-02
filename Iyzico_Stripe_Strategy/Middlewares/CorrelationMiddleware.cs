
namespace Iyzico_Stripe_Strategy.Middlewares
{
    public class CorrelationMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Request.Headers.TryGetValue("X-Correlation-Id", out var correlationId);
            if (correlationId.Count > 0) await next(context);
            Guid correlationKey = Guid.NewGuid();
            context.Response.Headers.Add("X-Correlation-Id", correlationKey.ToString());
            context.Items.Add("CorrelationId", correlationKey);
            await next(context);
        }
    }
    
}