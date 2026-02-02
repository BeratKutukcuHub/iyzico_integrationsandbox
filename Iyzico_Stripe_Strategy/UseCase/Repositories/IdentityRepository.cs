using System.Security.Claims;
using Iyzico_Stripe_Strategy.Domain;
using Iyzico_Stripe_Strategy.Domain.Hash;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Base;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Interfaces;
using MongoDB.Driver;

namespace Iyzico_Stripe_Strategy.UseCase.Repositories
{
    public sealed class IdentityRepository : RepositoryBase<Identity>, IIdentityRepository
    {
        public IdentityRepository(Persistance<Identity> persistance) : base(persistance)
        {
        }
        public override Task<Identity> AddEntityAsync(Identity entity)
        {
            entity.PasswordHash = PasswordHasher.HashPassword(entity.PasswordHash);
            return base.AddEntityAsync(entity);
        }
        public async Task<Identity?> Login(Identity entity)
        {
            var filt = GetFilterDefinition(x => x.Email == entity.Email);
            var result = await _persistance.Collection.Find(filt).FirstOrDefaultAsync();
            if(PasswordHasher.VerifyPassword(entity.PasswordHash, result.PasswordHash))
            {
                return result;
            }return null;
        }
    }
}