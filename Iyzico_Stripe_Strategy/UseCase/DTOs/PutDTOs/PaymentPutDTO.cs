using Iyzico_Stripe_Strategy.Domain;

namespace Iyzico_Stripe_Strategy.UseCase.DTOs.PutDTOs
{
    public record PaymentPutDTO(
        Guid Id,
        PaymentStatus Status
    );
}