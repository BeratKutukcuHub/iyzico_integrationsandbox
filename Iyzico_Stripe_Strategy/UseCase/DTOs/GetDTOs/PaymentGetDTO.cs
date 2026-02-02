using Iyzico_Stripe_Strategy.Domain;

namespace Iyzico_Stripe_Strategy.UseCase.DTOs.GetDTOs
{
    public record PaymentGetDTO(
        Guid Id,
        Guid OrderId,
        string IdempotencyKey,
        string Provider,
        string ProviderRef,
        decimal Amount,
        string Currency,
        PaymentStatus Status,
        string RawInitResponse,
        string RawWebhookPayload,
        DateTime? PaidAt
    );
}