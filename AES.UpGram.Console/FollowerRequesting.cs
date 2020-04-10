using System.Linq;
using System.Text.Json;
using UpSocial.UpGram.Domain.Entity;
using UpSocial.UpGram.Domain.Interface;
using UpSocial.UpGram.Service;

namespace UpSocial.UpGram.Console
{
    public class FollowerRequesting
    {
        public void Execute(IInstaConnector connector, string userNameFrom, int accountId)
        {
            var userResponse = connector.User.UserAsync(userNameFrom).Result;

            if (!userResponse.Succeeded)
            {
                // Log
            }

            var basefollower = BaseService<FollowerRequestingEntity>.Builder();
            var follower = basefollower.GetAsync().Result
                .LastOrDefault(cmp => cmp.AccountId == accountId);

            string fromMaxId = follower?.FromMaxId ?? string.Empty;

            var result = connector.User.FollowAsync(userNameFrom, fromMaxId).Result;

            if (result.Succeeded)
            {
                var followerRequesting = new FollowerRequestingEntity()
                {
                    AccountId = accountId,
                    AccountFollowerId = 1, /// ARRUMA ISSO!!!!
                    FromMaxId = result.ResponseData.NextMaxId,
                    Message = result.Message,
                    Succeeded = result.Succeeded,
                    RequestedUserId = JsonSerializer.Serialize(result.ResponseData.RequestedUserId)
                };

                var response = basefollower.PostAsync(followerRequesting).Result;
            }
            else
            {

            }
        }
    }
}
