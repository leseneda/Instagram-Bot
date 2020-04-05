using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using UpSocial.UpGram.Core;
using UpSocial.UpGram.Domain.Entity;
using UpSocial.UpGram.Service;

namespace UpSocial.UpGram.Console
{
    public class UnFollowRequested
    {
        public void Execute(Connector connector)
        {
            var followerService = new BaseService<FollowerRequestingEntity>();
            var follower = followerService.GetAsync().Result.LastOrDefault();

            var followerRequesting = JsonSerializer.Deserialize<IList<long>>(follower.RequestedUserId);
            var unfollowed = connector.User.Value.UnFollowAsync(followerRequesting.ToArray()).Result;

            if (unfollowed.ResponseData.Count > 0)
            {
                var remained = followerRequesting.Except(unfollowed.ResponseData);

                follower.RequestedUserId = (remained.Count() > 0) ? JsonSerializer.Serialize(remained) : null;

                var ret = followerService.PutAsync(follower).Result;
            }
        }
    }
}
