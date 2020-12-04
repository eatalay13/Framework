namespace Entities.Models
{
    public interface ISoftDeletedEntity
    {
        bool Deleted { get; set; }
    }
}