using System.Linq.Expressions;
using Iyzico_Stripe_Strategy.Domain;
using MongoDB.Driver;

namespace Iyzico_Stripe_Strategy.UseCase.Repositories.Base
{
    public class RepositoryBase<TDomain> : 
    IRepositoryBase<TDomain> where TDomain : Entity, new()
    {
        protected readonly Persistance<TDomain> _persistance;

        protected RepositoryBase(Persistance<TDomain> persistance)
        {
            _persistance = persistance;
        }

        public virtual async Task<TDomain> AddEntityAsync(TDomain entity)
        {
            await _persistance.Collection.InsertOneAsync(entity);
            return entity;
        }
        
        protected FilterDefinition<TDomain> GetFilterDefinition(Expression<Func<TDomain, bool>> filter)
        => Builders<TDomain>.Filter.Where(filter);
        
        public virtual async Task<TDomain> UpdateEntityAsync(Guid id, TDomain entity)
        {
            await _persistance.Collection.ReplaceOneAsync(GetFilterDefinition(x => x.Id == id), entity);
            return entity;
        }

        public virtual async Task<bool> DeleteEntityAsync(Guid id)
        {
            var result = await _persistance.Collection.DeleteOneAsync(GetFilterDefinition(x => x.Id == id));
            return result.DeletedCount > 0;
        }

        public virtual async Task<TDomain> GetEntityAsync(Guid id)
        => await _persistance.Collection.Find(GetFilterDefinition(x => x.Id == id)).FirstOrDefaultAsync();

        public virtual async Task<TDomain> FindOneAsync(Expression<Func<TDomain, bool>> filter)
        => await _persistance.Collection.Find(filter).FirstOrDefaultAsync();

        public virtual async Task<List<TDomain>> FindAllAsync(Expression<Func<TDomain, bool>> filter)
        => await _persistance.Collection.Find(filter).ToListAsync();
        
        public virtual async Task<List<TDomain>> GetEntitiesAsync() 
        => await _persistance.Collection.Find(x => true).ToListAsync();
    }
}