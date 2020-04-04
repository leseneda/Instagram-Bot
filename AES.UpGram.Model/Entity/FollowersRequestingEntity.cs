using Dapper.Contrib.Extensions;
using System;

namespace UpSocial.UpGram.Domain.Entity
{
    [Table("FollowersRequesting")]
    public class FollowersRequestingEntity : BaseEntity
    {
        public int AccountId { get; set; }
        public string UserName { get; set; }
        public string FromMaxId { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
