using Iyzico_Stripe_Strategy.Domain;

namespace Iyzico_Stripe_Strategy.UseCase.DTOs.GetDTOs
{
    public record ProducPostDTO(
        string Name,
        ProductCategories Category,
        ProductType Type,
        decimal Price,
        int Stock
    );
}