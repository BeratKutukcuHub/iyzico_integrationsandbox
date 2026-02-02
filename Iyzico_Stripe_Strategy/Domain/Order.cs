namespace Iyzico_Stripe_Strategy.Domain
{
    public sealed class Order : Entity
    {
        public Guid CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public CurrencyType Currency { get; set; } = CurrencyType.TRY;
        public OrderStatus Status { get; set; } = OrderStatus.PendingPayment;
        public DateTime? PaidAt { get; set; }
        public Guid? PaymentId { get; set; }
    }
}