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
        public Lazy<User> User { get; set; }

        static IInstaApi _apiConnector;
        static readonly LogLevel _logLevel = LogLevel.Exceptions;
        static readonly int _requestDelayFromSec = 2;

        public Connector(ConfigurationEntity configuration)
        {
            _apiConnector = InstaApiBuilder.CreateBuilder()
                .UseLogger(new DebugLogger(_logLevel))
                .SetRequestDelay(RequestDelay.FromSeconds(_requestDelayFromSec, _requestDelayFromSec))
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
        }

        #region Log

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
                            result = false;
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            else
            {

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

        #endregion

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
    }
}
