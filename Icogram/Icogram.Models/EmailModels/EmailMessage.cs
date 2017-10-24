using Icogram.Models.Abstract;
using Icogram.Models.UserModels;

namespace Icogram.Models.EmailModels
{
    public class EmailMessage : Entity
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public ApplicationUser Sender { get; set; }

        public string SenderId { get; set; }

        public EmailTemplate Template { get; set; }

        public int TemplateId { get; set; }
    }
}