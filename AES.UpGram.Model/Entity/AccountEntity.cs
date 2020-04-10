using Dapper.Contrib.Extensions;

namespace MeConecta.Gram.Domain.Entity
{
    [Table("Account")]
    public class AccountEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
