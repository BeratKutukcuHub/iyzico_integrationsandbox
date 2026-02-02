using Iyzico_Stripe_Strategy.Domain;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Base;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Interfaces;

namespace Iyzico_Stripe_Strategy.UseCase.Repositories
{
    public sealed class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(Persistance<Product> persistance) : base(persistance)
        {
        }
    }
}