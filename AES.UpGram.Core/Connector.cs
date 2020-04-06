using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.SessionHandlers;
using InstagramApiSharp.Logger;
using System;
using System.Threading.Tasks;
using UpSocial.UpGram.Domain.Entity;

namespace UpSocial.UpGram.Core
{
    public class Connector
    {
        static IInstaApi _apiConnector;
        public Lazy<User> User { get; set; }
        public Lazy<Message> Message { get; set; }

        public Connector(ConfigurationEntity configuration)
        {
            _apiConnector = InstaApiBuilder.CreateBuilder()
                .UseLogger(new DebugLogger((LogLevel)configuration.LogLevel))
                .SetRequestDelay(RequestDelay.FromSeconds(configuration.RequestDelay, configuration.RequestDelay))
                .SetSessionHandler(new FileSessionHandler()
                {
                    FilePath = $"{configuration.UserName.ToLower()}.bin"
                })
                .SetUser(new UserSessionData()
                {
                    UserName = configuration.UserName,
                    Password = configuration.Password,
                })
                .Build();

            User = new Lazy<User>(() => new User(_apiConnector.UserProcessor, configuration));
            Message = new Lazy<Message>(() => new Message(_apiConnector));
        }

        #region Logging

        public async Task<bool> LoginAsync()
        {
            var result = false;

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

                    result = true;
                }
                else
                {
                    if (login.Value == InstaLoginResult.ChallengeRequired)
                    {
                        var challenge = await GetChallenge();

                        if (challenge.Succeeded)
                        {

                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {
                        
                    }
                }
            }
            else
            {
                result = true;
            }

            return result;
        }

        public async void LogoutAsync()
        {
            if (_apiConnector.IsUserAuthenticated)
            {
                await _apiConnector.LogoutAsync();
            }
        }

        #endregion

        #region Private

        #region Session

        void SaveSession()
        {
            //if (_apiConnector == null || !_apiConnector.IsUserAuthenticated)
            if (!_apiConnector?.IsUserAuthenticated ?? false)
            {
                return;
            }

            _apiConnector.SessionHandler.Save();
        }

        void LoadSession()
        {
            _apiConnector?.SessionHandler?.Load();
        }

        #endregion

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

        #endregion
    }
}
