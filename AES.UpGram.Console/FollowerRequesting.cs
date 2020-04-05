using System.Linq;
using UpSocial.UpGram.Core;
using UpSocial.UpGram.Domain.Entity;
using UpSocial.UpGram.Service;

namespace UpSocial.UpGram.Console
{
    public class FollowerRequesting
    {
        public void Execute(Connector connector, string userNameFrom, int accountId)
        {
            var userResponse = connector.User.Value.UserAsync(userNameFrom).Result;

            if (!userResponse.Succeeded)
            {
                // Log
            }

            var followerRequesting = new BaseService<FollowerRequestingEntity>();
            var follower = followerRequesting.GetAsync().Result.LastOrDefault();

            string fromMaxId = follower?.FromMaxId ?? string.Empty;

            var result = connector.User.Value.FollowAsync(userNameFrom, fromMaxId).Result;

            if (result.Succeeded)
            {
                var response = followerRequesting.PostAsync(
                    new FollowerRequestingEntity()
                    {
                        AccountId = accountId,
                        AccountFollowerId = 1,
                        FromMaxId = result.ResponseData.NextMaxId,
                        Message = result.Message,
                        Succeeded = result.Succeeded,
                        ResponseType = result.ResponseType,
                    })
                    .Result;

                var requestingUser = result.ResponseData.RequestedUserPk
                    .Select(pk => new FollowerRequestingUserEntity()
                    {
                        FollowerRequestingId = follower.Id,
                        UserPK = pk
                    });

                var followerRequested = new BaseService<FollowerRequestingUserEntity>();

                foreach (var user in requestingUser)
                {
                    var ret = followerRequested.PostAsync(user).Result;
                }
            }
            else
            {

            }
        }
    }
}
