using Dapper.Contrib.Extensions;

namespace UpSocial.UpGram.Domain.Entity
{
    public abstract class BaseEntity
    {
        [Key]
        public virtual int Id { get; set; }
    }
}
