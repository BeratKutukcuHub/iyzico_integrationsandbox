using AutoMapper;
using Iyzico_Stripe_Strategy.Domain;
using Iyzico_Stripe_Strategy.UseCase.DTOs.GetDTOs;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Iyzico_Stripe_Strategy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Products() => Ok(_mapper.Map<List<ProductGetDTO>>(await _productRepository.GetEntitiesAsync()));
        [HttpGet("{id}")]
        public async Task<IActionResult> Product(Guid id) => Ok(_mapper.Map<ProductGetDTO>(await _productRepository.GetEntityAsync(id)));
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProducPostDTO product) => Ok(await _productRepository.
        AddEntityAsync(_mapper.Map<Product>(product)));
    }
}