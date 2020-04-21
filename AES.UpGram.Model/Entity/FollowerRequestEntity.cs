using Dapper.Contrib.Extensions;

namespace MeConecta.Gram.Domain.Entity
{
    [Table("FollowerRequest")]
    public class FollowerRequestEntity : BaseEntity
    {
        public int AccountId { get; set; }
        public long AccountFollowerId { get; set; }
        public string FromMaxId { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string ResponseType { get; set; }
        public string FollowerRequestPk { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
