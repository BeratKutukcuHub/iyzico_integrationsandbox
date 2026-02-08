using System.Globalization;
using Iyzico_Stripe_Strategy.Domain;
using Iyzico_Stripe_Strategy.Services.Interfaces;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Base;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Interfaces;
using Iyzipay.Model;
using MongoDB.Driver;

namespace Iyzico_Stripe_Strategy.UseCase.Repositories
{
    public sealed class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        private readonly IUnitOfWork _uow;
        public ProductRepository(Persistance<Product> persistance, IUnitOfWork uow) : base(persistance)
        {
            _uow = uow;
        }
        public override Task<Product> AddEntityAsync(Product entity)
        {
            return base.AddEntityAsync(entity);
        }
        public async Task<bool> BulkAddProductAsync(IEnumerable<Product> products)
        {
            return await _uow.TransactionOperationAsync<bool>(async session =>
            {
                await _persistance.Collection.InsertManyAsync(session, products);
                return true;
            });
        }
        public async Task ProductStockReservedIfPaymentProcress(IClientSessionHandle session,
        List<ProductWithQuantity> productsAndQuantities)
        {
            var models = productsAndQuantities.Select(item =>
                new UpdateOneModel<Product>(
                    filter: Builders<Product>.Filter.And(
                        Builders<Product>.Filter.Eq(p => p.Id, item.ProductId),
                        Builders<Product>.Filter.Gte(p => p.Stock, item.Quantity) 
                    ),
                    update: Builders<Product>.Update
                        .Inc(p => p.Stock, -item.Quantity)
                        .Inc(p => p.ReservedStock, item.Quantity)
                )
            );
            var result = await _persistance.Collection.BulkWriteAsync(
                session,
                models,
                new BulkWriteOptions { IsOrdered = true }
            );

            if (result.ModifiedCount != productsAndQuantities.Count)
                throw new InvalidOperationException("Insufficient stock for one or more products.");
        }

        public async Task<BasketItemsAndTotalPrice> GetBasketItemsAndTotalPrice(IClientSessionHandle session,
        List<ProductWithQuantity> productsAndQuantities)
        {
            var crud = await _persistance.Collection.Find(session,
            x => productsAndQuantities.Select(y => y.ProductId).Contains(x.Id))
            .ToListAsync();

            List<ProductTotalAmount> result = crud.Select(x => new ProductTotalAmount(x,
            x.Price * productsAndQuantities.Find(y => y.ProductId == x.Id)!.Quantity)).ToList();
            
            if (result.Count != productsAndQuantities.Count)
                throw new InvalidOperationException("Insufficient stock for one or more products.");
            var basketItems = result.Select(x =>
            {
                return new BasketItem()
                {
                    Id = x.Product.Id.ToString(),
                    Name = x.Product.Name,
                    Category1 = x.Product.Category.ToString(),
                    Category2 = x.Product.Category.ToString(),
                    Price = x.Price.ToString("F2", CultureInfo.InvariantCulture),
                    ItemType = BasketItemType.PHYSICAL.ToString(),
                };
            }).ToList();
            decimal totalPrice = result.Sum(x => x.Price);

            return new BasketItemsAndTotalPrice(basketItems, totalPrice);
        }

        public async Task UpdateProductStocksFinalizedAsync(IClientSessionHandle session, List<ProductWithQuantity> products)
        {
            var bulkOps = products.Select(p =>
            new UpdateOneModel<Product>(
            filter: Builders<Product>.Filter.And(
                Builders<Product>.Filter.Eq(x => x.Id, p.ProductId),
                Builders<Product>.Filter.Gte(x => x.ReservedStock, p.Quantity)
            ),
            update: Builders<Product>.Update
                .Inc(x => x.ReservedStock, -p.Quantity)
                .Inc(x => x.Stock, -p.Quantity)
            )
            ).ToList();

            var result = await _persistance.Collection.BulkWriteAsync(
                session,
                bulkOps,
                new BulkWriteOptions { IsOrdered = true }
            );

            if (result.ModifiedCount != products.Count)
                throw new InvalidOperationException("Insufficient stock for one or more products.");
        }
    }
    public record ProductTotalAmount(Product Product, decimal Price);
    public record ProductsAndQuantity(Guid ProductId, int Quantity);
    public record BasketItemsAndTotalPrice(List<BasketItem> BasketItems, decimal TotalPrice);
}