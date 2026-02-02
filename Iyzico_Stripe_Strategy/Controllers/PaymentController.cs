using AutoMapper;
using Iyzico_Stripe_Strategy.UseCase.DTOs.GetDTOs;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Iyzico_Stripe_Strategy.Controllers
{
    public sealed class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public PaymentController(IPaymentRepository paymentRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Payments() => Ok(_mapper.Map<List<PaymentGetDTO>>(await _paymentRepository.GetEntitiesAsync()));
        [HttpPost("webhook/iyzico")]
        public async Task<IActionResult> WebhookPaymentProcress() => Ok();
    }
}