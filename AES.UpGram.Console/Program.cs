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
                //AccountName = "carolinaseneda", //"acontabate",
                //Password = "ti57ga05", //"28vegana",
                AccountName = "acontabate",
                Password = "28vegana",
                LogLevel = 1,
                RequestDelay = 2,
                PagingData = 100
            });

            #endregion

            var login = connector.Login().Result;

            if (login.Succeeded)
            {
                connector.LoadState();

                //string fromNextId = string.Empty;

                //do
                //{
                //    fromNextId = connector.User.Follow("carolinaseneda", fromNextId).Result;

                //} while (!string.IsNullOrEmpty(fromNextId));

                var unfollowed = connector.User.UnFollow().Result;

                connector.Logout();
            }
        }
    }
}
