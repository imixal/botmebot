using Icogram.Models.Abstract;

namespace Icogram.Models.Payments
{
    public class PaymentType : Entity
    {
        public string Text { get; set; }

        public int NumberOfMonth { get; set; }

        public double Eth { get; set; }
    }
}