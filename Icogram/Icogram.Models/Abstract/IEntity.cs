namespace Icogram.Models.Abstract
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}