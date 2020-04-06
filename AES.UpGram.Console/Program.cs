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
            var choose = 3;
            var accountId = 1;
            var userNameFrom = string.Empty;
            var config = new BaseService<ConfigurationEntity>().GetAsync().Result.FirstOrDefault();
            var account = new BaseService<AccountEntity>().GetAsync(accountId).Result;
            var accountFollower = new BaseService<AccountFollowerEntity>().GetAsync()
                .Result.FirstOrDefault(cmp => cmp.AccountId == accountId);

            config.UserName = account.Name;
            config.Password = account.Password;

            userNameFrom = accountFollower.UserName;

            var connector = new Connector(config);
            
            var login = connector.LoginAsync().Result;

            if (login)
            {
                switch (choose)
                {
                    case 1:
                        new FollowerRequesting().Execute(connector, userNameFrom, account.Id);
                        break;

                    case 2:
                        new UnFollowRequested().Execute(connector);
                        break;

                    case 3:
                        var ret1 = connector.Message.Value.DirectMessage("leseneda", $"olá alexandre!, mensagem de {config.UserName}").Result;
                        //var ret2 = connector.Message.Value.DirectMessage(new string[] { "leseneda", "carolinaseneda" } , $"olá !, mensagem de {config.UserName}").Result;
                        break;

                    default:
                        break;
                }
            }

            connector.LogoutAsync();
        }
    }
}
