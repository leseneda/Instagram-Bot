using Dapper.Contrib.Extensions;

namespace MeConecta.Gram.Domain.Entity
{
    [Table("AccountUserName")] 
    public class AccountUserNameEntity : BaseEntity
    {
        public long AccountId { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
