using Iyzico_Stripe_Strategy.Services.Interfaces;
using MongoDB.Driver;

namespace Iyzico_Stripe_Strategy.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoClient _client;

        public UnitOfWork(IMongoClient client)
        {
            _client = client;
        }
        public Task<IClientSessionHandle> StartSessionAsync() => _client.StartSessionAsync();
        public void StartTransactionAsync(IClientSessionHandle session) => session.StartTransaction();
        public Task CommitTransactionAsync(IClientSessionHandle session) => session.CommitTransactionAsync();
        public Task AbortTransactionAsync(IClientSessionHandle session) => session.AbortTransactionAsync();

        public async Task<T> TransactionOperationAsync<T>(Func<IClientSessionHandle, Task<T>> operation,
        CancellationToken cancellationToken = default)
        {
            using IClientSessionHandle session = await StartSessionAsync();
            session.StartTransaction(new TransactionOptions(
                readConcern: ReadConcern.Majority,
                writeConcern: WriteConcern.WMajority
            ));
            try
            {
                var response = await operation.Invoke(session);
                await session.CommitTransactionAsync(cancellationToken);
                return response;
            }
            catch
            {
                if(session.IsInTransaction)
                await session.AbortTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}