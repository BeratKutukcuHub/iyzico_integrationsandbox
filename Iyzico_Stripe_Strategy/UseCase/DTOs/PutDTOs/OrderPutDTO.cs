using Iyzico_Stripe_Strategy.Domain;

namespace Iyzico_Stripe_Strategy.UseCase.DTOs.PutDTOs
{
    public record OrderPutDTO(
        Guid Id,
        OrderStatus Status
    );
}