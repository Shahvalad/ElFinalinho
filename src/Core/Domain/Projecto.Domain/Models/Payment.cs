namespace Projecto.Domain.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public string UserId { get; set; } 
        public AppUser User { get; set; } 
        public string SessionId { get; set; }
        public long? Amount { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethodCountry { get; set; }
        public string PaymentMethodLast4 { get; set; }
    }

}
