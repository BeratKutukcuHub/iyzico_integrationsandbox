using Iyzico_Stripe_Strategy.UseCase.Repositories;
using Iyzipay.Model;

namespace Iyzico_Stripe_Strategy.Services.Interfaces
{
    public interface IIyzicoPaymentService
    {
        Task<CheckoutResponse?> IyzicoRequestAsync(OrderAndTotalAmount order);
        Task<CheckoutForm?> IyzicoRequestIfResponse(string token);
    }
}