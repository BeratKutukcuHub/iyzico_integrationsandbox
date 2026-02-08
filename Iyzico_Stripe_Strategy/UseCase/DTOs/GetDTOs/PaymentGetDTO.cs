using Iyzico_Stripe_Strategy.Domain;

namespace Iyzico_Stripe_Strategy.UseCase.DTOs.GetDTOs
{
    public record PaymentGetDTO(
        Guid Id,
        Guid OrderId,
        string Provider,
        string ProviderRef,
        decimal Amount,
        string Currency,
        DateTime? PaidAt
    );
}