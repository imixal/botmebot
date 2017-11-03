namespace Icogram.ViewModels.Command
{
    public class UpdateMyCommandViewModel
    {
        public int Id { get; set; }

        public string CommandName { get; set; }

        public string CommandDescription { get; set; }
        public string ActionMessage { get; set; }

        public bool IsCommandShowInList { get; set; }
    }
}