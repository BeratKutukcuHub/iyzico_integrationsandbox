namespace Iyzico_Stripe_Strategy.Domain
{
    public sealed class Product : Entity
    {
        public string Name { get; set; }
        public ProductCategories Category { get; set; } = ProductCategories.Others;
        public ProductType Type { get; set; } = ProductType.Physical;
        public decimal Price { get; set; } = 0;
        public int Stock { get; set; } = 0;
        public int ReservedStock { get; set; } = 0;
        public int AvailableStock => Stock - ReservedStock;
        public decimal CalculatePrice(int quantity) => quantity * Price;
    }
}