namespace Iyzico_Stripe_Strategy.Domain
{
    public sealed class Customer : Entity
    {
        public Guid IdentityId { get; set; }
        public List<ProductWithQuantity> Products { get; set; }
        public decimal TotalPaidScore { get; set; }
        public string Ip { get; set; }
    }
}