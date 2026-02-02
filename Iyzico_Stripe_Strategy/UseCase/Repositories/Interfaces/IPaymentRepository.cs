using Iyzico_Stripe_Strategy.Domain;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Base;

namespace Iyzico_Stripe_Strategy.UseCase.Repositories.Interfaces
{
    public interface IPaymentRepository : IRepositoryBase<Payment> { }
}