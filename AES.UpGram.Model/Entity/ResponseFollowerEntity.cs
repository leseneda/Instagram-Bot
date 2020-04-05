using System.Collections.Generic;

namespace UpSocial.UpGram.Domain.Entity
{
    public class ResponseFollowerEntity
    {
        public ResponseFollowerEntity()
        {
            RequestedUserId = new List<long>();
        }

        public IList<long> RequestedUserId { get; set; } 
        public string NextMaxId { get; set; }
    }
}
