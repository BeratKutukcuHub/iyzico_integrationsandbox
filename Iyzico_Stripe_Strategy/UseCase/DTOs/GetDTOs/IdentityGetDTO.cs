namespace Iyzico_Stripe_Strategy.UseCase.DTOs.GetDTOs
{
    public record IdentityGetDTO(
        Guid Id,
        string Name,
        string Surname,
        string Username,
        string Email,
        string PhoneNumber,
        string City,
        string Country,
        bool IsActive);
}