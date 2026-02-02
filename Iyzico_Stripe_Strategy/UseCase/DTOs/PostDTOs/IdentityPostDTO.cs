namespace Iyzico_Stripe_Strategy.UseCase.DTOs.GetDTOs
{
    public record IdentityPostDTO(
        string Username,
        string Email,
        string PhoneNumber,
        string PasswordHash,
        string City,
        string Country
    );
}