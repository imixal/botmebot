namespace Icogram.Enums
{
    public class GlobalEnums
    {
        public enum CompanyType
        {
            ManagementCompany = 1,
            CustomerCompany = 2
        }

        public enum ModuleType
        {
            NotSet,
            WelcomeMessageModule,
            CommandModule,
            AntiSpamModule,
            CustomMessageModule
        }

        public enum CommandType
        {
            NotSet = 0,
            PlainMessage = 1,
            PhotoMessage = 2,
            LocationMessage = 3,
            VenueMessage = 4
        }

        public enum FileType
        {
            CommandImage = 1
        }
    }
}