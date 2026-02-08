using Iyzico_Stripe_Strategy.Domain;
using Iyzico_Stripe_Strategy.UseCase.DTOs.GetDTOs;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Base;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Interfaces;
using MongoDB.Driver;

namespace Iyzico_Stripe_Strategy.UseCase.Repositories
{
    public sealed class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
    {
        public PaymentRepository(Persistance<Payment> persistance) : base(persistance)
        {
        }
        public async Task BulkAddTransactionSuccessPaidPaymentAsync(IClientSessionHandle session, List<Payment> entities) =>
        await _persistance.Collection.InsertManyAsync(session, entities);
        public async Task<Guid> AddTransactionSuccessPaidPaymentAsync(IClientSessionHandle session, Payment entity)
        {
            await _persistance.Collection.InsertOneAsync(session, entity);
            return entity.Id;
        }
    }
}