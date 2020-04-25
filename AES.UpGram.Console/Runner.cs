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
            var account = await BaseReadOnlyService<AccountEntity>.Build().GetAsync(accountId);
            var userNameFrom = (await BaseReadOnlyService<AccountUserNameEntity>.Build()
                .GetAsync()).FirstOrDefault(cmp => cmp.AccountId == accountId && cmp.IsActive).UserName;

            config.Account.Name = account.Name;
            config.Account.Password = account.Password;

            var connector = ConnectorCore.Build(config);
            var login = await connector.LoginAsync();

            if (login.Succeeded)
            {
                switch (choose)
                {
                    case 1:
                        new FollowerRequesting().Execute(connector, userNameFrom, account.Id, config, config.Account.Name);
                        break;

                    case 2:
                        new UnFollowRequested().Execute(connector);
                        break;

                    case 3:
                        var recipients = new string[] { "leseneda" };
                        var body = "Olá, Tudo bem?\r\n\r\nMe chamo Carolina e recentemente eu escrevi um eBook com receitas de bolos veganos e sem glúten e gostaria de saber se você teria interesse em fazer uma troca de divulgação.\r\n Eu faço a propaganda dos seus produtos e você me ajuda com a propaganda do eBook 🙏 \r\n\r\nBom resto de semana e aguardo a sua resposta 😘";

                        foreach (var recipient in recipients)
                        {
                            var ret = await connector.Message.SendDirectMessage(recipient, body);

                            if (!ret.Succeeded)
                            {

                            }
                        }
                        break;

                    default:
                        break;
                }

                connector = null;

                return true;
            }
            return false;
        }
    }
}
