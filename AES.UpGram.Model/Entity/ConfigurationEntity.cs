using Dapper.Contrib.Extensions;

namespace MeConecta.Gram.Domain.Entity
{
    [Table("Configuration")]
    public class ConfigurationEntity : BaseEntity
    {
        public int RequestDelay { get; set; }
        public int LogLevel { get; set; }
        public int MaxPagesToLoad { get; set; }

        [Write(false)]
        public AccountEntity Account { get; set; }

        ConfigurationEntity()
        {
            Account = new AccountEntity();
        }
    }
}
