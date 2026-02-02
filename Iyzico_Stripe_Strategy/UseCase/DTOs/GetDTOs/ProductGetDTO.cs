using Iyzico_Stripe_Strategy.Domain;

namespace Iyzico_Stripe_Strategy.UseCase.DTOs.GetDTOs
{
    public record ProductGetDTO(
        Guid Id,
        string Name,
        decimal Price,
        ProductCategories Category,
        ProductType Type,
        int Stock,
        int ReservedStock,
        int AvailableStock
    );
}