using System.Reflection;
using Iyzico_Stripe_Strategy.Options;
using Iyzico_Stripe_Strategy.UseCase.DTOs.GetDTOs;
using Iyzico_Stripe_Strategy.UseCase.Mappings;
using Iyzico_Stripe_Strategy.UseCase.Repositories;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Base;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Iyzico_Stripe_Strategy.UseCase
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddUseCase(this IServiceCollection services)
        {
            // MongoDB Serialization ayarları
            BsonSerializer.RegisterSerializer(
                new GuidSerializer(GuidRepresentation.Standard)
            );
            
            var conventionPack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
            ConventionRegistry.Register("IgnoreExtraElements", conventionPack, type => true);
            
            services.AddSingleton<IMongoClient>(sp => new MongoClient(
                sp.GetRequiredService<IOptions<MongoOption>>().Value.ConnectionString));
            services.AddScoped(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(
                sp.GetRequiredService<IOptions<MongoOption>>().Value.DatabaseName));
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped(typeof(Persistance<>));
            services.AddAutoMapper(x => x.AddProfile(typeof(MappingProfile)));
            return services;
        }
    }
}