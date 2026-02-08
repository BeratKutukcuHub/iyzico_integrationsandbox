using Iyzico_Stripe_Strategy.Domain;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Base;
using MongoDB.Driver;

namespace Iyzico_Stripe_Strategy.UseCase.Repositories.Interfaces
{
    public interface IPaymentRepository : IRepositoryBase<Payment>
    {
        Task<Guid> AddTransactionSuccessPaidPaymentAsync(IClientSessionHandle session, Payment entity);
    }
}