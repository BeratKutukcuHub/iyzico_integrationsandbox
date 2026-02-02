using Iyzico_Stripe_Strategy.Domain;

namespace Iyzico_Stripe_Strategy.UseCase.DTOs.GetDTOs
{
    public record OrderPostDTO(
        Guid CustomerId,
        decimal TotalAmount,
        CurrencyType Currency,
        OrderStatus Status,
        DateTime? PaidAt,
        Guid? PaymentId
    );
}