namespace UpSocial.UpGram.Domain.Entity
{
    public class FollowerRequestingUserEntity : BaseEntity
    {
        public long FollowerRequestingId { get; set; }
        public long UserPK { get; set; }
    }
}
