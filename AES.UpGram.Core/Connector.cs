using AES.UpGram.Model;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.SessionHandlers;
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
                .SetSessionHandler(new FileSessionHandler() 
                { 
                    FilePath = $"{configuration.AccountName}.bin"
                })
                .SetUser(new UserSessionData()
                {
                    UserName = configuration.AccountName,
                    Password = configuration.Password,
                })
                .Build();

            User = new Lazy<User>(() => new User(_apiConnector.UserProcessor, configuration));
        }

        public async Task<bool> Login()
        {
            var result = true;

            LoadSession();

            if (!_apiConnector.IsUserAuthenticated)
            {
                await _apiConnector.SendRequestsBeforeLoginAsync();
                await Task.Delay(5000);

                var login = await _apiConnector.LoginAsync();

                if (login.Succeeded)
                {
                    await _apiConnector.SendRequestsAfterLoginAsync();
                    
                    SaveSession();
                }
                else
                {
                    if (login.Value == InstaLoginResult.ChallengeRequired)
                    {
                        var challenge = await GetChallenge();

                        result = challenge.Succeeded;

                        if (challenge.Succeeded)
                        { 
                        
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            return result;
        }

        public async void Logout()
        {
            if (_apiConnector.IsUserAuthenticated)
            {
                await _apiConnector.LogoutAsync();
            }
        }

        void SaveSession()
        {
            if (_apiConnector == null || !_apiConnector.IsUserAuthenticated)
            {
                return;
            }
            
            _apiConnector.SessionHandler.Save();
        }

        void LoadSession()
        {
            _apiConnector?.SessionHandler?.Load();
        }

        private async Task<IResult<InstaChallengeRequireVerifyMethod>> GetChallenge()
        {
            var challenge = await _apiConnector.GetChallengeRequireVerifyMethodAsync();

            if (challenge.Succeeded)
            {
                if (challenge.Value.SubmitPhoneRequired)
                {

                }
                else
                {
                    if (challenge.Value.StepData != null)
                    {
                        if (!string.IsNullOrEmpty(challenge.Value.StepData.PhoneNumber))
                        {

                        }

                        if (!string.IsNullOrEmpty(challenge.Value.StepData.Email))
                        {

                        }
                    }
                }
            }
            return challenge;
        }
    }
}
