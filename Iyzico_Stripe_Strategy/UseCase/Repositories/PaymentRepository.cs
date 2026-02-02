using Iyzico_Stripe_Strategy.Domain;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Base;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Interfaces;

namespace Iyzico_Stripe_Strategy.UseCase.Repositories
{
    public sealed class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
    {
        public PaymentRepository(Persistance<Payment> persistance) : base(persistance)
        {
        }
    }
}