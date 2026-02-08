using AutoMapper;
using Iyzico_Stripe_Strategy.Domain;
using Iyzico_Stripe_Strategy.Services;
using Iyzico_Stripe_Strategy.Services.Interfaces;
using Iyzico_Stripe_Strategy.UseCase.DTOs.GetDTOs;
using Iyzico_Stripe_Strategy.UseCase.DTOs.PostDTOs;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iyzico_Stripe_Strategy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IIyzicoPaymentService _iyzicoService;
        private readonly ICurrentUserInformation _currentUserInformation;
        
        public OrderController(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IMapper mapper,
            IIyzicoPaymentService iyzicoService,
            ICurrentUserInformation currentUserInformation)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _iyzicoService = iyzicoService;
            _currentUserInformation = currentUserInformation;
        }

        [HttpGet]
        public async Task<IActionResult> Orders() => Ok(_mapper.Map<List<OrderGetDTO>>(await _orderRepository.GetEntitiesAsync()));
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Order(Guid id) => Ok(_mapper.Map<OrderGetDTO>(await _orderRepository.GetEntityAsync(id)));
        
        [Authorize(Roles = "user")]
        [HttpPost("request/iyzico")]
        public async Task<IActionResult> RequestIyzicoPayment([FromBody] OrderPostDTO orderDto)
        {
            var response = await _orderRepository.AddEntityAndStockReservedIfPaymentAsync(orderDto);
            CheckoutResponse? iyzicoResponse = await _iyzicoService.IyzicoRequestAsync(response!);
            
            if (iyzicoResponse is null) 
                return BadRequest();

            return Ok(iyzicoResponse);
        }
    }
}