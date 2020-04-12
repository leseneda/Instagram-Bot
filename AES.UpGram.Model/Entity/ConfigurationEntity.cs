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
        //[Write(false)]
        //public string UserName { get; set; }
        //[Write(false)]
        //public string Password { get; set; }
        ConfigurationEntity()
        {
            Account = new AccountEntity();
        }
    }
}
