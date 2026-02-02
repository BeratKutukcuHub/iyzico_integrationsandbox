namespace Iyzico_Stripe_Strategy.Domain
{
    public sealed class ProductWithQuantity
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}