namespace Iyzico_Stripe_Strategy.Domain
{
    public sealed class Payment : Entity
    {
        public Guid OrderId { get; set; }
        public string IdempotencyKey { get; set; } = string.Empty;
        public string Provider { get; set; } = "IYZICO";
        public string ProviderRef { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public CurrencyType Currency { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public string RawInitResponse { get; set; } = string.Empty;
        public string RawWebhookPayload { get; set; } = string.Empty;
        public DateTime? PaidAt { get; set; }
    }
}