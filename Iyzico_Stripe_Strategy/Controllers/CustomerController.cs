using AutoMapper;
using Iyzico_Stripe_Strategy.UseCase.DTOs.GetDTOs;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Iyzico_Stripe_Strategy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        public CustomerController(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Customers() => Ok(_mapper.Map<List<CustomerGetDTO>>(await _customerRepository.GetEntitiesAsync()));
        [HttpGet("{id}")]
        public async Task<IActionResult> Customer(Guid id) => Ok(_mapper.Map<CustomerGetDTO>(await _customerRepository.GetEntityAsync(id)));
        [HttpGet("most-lagging-customer")]
        public async Task<IActionResult> MostLaggingCustomer() 
        => Ok(_mapper.Map<List<CustomerGetDTO>>(await _customerRepository.FindAllAsync(x => x.TotalPaidScore > 0)));
    }
}