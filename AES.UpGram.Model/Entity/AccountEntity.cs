using Dapper.Contrib.Extensions;
using System;

namespace UpSocial.UpGram.Domain.Entity
{
    [Table("Account")]
    public class AccountEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
