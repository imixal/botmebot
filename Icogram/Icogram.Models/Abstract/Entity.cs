using System;

namespace Icogram.Models.Abstract
{
    public class Entity : IEntity<int>
    {
        public int Id { get; set; }

        public DateTime? CreationDate { get; set; }

    }
}