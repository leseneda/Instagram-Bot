using System;

namespace UpSocial.UpGram.Domain.Entity
{
    public class ResponseEntity
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}
