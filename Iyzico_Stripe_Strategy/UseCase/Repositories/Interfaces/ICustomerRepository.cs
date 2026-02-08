using Iyzico_Stripe_Strategy.Domain;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Base;
using MongoDB.Driver;

namespace Iyzico_Stripe_Strategy.UseCase.Repositories.Interfaces
{
    public interface ICustomerRepository : IRepositoryBase<Customer>
    {
        Task<Guid> AddTransactionSuccessPaidPaymentAsync(IClientSessionHandle session, Customer entity);
    }
}