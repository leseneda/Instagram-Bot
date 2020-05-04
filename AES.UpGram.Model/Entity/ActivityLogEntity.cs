using Dapper.Contrib.Extensions;

namespace MeConecta.Gram.Domain.Entity
{
    [Table("ActivityLog")]
    public class ActivityLogEntity : BaseEntity
    {
        public long ActivityId { get; set; }
        public short ActivityType { get; set; }
        public long AccountId { get; set; }
        public string Message { get; set; }
        public string ResponseType { get; set; }
        public bool Succeeded { get; set; } = true;
    }
}
