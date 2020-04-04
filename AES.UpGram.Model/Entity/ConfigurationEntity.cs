using Dapper.Contrib.Extensions;

namespace UpSocial.UpGram.Domain.Entity
{
    [Table("Configuration")]
    public class ConfigurationEntity : BaseEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int RequestDelay { get; set; }
        public int LogLevel { get; set; }
        public int PagingData { get; set; }
    }
}
