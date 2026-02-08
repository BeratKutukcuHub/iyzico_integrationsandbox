namespace Iyzico_Stripe_Strategy.Middlewares
{
    public class Auth
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public DateTime Expire { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}