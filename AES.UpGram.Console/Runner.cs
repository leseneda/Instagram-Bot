using MeConecta.Gram.Core;
using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Service;
using System.Linq;
using System.Threading.Tasks;

namespace MeConecta.Gram.Console
{
    public class Runner
    {
        public async Task<bool> Execute(int accountId, int choose, ConfigurationEntity config)
        {
            var account = await BaseServiceReadOnly<AccountEntity>.Build().GetAsync(accountId);
            var userNameFrom = (await BaseServiceReadOnly<AccountFollowerEntity>.Build()
                .GetAsync()).FirstOrDefault(cmp => cmp.AccountId == accountId && cmp.IsActive).UserName;

            config.UserName = account.Name;
            config.Password = account.Password;

            var connector = Connector.Build(config);
            var login = await connector.LoginAsync();

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

                    default:
                        break;
                }
                
                connector.LogoutAsync();

                return true;
            }
            return false;
        }
    }
}
