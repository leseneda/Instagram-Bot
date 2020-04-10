using System.Linq;
using System.Text.Json;
using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;
using MeConecta.Gram.Service;

namespace MeConecta.Gram.Console
{
    public class FollowerRequesting
    {
        public void Execute(IInstaConnector connector, string userNameFrom, int accountId)
        {
            var userResponse = connector.User.GetUserAsync(userNameFrom).Result;

            if (!userResponse.Succeeded)
            {
                // Log
            }

            var basefollower = BaseService<FollowerRequestingEntity>.Builder();
            var follower = basefollower.GetAsync().Result
                .LastOrDefault(cmp => cmp.AccountId == accountId);

            string fromMaxId = follower?.FromMaxId ?? string.Empty;

            var result = connector.User.RequestFollowersAsync(userNameFrom, fromMaxId).Result;

            if (result.Succeeded)
            {
                var followerRequesting = new FollowerRequestingEntity()
                {
                    AccountId = accountId,
                    AccountFollowerId = 1, /// ARRUMA ISSO!!!!
                    FromMaxId = result.ResponseData.NextMaxId,
                    Message = result.Message,
                    Succeeded = result.Succeeded,
                    ResponseType = "OK",  /// ARRUMA ISSO!!!!
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
