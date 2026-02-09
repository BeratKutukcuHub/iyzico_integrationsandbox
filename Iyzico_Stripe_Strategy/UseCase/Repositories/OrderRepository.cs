using AutoMapper;
using Iyzico_Stripe_Strategy.Controllers;
using Iyzico_Stripe_Strategy.Domain;
using Iyzico_Stripe_Strategy.Services.Interfaces;
using Iyzico_Stripe_Strategy.UseCase.DTOs.PostDTOs;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Base;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Interfaces;
using Iyzipay.Model;
using MongoDB.Driver;

namespace Iyzico_Stripe_Strategy.UseCase.Repositories
{
    public sealed class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserInformation _currentUserInformation;
        public OrderRepository(
        Persistance<Order> persistance,
        IProductRepository productRepository,
        IUnitOfWork uow,
        ICustomerRepository customerRepository,
        IPaymentRepository paymentRepository,
        ICurrentUserInformation currentUserInformation,
        IMapper mapper) : base(persistance)
        {
            _productRepository = productRepository;
            _uow = uow;
            _customerRepository = customerRepository;
            _paymentRepository = paymentRepository;
            _currentUserInformation = currentUserInformation;
            _mapper = mapper;
        }
        public async Task<Order> UpdateStatusPendingAsync(IClientSessionHandle session, Order entity, OrderStatus status)
        {
            var result = await _persistance.Collection.UpdateOneAsync(
                session, GetFilterDefinition(x => x.Id == entity.Id), Builders<Order>.Update.Set(o => o.Status, status)
            );
            if (result.ModifiedCount == 0) throw new Exception("Order not found");
            return entity;
        }
        public async Task UpdateProductStocksFinalizedAsync(IClientSessionHandle session, List<ProductWithQuantity> products)
        => await _productRepository.UpdateProductStocksFinalizedAsync(session, products);

        public async Task<OrderAndTotalAmount> AddEntityAndStockReservedIfPaymentAsync(OrderPostDTO entity)
        {
            return await _uow.TransactionOperationAsync(async x =>
            {
                Order order = _mapper.Map<Order>(entity);
                order.Status = OrderStatus.PendingStock;
                var addedOrder = await base.AddEntityAsync(order);
                
                await _productRepository.ProductStockReservedIfPaymentProcress(x, entity.Products);

                return new OrderAndTotalAmount(await UpdateStatusPendingAsync(x, addedOrder, OrderStatus.PendingPayment),
                await _productRepository.GetBasketItemsAndTotalPrice(x, entity.Products));
            }
            );
        }
        public async Task SuccessPaymentCustomerAndPaymentUpdateAsync(IClientSessionHandle session,
        Guid orderId,
        Guid customerId,
        Guid paymentId)
        {
            var result = await _persistance.Collection.UpdateOneAsync(session, x => x.Id == orderId,
            Builders<Order>.Update.Set(o => o.CustomerId, customerId).Set(o => o.PaymentId, paymentId));
            if(result.ModifiedCount == 0) throw new Exception("Order not found");
        }
        public async Task SuccessPaymentPaidAsync(CheckoutForm orderPaymentIyzico)
        {
            await _uow.TransactionOperationAsync<bool>(async x =>
            {
                bool parsing = Guid.TryParse(orderPaymentIyzico.BasketId, out var guid);
                if(parsing == false) throw new Exception("BasketId not found");
                var result = await GetEntityAsync(guid);
                
                var customerId = await _customerRepository.AddTransactionSuccessPaidPaymentAsync(x, new Customer
                {
                    Products = result.Products,
                    Ip = _currentUserInformation.Infos.IpAddress!,
                    IdentityId = result.CustomerId,
                    TotalPaidScore = result.TotalAmount
                });
                var paymentId = await _paymentRepository.AddTransactionSuccessPaidPaymentAsync(x, new Domain.Payment
                {
                    Amount = result.TotalAmount,
                    Currency = result.Currency,
                    OrderId = result.Id,
                    PaidAt = DateTime.UtcNow,
                    Provider = "IYZICO",
                    ProviderRef = orderPaymentIyzico.ConversationId,
                });
                await UpdateStatusPendingAsync(x, result, OrderStatus.Paid);
                await SuccessPaymentCustomerAndPaymentUpdateAsync(x,
                guid, customerId, paymentId);
                await _productRepository.UpdateProductStocksFinalizedAsync(x, result.Products);
                return true;
            });
        }
    }
    public record OrderAndTotalAmount(Order Order, BasketItemsAndTotalPrice BasketItemsAndTotalPrice);
}