using System;

namespace UpSocial.UpGram.Domain.Entity
{
    public class ResponseEntity<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public T ResponseData { get; set; }
    }
}
