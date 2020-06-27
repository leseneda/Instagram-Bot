using Dapper.Contrib.Extensions;

namespace MeConecta.Gram.Domain.Entity
{
    [Table("FollowerRequest")]
    public class FollowerRequestEntity : BaseEntity
    {
        public long AccountId { get; set; }
        public long AccountUserNameId { get; set; }
        public string FromMaxId { get; set; }
        public string Message { get; set; }
        public string ResponseType { get; set; }
        public string FollowerRequestPk { get; set; }
        public string FollowerRemainPk { get; set; }
        public byte AmountAttemptUnfollowing { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
