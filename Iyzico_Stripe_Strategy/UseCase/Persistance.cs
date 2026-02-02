using MongoDB.Driver;

namespace Iyzico_Stripe_Strategy.UseCase
{
    public sealed class Persistance<TRepository>
    {
        private readonly IMongoDatabase _mongoDatabase;
        public IMongoCollection<TRepository> Collection =>
        _mongoDatabase.GetCollection<TRepository>(typeof(TRepository).Name + " " + "Collection");

        public Persistance(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }
    }
}