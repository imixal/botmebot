using Icogram.Enums;
using Icogram.Models.Abstract;
using Icogram.Models.ChatModels;

namespace Icogram.Models.FileModel
{
    public class File : Entity
    {
        public string FileName { get; set; }

        public string ContentType { get; set; }

        public byte[] Content { get; set; }

        public GlobalEnums.FileType Type { get; set; }

        public Chat Chat { get; set; }

        public int ChatId { get; set; }
    }
}