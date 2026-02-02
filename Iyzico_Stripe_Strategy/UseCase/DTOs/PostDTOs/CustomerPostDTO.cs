using Iyzico_Stripe_Strategy.Domain;

namespace Iyzico_Stripe_Strategy.UseCase.DTOs.PostDTOs
{
    public record CustomerPostDTO(
        Guid IdentityId,
        List<ProductWithQuantity> Products,
        decimal TotalPaidScore
    );

}