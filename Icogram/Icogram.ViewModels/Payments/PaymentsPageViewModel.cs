using System.Collections.Generic;
using Icogram.Models.Payments;
using Icogram.ViewModels.Layout;

namespace Icogram.ViewModels.Payments
{
    public class PaymentsPageViewModel : LayoutViewModel
    {
        public List<Payment> Payments { get; set; }

        public List<PaymentType> PaymentTypes { get; set; }
    }
}