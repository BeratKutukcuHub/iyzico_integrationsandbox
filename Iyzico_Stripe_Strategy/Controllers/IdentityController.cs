using AutoMapper;
using Iyzico_Stripe_Strategy.Domain;
using Iyzico_Stripe_Strategy.Middlewares;
using Iyzico_Stripe_Strategy.UseCase.DTOs.GetDTOs;
using Iyzico_Stripe_Strategy.UseCase.DTOs.PutDTOs;
using Iyzico_Stripe_Strategy.UseCase.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iyzico_Stripe_Strategy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class IdentityController : ControllerBase
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly IMapper _mapper;

        public IdentityController(IIdentityRepository identityRepository, IMapper mapper)
        {
            _identityRepository = identityRepository;
            _mapper = mapper;
        }
        [Authorize(Roles = "user")]
        [HttpGet]
        public async Task<IActionResult> Identities()
        => Ok(await _identityRepository.GetEntitiesAsync());
        [Authorize(Roles = "user")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Identity(Guid id)
        => Ok(await _identityRepository.GetEntityAsync(id));
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] IdentityPostDTO identity)
        => Ok(await _identityRepository.AddEntityAsync(_mapper.Map<Identity>(identity)));
        [HttpPut]
        public async Task<IActionResult> ReplaceInformation([FromQuery] Guid id, [FromBody] IdentityPutDTO identity) => Ok(
            await _identityRepository.UpdateEntityAsync(id, _mapper.Map<Identity>(identity))
        );
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromQuery] LoginDTO identity)
        {
            var result = await _identityRepository.Login(_mapper.Map<Identity>(identity));
            if (result is not null) return Ok(new
            {
                token = AuthTokenGenerater.GenerateToken(result, "user"),
                user = _mapper.Map<IdentityGetDTO>(result)
            });
            return BadRequest("Invalid User");
        }

    }
    public record LoginDTO(string Email, string PasswordHash);
}