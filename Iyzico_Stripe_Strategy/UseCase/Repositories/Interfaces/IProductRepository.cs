using Iyzico_Stripe_Strategy.Domain;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Base;
using Iyzipay.Model;
using MongoDB.Driver;

namespace Iyzico_Stripe_Strategy.UseCase.Repositories.Interfaces
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task ProductStockReservedIfPaymentProcress(IClientSessionHandle session,
        List<ProductWithQuantity> productsAndQuantities);
        Task<BasketItemsAndTotalPrice> GetBasketItemsAndTotalPrice(IClientSessionHandle session,
        List<ProductWithQuantity> productsAndQuantities);
        Task UpdateProductStocksFinalizedAsync(IClientSessionHandle session, List<ProductWithQuantity> products);
        Task<bool> BulkAddProductAsync(IEnumerable<Product> products);
    }
}