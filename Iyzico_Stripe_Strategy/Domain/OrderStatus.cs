namespace Iyzico_Stripe_Strategy.Domain
{
    public enum OrderStatus
    {
        PendingStock,      
        PendingPayment,    
        Paid,              
        Failed,            
        Cancelled          
    }
}