using System.Linq;
using System.Text.Json;
using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;
using MeConecta.Gram.Service;

namespace MeConecta.Gram.Console
{
    public class FollowerRequesting
    {
        public void Execute(ICoreConnector connector, string userNameFrom, int accountId)
        {
            var unitOfWork = BasePatternUnitOfWork.Build();

            var configuration = new ConfigurationEntity()
            {
                LogLevel = 1,
                MaxPagesToLoad = 1,
                RequestDelay = 1,
            };

            var configuration2 = new ConfigurationEntity()
            {
                LogLevel = 2,
                MaxPagesToLoad = 2,
                RequestDelay = 2,
            };

            unitOfWork.Post(configuration);
            unitOfWork.Post(configuration2);
            unitOfWork.Commit();

            var basefollower = BaseService<AccountFollowerRequestEntity>.Build();
            var follower = basefollower.GetAsync().Result
                .LastOrDefault(cmp => cmp.AccountId == accountId);

            string fromMaxId = follower?.FromMaxId ?? string.Empty;

            var result = connector.User.FollowAsync(userNameFrom, fromMaxId).Result;

            if (result.Succeeded)
            {
                var followerRequesting = new AccountFollowerRequestEntity()
                {
                    AccountId = accountId,
                    AccountFollowerId = 1, /// ARRUMA ISSO!!!!
                    FromMaxId = result.ResponseData.NextMaxId,
                    Message = result.Message,
                    Succeeded = result.Succeeded,
                    ResponseType = "OK",  /// ARRUMA ISSO!!!!
                    FollowerRequestPk = JsonSerializer.Serialize(result.ResponseData.RequestedUserId)
                };

                var response = basefollower.PostAsync(followerRequesting).Result;
            }
            else
            {

            }
        }
    }
}
