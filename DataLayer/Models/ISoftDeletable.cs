namespace DataLayer.Models
{
    public interface ISoftDeletable
    {
        int Id { get; set; }
        bool SoftDeleted { get; set; }
    }
}
