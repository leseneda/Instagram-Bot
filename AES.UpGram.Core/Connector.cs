using AES.UpGram.Model;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Logger;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AES.UpGram.Core
{
    public class Connector
    {
        static IInstaApi _apiConnector;
        private static Configuration _configuration;

        #region Property

        public User User
        {
            get { return new User(_apiConnector, _configuration); }
        }

        #endregion

        public Connector(Configuration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IResult<InstaLoginResult>> Login()
        {
            var userLogger = new DebugLogger((LogLevel)_configuration.LogLevel);
            var requestDelay = RequestDelay.FromSeconds(_configuration.RequestDelay, _configuration.RequestDelay);
            var user = new UserSessionData()
            {
                UserName = _configuration.AccountName,
                Password = _configuration.Password,
            };

            _apiConnector = InstaApiBuilder.CreateBuilder()
                .SetUser(user)
                .UseLogger(userLogger)
                .SetRequestDelay(requestDelay)
                .Build();

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
