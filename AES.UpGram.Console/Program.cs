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
            var accountId = 1;
            var userNameFrom = "alana_rox";

            var account = new BaseService<AccountEntity>().GetAsync(accountId).Result;
            var config = new BaseService<ConfigurationEntity>().GetAsync().Result.FirstOrDefault();

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

                var FollowerRequesting = new BaseService<FollowerRequestingEntity>();
                var follower = FollowerRequesting.GetAsync().Result.LastOrDefault();

                string fromMaxId = follower?.FromMaxId ?? string.Empty;

                var result = connector.User.Value.FollowAsync(userNameFrom, fromMaxId).Result;

                if (result.Succeeded)
                {
                    var response = FollowerRequesting.PostAsync(new FollowerRequestingEntity()
                    {
                        AccountId = account.Id,
                        AccountFollowerId = 1,
                        FromMaxId = (string)result.ResponseData.NextMaxId,
                        Message = result.ResponseData.Info.Message,
                        Succeeded = result.ResponseData.Succeeded,
                        ResponseType = result.ResponseData.Info.ResponseType.ToString()
                    }).Result;

                    var responseData = (long[])result.ResponseData.RequestingUser;
                    
                    var requestingUser = responseData
                        .Select(pk => new FollowerRequestingUserEntity()
                        {
                            FollowerRequestingId = follower.Id,
                            UserPK = pk
                        });

                }
                else
                {

                }


                //var fromNextId = "QVFER0lDTFJueExzSWMtM0t2YTNfa1EwSk1rWUJxZERFek90Y3JOY2ZuU3czSDVZdGhWMFJqclZRSk50eTBlQlhwRVhGSThlQTRBMXZJUXhPcVpkdUx5Vg==";
                //var unfollowed = connector.User.Value.UnFollowAsync(userNameFrom, fromNextId).Result;
            }

            connector.Logout();
        }
    }
}
