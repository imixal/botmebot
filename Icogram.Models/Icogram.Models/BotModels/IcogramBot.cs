﻿using Icogram.Models.Abstract;

namespace Icogram.Models.BotModels
{
    public class IcogramBot : Entity
    {
        public int BotId { get; set; }

        public string Token { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public bool IsPrivacyModeActivated { get; set; }
    }
}