using System.Text.Json;
using AutoMapper;
using Iyzico_Stripe_Strategy.Services.Interfaces;
using Iyzico_Stripe_Strategy.UseCase.DTOs.GetDTOs;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Interfaces;
using Iyzipay.Model;
using Iyzipay.Request;
using Iyzipay.Request.V2.Subscription;
using Microsoft.AspNetCore.Mvc;

namespace Iyzico_Stripe_Strategy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IIyzicoPaymentService _iyzicoService;
        private readonly IMapper _mapper;

        public PaymentController(IPaymentRepository paymentRepository,
        IMapper mapper,
        IOrderRepository orderRepository,
        IIyzicoPaymentService iyzicoService)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _orderRepository = orderRepository;
            _iyzicoService = iyzicoService;
        }

        [HttpGet]
        public async Task<IActionResult> Payments() => Ok(_mapper.Map<List<PaymentGetDTO>>(await _paymentRepository.GetEntitiesAsync()));

        [HttpPost("webhook/iyzico")]
        public async Task<IActionResult> IyzicoRequest([FromForm] string token)
        {
            CheckoutForm? result = await _iyzicoService.IyzicoRequestIfResponse(token);
            if(result is null) return BadRequest();

            await _orderRepository.SuccessPaymentPaidAsync(result);
            return Ok(result);
        }
    }
    
    public class IyzicoWebhookRequest
    {
        public string ConversationId { get; set; }
        public string PaymentId { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public decimal PaidPrice { get; set; }
        public decimal Price { get; set; }
        public string MdStatus { get; set; }
        public string FraudStatus { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
    public record OrderPaymentIyzico(IyzicoWebhookRequest request, Guid orderId);
}