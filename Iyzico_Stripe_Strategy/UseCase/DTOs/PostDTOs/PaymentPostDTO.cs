using Iyzico_Stripe_Strategy.Domain;

namespace Iyzico_Stripe_Strategy.UseCase.DTOs.GetDTOs
{
    public record PaymentPostDTO(
        Guid OrderId,
        string Provider,
        string ProviderRef,
        decimal Amount,
        CurrencyType Currency,
        PaymentStatus Status,
        string RawInitResponse,
        string RawWebhookPayload,
        DateTime? PaidAt
    );
}