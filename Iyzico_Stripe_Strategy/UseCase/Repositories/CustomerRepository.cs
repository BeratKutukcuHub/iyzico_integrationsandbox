using Iyzico_Stripe_Strategy.Domain;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Base;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Interfaces;

namespace Iyzico_Stripe_Strategy.UseCase.Repositories
{
    public sealed class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(Persistance<Customer> persistance) : base(persistance)
        {
        }
    }
}