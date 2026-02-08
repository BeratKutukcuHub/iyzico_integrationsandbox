using Iyzico_Stripe_Strategy.Domain;

namespace Iyzico_Stripe_Strategy.UseCase.DTOs.PostDTOs
{
    public record PaymentPostDTO(
        Guid OrderId,
        string IdempotencyKey,
        string Provider,
        decimal Amount,
        CurrencyType Currency
    );
}
