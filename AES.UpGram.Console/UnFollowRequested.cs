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
        public void Execute(ICoreConnector connector)
        {
            var baseFollow = BaseService<AccountFollowerRequestEntity>.Build();
            var follow = baseFollow.GetAsync().Result.LastOrDefault();
            var followRequesting = JsonSerializer.Deserialize<IList<long>>(follow.FollowerRequestPk);
            
            var unfollowedUsersFail = connector.User.UnfollowAsync(followRequesting.ToArray()).Result;
            var remainedUsers = followRequesting.Except(unfollowedUsersFail.ResponseData);

            follow.FollowerRequestPk = (remainedUsers.Count() > 0) ? JsonSerializer.Serialize(remainedUsers) : null;

            var ret = baseFollow.PutAsync(follow).Result;
        }
    }
}
