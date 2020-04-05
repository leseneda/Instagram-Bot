using System.Linq;
using UpSocial.UpGram.Core;
using UpSocial.UpGram.Domain.Entity;
using UpSocial.UpGram.Service;

namespace AES.UpGram.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var userNameFrom = "alana_rox12";
            var account = new BaseService<AccountEntity>().GetAsync(1).Result;
            var config = new BaseService<ConfigurationEntity>().GetAsync(1).Result;

            config.UserName = account.Name;
            config.Password = account.Password;

            var connector = new Connector(config);
            var login = connector.Login().Result;

            if (login)
            {
                var userResponse = connector.User.Value.UserAsync(userNameFrom).Result;
                
                if (!userResponse.Succeeded)
                {
                    // Log
                }

                var followersResponse = new BaseService<FollowersRequestingEntity>();
                var follower = followersResponse.GetAsync().Result
                    .LastOrDefault(cmp => cmp.UserName == userNameFrom);

                string fromMaxId = follower?.FromMaxId ?? string.Empty;

                var result = connector.User.Value.FollowAsync(userNameFrom, fromMaxId).Result;

                if (result.Succeeded || (result.Data?.Succeeded ?? true))
                {
                    var response = followersResponse.PostAsync(new FollowersRequestingEntity()
                    {
                        AccountId = account.Id,
                        UserName = userNameFrom,
                        FromMaxId = result.ResponseData.ToString(),
                        Message = result.Data.Info.Message,
                        Succeeded = result.Data.Succeeded,
                    }).Result;
                }
                else
                {

                }
                
                //var unfollowed = connector.User.Value.UnFollow().Result;
            }

            connector.Logout();
        }
    }
}
