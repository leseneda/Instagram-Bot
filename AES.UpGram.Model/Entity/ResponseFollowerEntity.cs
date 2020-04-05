using System.Collections.Generic;

namespace UpSocial.UpGram.Domain.Entity
{
    public class ResponseFollowerEntity
    {
        public ResponseFollowerEntity()
        {
            RequestedUserPk = new List<long>();
        }

        public IList<long> RequestedUserPk { get; set; } 
        public string NextMaxId { get; set; }
    }
}
