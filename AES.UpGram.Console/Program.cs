using AES.UpGram.Core;
using AES.UpGram.Model;

namespace AES.UpGram.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Account Detail

            var connector = new Connector(new Configuration()
            {
                LogLevel = 1,
                RequestDelay = 2,
            });

            #endregion

            var login = connector.Login().Result;

            if (login)
            {
                



                //string fromNextId = "QVFBZ0RGZVZFRm9vclBvcTJOWHRaOEQtUUp2bDBfYkdtWkZXVmktSEVlNDdzSWJBYzRjNGYybnRRaTZqeGdHZm1mZUU3SWhKT3lmZTUzUXFEM0dFSG9GMQ=="; // alana_rox

                //do
                //{
                //    fromNextId = connector.User.Value.Follow("alana_rox", fromNextId).Result;

                //} while (!string.IsNullOrEmpty(fromNextId));

                //var unfollowed = connector.User.Value.UnFollow().Result;


            }




            connector.Logout();
        }
    }
}
