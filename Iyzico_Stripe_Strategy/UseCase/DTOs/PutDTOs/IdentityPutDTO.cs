namespace Iyzico_Stripe_Strategy.UseCase.DTOs.PutDTOs
{
    public record IdentityPutDTO(
        Guid Id,
        string? Username,
        string? Email,
        string? PasswordHash,
        string? PhoneNumber,
        string? City,
        string? Country
    );
}