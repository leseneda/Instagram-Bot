using System.Linq;
using UpSocial.UpGram.Console;
using UpSocial.UpGram.Core;
using UpSocial.UpGram.Domain.Entity;
using UpSocial.UpGram.Service;

namespace AES.UpGram.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var choose = true;
            var accountId = 1;
            var userNameFrom = "alana_rox";
            var account = new BaseService<AccountEntity>().GetAsync(accountId).Result;
            var config = new BaseService<ConfigurationEntity>().GetAsync().Result.FirstOrDefault();

            config.UserName = account.Name;
            config.Password = account.Password;

            var connector = new Connector(config);
            
            var login = connector.LoginAsync().Result;

            if (login)
            {
                if (choose)
                {
                    new FollowerRequesting().Execute(connector, userNameFrom, account.Id);
                }
                else
                {
                    //var fromNextId = "QVFER0lDTFJueExzSWMtM0t2YTNfa1EwSk1rWUJxZERFek90Y3JOY2ZuU3czSDVZdGhWMFJqclZRSk50eTBlQlhwRVhGSThlQTRBMXZJUXhPcVpkdUx5Vg==";
                    //var unfollowed = connector.User.Value.UnFollowAsyncNew(userNameFrom, fromNextId).Result;
                }
            }

            connector.LogoutAsync();
        }
    }
}
