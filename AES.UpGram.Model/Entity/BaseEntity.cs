using Dapper.Contrib.Extensions;
using System;

namespace MeConecta.Gram.Domain.Entity
{
    public abstract class BaseEntity
    {
        [Key]
        public virtual int Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? UpdatedOn { get; set; }
    }
}
