using MongoDB.Driver;

namespace Iyzico_Stripe_Strategy.Services.Interfaces
{
    public interface IUnitOfWork
    {
        Task<IClientSessionHandle> StartSessionAsync();
        void StartTransactionAsync(IClientSessionHandle session);
        Task CommitTransactionAsync(IClientSessionHandle session);
        Task AbortTransactionAsync(IClientSessionHandle session);
        Task<T> TransactionOperationAsync<T>(Func<IClientSessionHandle, Task<T>> operation,
        CancellationToken cancellationToken = default);
    }
}