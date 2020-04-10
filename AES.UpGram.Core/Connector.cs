using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.SessionHandlers;
using InstagramApiSharp.Logger;
using System;
using System.Threading.Tasks;
using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;

namespace MeConect.Gram.Core
{
    public class Connector : IInstaConnector
    {
        #region Field

        static IInstaApi _apiConnector { get; set; }
        Lazy<IInstaUser> _user { get; }
        Lazy<IInstaMessage> _message { get; }

        #endregion

        #region Property

        public IInstaUser User { get { return _user.Value; } }
        public IInstaMessage Message { get { return _message.Value; } }

        #endregion

        public static IInstaConnector Builder(ConfigurationEntity configuration)
        {
            return new Connector(configuration);
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

        private Connector(ConfigurationEntity configuration)
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

            _user = new Lazy<IInstaUser>(() => Core.User.Builder(_apiConnector.UserProcessor, configuration));
            _message = new Lazy<IInstaMessage>(() => Core.Message.Builder(_apiConnector));
        }

        #region Session

        void SaveSession()
        {
            if (_apiConnector.IsUserAuthenticated)
            {
                _apiConnector.SessionHandler.Save();
            }
        }

        void LoadSession()
        {
            _apiConnector.SessionHandler.Load();
        }

        #endregion

        async Task<IResult<InstaChallengeRequireVerifyMethod>> GetChallenge()
        {
            var challenge = await _apiConnector.GetChallengeRequireVerifyMethodAsync();

            if (challenge.Succeeded)
            {
                if (challenge.Value.SubmitPhoneRequired)
                {
                    var result = await _apiConnector.SubmitPhoneNumberForChallengeRequireAsync("numero do telefone da conta...");

                    if (result.Succeeded)
                    {

                    }
                    else
                    {

                    }
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
