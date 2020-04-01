using AES.UpGram.Model;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Logger;
using System;
using System.Threading.Tasks;

namespace AES.UpGram.Core
{
    public class Connector
    {
        static IInstaApi _apiConnector;

        public Lazy<User> User { get; set; }

        public Connector(Configuration configuration)
        {
            _apiConnector = InstaApiBuilder.CreateBuilder()
                .UseLogger(new DebugLogger((LogLevel)configuration.LogLevel))
                .SetRequestDelay(RequestDelay.FromSeconds(configuration.RequestDelay, configuration.RequestDelay))
                .SetUser(new UserSessionData()
                {
                    UserName = configuration.AccountName,
                    Password = configuration.Password,
                })
                .Build();

            User = new Lazy<User>(() => new User(_apiConnector.UserProcessor, configuration));
        }

        public async Task<IResult<InstaLoginResult>> Login()
        {
            return await _apiConnector.LoginAsync();
        }

        public async void Logout()
        {
            if (_apiConnector.IsUserAuthenticated)
            {
                await _apiConnector.LogoutAsync();
            }
        }
    }
}
