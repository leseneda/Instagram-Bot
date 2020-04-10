using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;
using MeConecta.Gram.Service;

namespace MeConecta.Gram.Console
{
    public class UnFollowRequested
    {
        public void Execute(IInstaConnector connector)
        {
            var basefollower = BaseService<FollowerRequestingEntity>.Builder();
            var follower = basefollower.GetAsync().Result
                .LastOrDefault();

            var followerRequesting = JsonSerializer.Deserialize<IList<long>>(follower.RequestedUserId);
            var unfollowed = connector.User.UnfollowAsync(followerRequesting.ToArray()).Result;

            if (unfollowed.ResponseData.Count > 0)
            {
                var remained = followerRequesting.Except(unfollowed.ResponseData);

                follower.RequestedUserId = (remained.Count() > 0) ? JsonSerializer.Serialize(remained) : null;

                var ret = basefollower.PutAsync(follower).Result;
            }
        }
    }
}
