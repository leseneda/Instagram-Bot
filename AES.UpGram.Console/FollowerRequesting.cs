using System.Linq;
using System.Text.Json;
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

            var followerService = new BaseService<FollowerRequestingEntity>();
            var follower = followerService.GetAsync().Result.LastOrDefault(cmp => cmp.AccountId == accountId);

            string fromMaxId = follower?.FromMaxId ?? string.Empty;

            var result = connector.User.Value.FollowAsync(userNameFrom, fromMaxId).Result;

            if (result.Succeeded)
            {
                var followerRequesting = new FollowerRequestingEntity()
                {
                    AccountId = accountId,
                    AccountFollowerId = 1, /// ARRUMA ISSO!!!!
                    FromMaxId = result.ResponseData.NextMaxId,
                    Message = result.Message,
                    Succeeded = result.Succeeded,
                    ResponseType = result.ResponseType,
                    RequestedUserId = JsonSerializer.Serialize(result.ResponseData.RequestedUserId)
                };

                var response = followerService.PostAsync(followerRequesting).Result;
            }
            else
            {

            }
        }
    }
}
