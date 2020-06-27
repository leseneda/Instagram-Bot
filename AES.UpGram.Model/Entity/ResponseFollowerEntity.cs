using System.Collections.Generic;

namespace MeConecta.Gram.Domain.Entity
{
    public class ResponseFollowerEntity
    {
        public ResponseFollowerEntity()
        {
            FollowerRequestPk = new List<long>();
            FollowerRemainPk = new List<long>();
        }

        public IList<long> FollowerRequestPk { get; set; }
        public List<long> FollowerRemainPk { get; set; }
        public string NextMaxId { get; set; }
        public string ResponseType { get; set; }
    }
}
