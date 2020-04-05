using Dapper.Contrib.Extensions;

namespace UpSocial.UpGram.Domain.Entity
{
    [Table("Configuration")]
    public class ConfigurationEntity : BaseEntity
    {
        [Write(false)]
        public string UserName { get; set; }
        [Write(false)]
        public string Password { get; set; }
        public int RequestDelay { get; set; }
        public int LogLevel { get; set; }
        public int MaxPagesToLoad { get; set; }
    }
}
