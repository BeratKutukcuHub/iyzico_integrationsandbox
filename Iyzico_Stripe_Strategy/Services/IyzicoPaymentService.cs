using System.Globalization;
using Iyzico_Stripe_Strategy.Controllers;
using Iyzico_Stripe_Strategy.Domain;
using Iyzico_Stripe_Strategy.Options;
using Iyzico_Stripe_Strategy.Services.Interfaces;
using Iyzico_Stripe_Strategy.UseCase.Repositories;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Interfaces;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Iyzico_Stripe_Strategy.Services
{
    public class IyzicoPaymentService : IIyzicoPaymentService
    {
        private readonly IyzicoOption _opt;
        private readonly ICurrentUserInformation _currentUserInformation;
        private readonly IOrderRepository _orderRepository;

        public IyzicoPaymentService(IOptions<IyzicoOption> opt,
        ICurrentUserInformation currentUserInformation,
        IOrderRepository orderRepository)
        {
            _opt = opt.Value;
            _currentUserInformation = currentUserInformation;
            _orderRepository = orderRepository;
        }

        public async Task<CheckoutResponse?> IyzicoRequestAsync(OrderAndTotalAmount order)
        {
            Console.WriteLine($" Total Price => {order.BasketItemsAndTotalPrice.TotalPrice}");
            CreateCheckoutFormInitializeRequest request = new CreateCheckoutFormInitializeRequest
            {
                Locale = "tr",
                ConversationId = _currentUserInformation.CorrelationId!.ToString(),
                Currency = order.Order.Currency.ToString(),
                Price = order.BasketItemsAndTotalPrice.TotalPrice.ToString("F2", CultureInfo.InvariantCulture),
                PaidPrice = order.BasketItemsAndTotalPrice.TotalPrice.ToString("F2", CultureInfo.InvariantCulture),
                BasketId = order.Order.Id.ToString(),
                CallbackUrl = _opt.CallbackUrl,
            };
            request.Buyer = new Buyer()
            {
                Id = _currentUserInformation.Infos.UserId.ToString(),
                Name = _currentUserInformation.Infos.Name,
                City = _currentUserInformation.Infos.City,
                Country = _currentUserInformation.Infos.Country,
                Surname = _currentUserInformation.Infos.Name,
                GsmNumber = _currentUserInformation.Infos.PhoneNumber,
                Email = _currentUserInformation.Infos.Email,
                Ip = _currentUserInformation.Infos.IpAddress,
                RegistrationAddress = "TR",
                ZipCode = "34000",
                IdentityNumber = "11111111111",
            };
            request.ShippingAddress = new Address
            {
                ContactName = _currentUserInformation.Infos.Username,
                City = _currentUserInformation.Infos.City,
                Country = _currentUserInformation.Infos.Country,
                Description = "TR"
            };
            request.BillingAddress = new Address
            {
                ContactName = _currentUserInformation.Infos.Username,
                City = _currentUserInformation.Infos.City,
                Country = _currentUserInformation.Infos.Country,
                Description = "TR"
            };
            request.BasketItems = order.BasketItemsAndTotalPrice.BasketItems;

            var response = await CheckoutFormInitialize.Create(request, _opt);
            if (response.Status != "success")
                throw new Exception($"Iyzico checkout creation failed: {response.ErrorMessage}");

            return new CheckoutResponse
            (
                OrderId: order.Order.Id.ToString(),
                PaymentUrl: response.PaymentPageUrl
            );
        }
        public async Task<CheckoutForm?> IyzicoRequestIfResponse(string token)
        {
            RetrieveCheckoutFormRequest request = new RetrieveCheckoutFormRequest()
            {
                Token = token
            };
            CheckoutForm form = await CheckoutForm.Retrieve(request, _opt);
            if (form.Status != "success")
            {
                return null;
            }

            var orderId = Guid.Parse(form.BasketId);
            var order = await _orderRepository.GetEntityAsync(orderId);
            if (order == null)
                return null;

            if (order.Status == OrderStatus.Paid || order.Status == OrderStatus.Failed)
                return null;
            return form;
        }
    }
    public record RawInitializeResponse(string Status, string ConversationId, string Token, string ErrorCode,string ErrorMessage);
    public record CheckoutResponse(string OrderId, string PaymentUrl);
}

   
