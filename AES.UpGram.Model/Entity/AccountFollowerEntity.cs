using Dapper.Contrib.Extensions;

namespace MeConecta.Gram.Domain.Entity
{
    [Table("AccountFollower")] 
    public class AccountFollowerEntity: BaseEntity
    {
        public int AccountId { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
    }
}
