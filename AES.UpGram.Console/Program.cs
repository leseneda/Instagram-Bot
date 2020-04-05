using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
            var choose = false;
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
                    new UnFollowRequested().Execute(connector);
                }
            }

            connector.LogoutAsync();
        }
    }
}
