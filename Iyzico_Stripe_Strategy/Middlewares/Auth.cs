namespace Iyzico_Stripe_Strategy.Middlewares
{
    public class Auth
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public DateTime Expire { get; set; }
    }
}