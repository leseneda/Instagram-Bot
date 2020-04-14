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

            config.Account.Name = account.Name;
            config.Account.Password = account.Password;

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

                    case 3:
                        var recipients = new string[] { "leseneda" };

                        var body = "Olá, Tudo bem?\r\n\r\nMe chamo Carolina e recentemente eu escrevi um eBook com receitas de bolos veganos e sem glúten e publiquei ele na plataforma Hotmart.\r\n Gostaria de saber se você teria interesse em se afiliar e me ajudar com a divulgação 🙏 \r\nSe concordar irá receber 20 % do valor de cada venda realizada 😊 \r\n\r\nBom resto de semana e aguardo a sua resposta 😘";

                        foreach (var recipient in recipients)
                        {
                            var ret = connector.Message.SendDirectMessage(recipient, body).Result;

                            if (!ret.Succeeded)
                            {

                            }
                        }
                        break;

                    default:
                        break;
                }

                var result = await connector.LogoutAsync();
                connector = null;

                return true;
            }
            return false;
        }
    }
}
