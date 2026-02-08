namespace Iyzico_Stripe_Strategy.Services.Interfaces
{
    public interface ICurrentUserInformation
    {
        public string? IpAddress { get; }
        public string? UserAgent { get; }
        public string? CorrelationId { get; }
        public bool IsHttps { get; }
        Guid? UserId { get; }
        UserInformation Infos { get; }
    }
}