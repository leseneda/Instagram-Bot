using Dapper.Contrib.Extensions;

namespace MeConecta.Gram.Domain.Entity
{
    [Table("ActivityLog")]
    public class ActivityLogEntity : BaseEntity
    {
        public long FollowerRequestId { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string ResponseType { get; set; }
        public bool Succeeded { get; set; } = true;
    }
}
