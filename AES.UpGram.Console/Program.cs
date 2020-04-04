using UpSocial.UpGram.Core;
using UpSocial.UpGram.Domain.Entity;
using UpSocial.UpGram.Service;

namespace AES.UpGram.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var account = new BaseService<AccountEntity>().Get(1);
            var config = new BaseService<ConfigurationEntity>().Get(1);

            config.UserName = account.Name;
            config.Password = account.Password;

            var upGramConn = new Connector(config);
            var login = upGramConn.Login().Result;

            if (login)
            {



                //string fromNextId = "QVFBZ0RGZVZFRm9vclBvcTJOWHRaOEQtUUp2bDBfYkdtWkZXVmktSEVlNDdzSWJBYzRjNGYybnRRaTZqeGdHZm1mZUU3SWhKT3lmZTUzUXFEM0dFSG9GMQ=="; // alana_rox

                //do
                //{
                //    fromNextId = connector.User.Value.Follow("alana_rox", fromNextId).Result;

                //} while (!string.IsNullOrEmpty(fromNextId));

                //var unfollowed = connector.User.Value.UnFollow().Result;


            }




            upGramConn.Logout();
        }
    }
}
