namespace UpSocial.UpGram.Domain.Entity
{
    public class ResponseBaseEntity<T>
    {
        public bool Succeeded { get; set; } = true;
        public string Message { get; set; }
        public T ResponseData { get; set; }
    }
}
