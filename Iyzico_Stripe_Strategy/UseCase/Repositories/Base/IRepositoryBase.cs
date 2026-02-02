using System.Linq.Expressions;
using System.Threading.Tasks;
using Iyzico_Stripe_Strategy.Domain;

namespace Iyzico_Stripe_Strategy.UseCase.Repositories.Base
{
    public interface IRepositoryBase<TDomain> 
    where TDomain : Entity, new()
    {
        Task<TDomain> AddEntityAsync(TDomain entity);
        Task<TDomain> UpdateEntityAsync(Guid id, TDomain entity);
        Task<TDomain> GetEntityAsync(Guid id);
        Task<List<TDomain>> FindAllAsync(Expression<Func<TDomain, bool>> filter);
        Task<TDomain> FindOneAsync(Expression<Func<TDomain, bool>> filter);
        Task<bool> DeleteEntityAsync(Guid id);
        Task<List<TDomain>> GetEntitiesAsync();
    }
}