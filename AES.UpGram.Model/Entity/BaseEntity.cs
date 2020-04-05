using Dapper.Contrib.Extensions;
using System;

namespace UpSocial.UpGram.Domain.Entity
{
    public abstract class BaseEntity
    {
        [Key]
        public virtual int Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
