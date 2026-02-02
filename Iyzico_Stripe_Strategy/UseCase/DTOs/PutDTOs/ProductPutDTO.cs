using Iyzico_Stripe_Strategy.Domain;

namespace Iyzico_Stripe_Strategy.UseCase.DTOs.PutDTOs
{
    public record ProductPutDTO(
        Guid Id,
        string? Name,
        decimal? Price,
        ProductCategories Category,
        ProductType Type,
        int? Stock,
        int? ReservedStock
    );
}