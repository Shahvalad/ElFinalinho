namespace Projecto.Domain.Models.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public void Delete() => IsDeleted = true;
        public void Restore() => IsDeleted = false;
    }
}
