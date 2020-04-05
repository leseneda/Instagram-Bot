using Dapper.Contrib.Extensions;

namespace UpSocial.UpGram.Domain.Entity
{
    [Table("AccountFollower")] 
    public class AccountFollowerEntity: BaseEntity
    {
        public int AccountId { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
    }
}
