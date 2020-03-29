using AES.UpGram.Core;
using AES.UpGram.Core.Utils;
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
                AccountName = "acontabate",
                Password = "28vegana",
                LogLevel = 1,
                RequestDelay = 2,
                MaxPageToLoad = 100
            });

            #endregion

            var ret2 = new AccessKey().IsValidKeyDate;


            var login = connector.Login().Result;

            if (login.Succeeded)
            {
                var ret = connector.User.UnFollowFrom().Result;
            }

            connector.Logout();
        }
    }
}
