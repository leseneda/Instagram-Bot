using InstagramApiSharp.Classes;

namespace MeConecta.Gram.Domain.Entity
{
    public class ResponseEntity<T>
    {
        public bool Succeeded { get; set; } = true;
        public string Message { get; set; } = "No errors detected";
        public ResponseType ResponseType { get; set; } = ResponseType.OK;
        public T ResponseData { get; set; }
    }
}
