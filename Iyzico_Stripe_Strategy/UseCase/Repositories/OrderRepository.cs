using Iyzico_Stripe_Strategy.Domain;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Base;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Interfaces;

namespace Iyzico_Stripe_Strategy.UseCase.Repositories
{
    public sealed class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(Persistance<Order> persistance) : base(persistance)
        {
        }
    }
}