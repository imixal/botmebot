using System;
using Icogram.Models.Abstract;
using Icogram.Models.CompanyModels;

namespace Icogram.Models.Payments
{
    public class Payment : Entity
    {
        public Company Company { get; set; }

        public int CompanyId { get; set; }

        public string TxTransaction { get; set; }

        public string Comment { get; set; }

        public string Email { get; set; }

        public DateTime PaymentDate { get; set; }

        public string TelegramContact { get; set; }

        public int ChatCount { get; set; }

        public bool IsAproved { get; set; }

        public PaymentType PaymentType { get; set; }

        public int PaymentTypeId { get; set; }
    }
}