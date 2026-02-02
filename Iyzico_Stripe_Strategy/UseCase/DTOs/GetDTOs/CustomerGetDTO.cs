using Iyzico_Stripe_Strategy.Domain;

namespace Iyzico_Stripe_Strategy.UseCase.DTOs.GetDTOs
{
    public record CustomerGetDTO(
        Guid Id,
        Guid IdentityId,
        List<ProductWithQuantity> Products,
        decimal TotalPaidScore,
        string Ip);
}