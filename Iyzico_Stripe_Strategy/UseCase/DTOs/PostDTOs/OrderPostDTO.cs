using Iyzico_Stripe_Strategy.Domain;

namespace Iyzico_Stripe_Strategy.UseCase.DTOs.PostDTOs
{
    public record OrderPostDTO(
        List<ProductWithQuantity> Products,
        CurrencyType Currency
    );
}