using Dapper.Contrib.Extensions;

namespace MeConecta.Gram.Domain.Entity
{
    [Table("AccountHashtag")]
    public class AccountHashtagEntity : BaseEntity
    {
        public int AccountId { get; set; }
        public string Hashtag { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
