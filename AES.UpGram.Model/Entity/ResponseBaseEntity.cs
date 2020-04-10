namespace MeConecta.Gram.Domain.Entity
{
    public class ResponseEntity<T>
    {
        public bool Succeeded { get; set; } = true;
        public string Message { get; set; }
        public T ResponseData { get; set; }
    }
}
