using System.Collections.Generic;
using Icogram.Models.Abstract;
using Icogram.Models.UserModels;

namespace Icogram.Models.EmailModels
{
    public class EmailTemplate : Entity
    {
        public string TemplateName { get; set; }

        public string TemaplateDescription { get; set; }

        public string TemplateHtml { get; set; }

        public string PreviewTitle { get; set; }

        public string PreviewText { get; set; }

        public ApplicationUser Creator { get; set; }

        public string CreatorId { get; set; }

        public List<EmailVariable> Variables { get; set; }
    }
}