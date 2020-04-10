using System.Linq;
using MeConecta.Gram.Core;
using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Service;

namespace MeConecta.Gram.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var accountId = 1;
            var choose = 1;

            var userNameFrom = string.Empty;

            var baseConfig = BaseServiceReadOnly<ConfigurationEntity>.Builder();
            var config = baseConfig.GetAsync().Result.FirstOrDefault();
            
            var baseAccount = BaseServiceReadOnly<AccountEntity>.Builder();
            var account = baseAccount.GetAsync(accountId).Result;

            config.UserName = account.Name;
            config.Password = account.Password;

            var baseAccountFollower = BaseServiceReadOnly<AccountFollowerEntity>.Builder();
            var accountFollower = baseAccountFollower.GetAsync().Result
                .FirstOrDefault(cmp => cmp.AccountId == accountId);

            userNameFrom = accountFollower.UserName;

            var connector = Connector.Builder(config);
            var login = connector.LoginAsync().Result;

            var ret = connector.HashTag.GetRecentHashtagListAsync("spaldginasios").Result;

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
                        //var ret1 = connector.Message.Value.DirectMessage("leseneda", $"olá alexandre!, mensagem de {config.UserName} acesse: https://github.com/ramtinak/InstagramApiSharp").Result;
                        //var ret2 = connector.Message.Value.DirectMessage(new string[] { "leseneda", "carolinaseneda" } , $"olá !, mensagem de {config.UserName}").Result;

                        //var link = "https://github.com/ramtinak/InstagramApiSharp";
                        //var body = $"Hi, check this awesome instagram library for .net:\r\n{link}\r\nDon't forget to report issues!";
                        //var recipients = new string[] { "veganasnoesporte", "mulheresveganas", "malgadipaula.veg", "veganismoevida", 
                        //    "cami.vegana", "plantt.e", "veg.in.trio", "vegana.bacana", "rotaveg", "paruvegan","mariaalice_nutri", 
                        //    "marinasch.nutricionista", "carolvidavegan", "plantbased_healthylife" };

                        //var ret3 = connector.Message.Value.DirectMessageLink("leseneda", link, body).Result;

                        //body = "Olá, Tudo bem?\r\n\r\nMe chamo Carolina e recentemente eu escrevi um eBook com receitas de bolos veganos e sem glúten e publiquei ele na plataforma Hotmart.\r\n Gostaria de saber se você teria interesse em se afiliar e me ajudar com a divulgação 🙏 \r\nSe concordar irá receber 20 % do valor de cada venda realizada 😊 \r\n\r\nBom resto de semana e aguardo a sua resposta 😘";

                        //foreach (var recipient in recipients)
                        //{
                        //    var ret3 = connector.Message.Value.DirectMessage(recipient, body).Result;
                        //}

                        break;

                    default:
                        break;
                }
            }

            connector.LogoutAsync();
        }
    }
}
