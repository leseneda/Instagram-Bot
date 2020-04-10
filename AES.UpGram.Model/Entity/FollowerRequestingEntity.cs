using Dapper.Contrib.Extensions;

namespace MeConecta.Gram.Domain.Entity
{
    [Table("FollowerRequesting")]
    public class FollowerRequestingEntity : BaseEntity
    {
        public int AccountId { get; set; }
        public long AccountFollowerId { get; set; }
        public string FromMaxId { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string ResponseType { get; set; }
        public string RequestedUserId { get; set; }
    }
}
