namespace MeConecta.Gram.Domain.Enum
{
    public class ActivityTypeEnum : Enumeration
    {
        public static readonly ActivityTypeEnum FollowerByAccountName = new ActivityTypeEnum(1, "Request follower by account name");
        public static readonly ActivityTypeEnum FollowerByHashtag = new ActivityTypeEnum(2, "Request follower by hashtag");
        public static readonly ActivityTypeEnum FollowerByLocation = new ActivityTypeEnum(3, "Request follower by location");

        public static readonly ActivityTypeEnum Unfollow = new ActivityTypeEnum(100, "Unfollow");

        private ActivityTypeEnum(short id, string name) : base(id, name)
        {
        }
    }
}
