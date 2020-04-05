using Dapper.Contrib.Extensions;

namespace UpSocial.UpGram.Domain.Entity
{
    [Table("FollowerRequestingUser")]
    public class FollowerRequestingUserEntity : BaseEntity
    {
        public long FollowerRequestingId { get; set; }
        public long UserPK { get; set; }
    }
}
