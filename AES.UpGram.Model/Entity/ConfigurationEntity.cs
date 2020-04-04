namespace UpSocial.UpGram.Domain.Entity
{
    public class ConfigurationEntity : BaseEntity
    {
        public int RequestDelay { get; set; }
        public int LogLevel { get; set; }
        public int PagingData { get; set; } = 100;
    }
}
