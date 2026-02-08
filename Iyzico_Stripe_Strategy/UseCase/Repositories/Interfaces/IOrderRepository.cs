using Iyzico_Stripe_Strategy.Controllers;
using Iyzico_Stripe_Strategy.Domain;
using Iyzico_Stripe_Strategy.UseCase.DTOs.PostDTOs;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Base;
using Iyzipay.Model;
using MongoDB.Driver;

namespace Iyzico_Stripe_Strategy.UseCase.Repositories.Interfaces
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        Task<Order> UpdateStatusPendingAsync(IClientSessionHandle session, Order entity, OrderStatus status);
        Task<OrderAndTotalAmount> AddEntityAndStockReservedIfPaymentAsync(OrderPostDTO entity);
        Task UpdateProductStocksFinalizedAsync(IClientSessionHandle session, List<ProductWithQuantity> products);
        Task SuccessPaymentPaidAsync(CheckoutForm orderPaymentIyzico);
        Task SuccessPaymentCustomerAndPaymentUpdateAsync(IClientSessionHandle session,
        Guid orderId,
        Guid customerId,
        Guid paymentId);
    }
}