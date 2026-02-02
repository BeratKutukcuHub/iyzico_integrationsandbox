using AutoMapper;
using Iyzico_Stripe_Strategy.UseCase.DTOs.GetDTOs;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Iyzico_Stripe_Strategy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public OrderController(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Orders() => Ok(_mapper.Map<List<OrderGetDTO>>(await _orderRepository.GetEntitiesAsync()));
        [HttpGet("{id}")]
        public async Task<IActionResult> Order(Guid id) => Ok(_mapper.Map<OrderGetDTO>(await _orderRepository.GetEntityAsync(id)));
        [HttpPost]
        public async Task<IActionResult> OrderComplaint([FromQuery] Guid id, ComplaintLevel complaint) 
        => Ok(await _orderRepository.FindOneAsync(x => x.Id == id));
    }
    public enum ComplaintLevel { NotCompleted, NotRefunded, Other }
}