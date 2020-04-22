using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;
using MeConecta.Gram.Service;
using System.Linq;
using System.Text.Json;

namespace MeConecta.Gram.Console
{
    public class FollowerRequesting
    {
        public void Execute(ICoreConnector connector, string userNameFrom, int accountId, 
            ConfigurationEntity config, string accountName)
        {
            var service = InstaUserService.Build(connector.User);
            var request = service.FollowAsync(userNameFrom).Result;

            //var basefollower = BaseService<FollowerRequestEntity>.Build();
            //var follower = basefollower.GetAsync().Result
            //    .LastOrDefault(cmp => cmp.AccountId == accountId);

            //string fromMaxId = follower?.FromMaxId ?? string.Empty;

            //var result = connector.User.FollowAsync(userNameFrom, fromMaxId).Result;

            //if (result.Succeeded)
            //{
            //    var followerRequesting = new FollowerRequestEntity()
            //    {
            //        AccountId = accountId,
            //        AccountFollowerId = 1, /// ARRUMA ISSO!!!!
            //        FromMaxId = result.ResponseData.NextMaxId,
            //        Message = result.Message,
            //        Succeeded = result.Succeeded,
            //        ResponseType = "OK",  /// ARRUMA ISSO!!!!
            //        FollowerRequestPk = JsonSerializer.Serialize(result.ResponseData.RequestedUserId)
            //    };

            //    var response = basefollower.PostAsync(followerRequesting).Result;
            //}
            //else
            //{

            //}
        }
    }
}
