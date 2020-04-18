using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.SessionHandlers;
using InstagramApiSharp.Logger;
using System;
using System.Threading.Tasks;
using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;
using System.IO;

namespace MeConecta.Gram.Core
{
    public class CoreConnector : ICoreConnector
    {
        #region Field

        readonly IInstaApi _apiConnector;
        string _accountName;
        string _accountPassword;

        #endregion

        #region Property

        Lazy<ICoreUser> _user { get; }
        Lazy<ICoreMessage> _message { get; }
        Lazy<ICoreHashTag> _hashTag { get; }
        Lazy<ICoreLocation> _location { get; }

        public ICoreUser User { get { return _user.Value; } }
        public ICoreMessage Message { get { return _message.Value; } }
        public ICoreHashTag HashTag { get { return _hashTag.Value; } }
        public ICoreLocation Location { get { return _location.Value; } }

        #endregion

        #region Constructor

        private CoreConnector(ConfigurationEntity configuration)
        {
            _apiConnector = InstaApiBuilder.CreateBuilder()
                .UseLogger(new DebugLogger((LogLevel)configuration.LogLevel))
                .SetRequestDelay(RequestDelay.FromSeconds(configuration.RequestDelay, configuration.RequestDelay))
                .SetSessionHandler(new FileSessionHandler()
                {
                    FilePath = $"{configuration.Account.Name.ToLower()}.bin"
                })
                .Build();

            _accountName = configuration.Account.Name;
            _accountPassword = configuration.Account.Password;

            _user = new Lazy<ICoreUser>(() => Core.CoreUser.Build(_apiConnector, configuration));
            _message = new Lazy<ICoreMessage>(() => Core.CoreMessage.Build(_apiConnector, configuration));
            _hashTag = new Lazy<ICoreHashTag>(() => Core.CoreHashTag.Build(_apiConnector.HashtagProcessor, configuration));
            _location = new Lazy<ICoreLocation>(() => Core.CoreLocation.Build(_apiConnector.LocationProcessor, configuration));
        }

        public static ICoreConnector Build(ConfigurationEntity configuration)
        {
            return new CoreConnector(configuration);
        }

        #endregion

        #region Logging

        public async Task<bool> LoginAsync()
        {
            var result = false;

            LoadSession();

            if (_apiConnector.IsUserAuthenticated)
            {
                if (!await Renew())
                {
                    return true;
                }
            }

            _apiConnector.SetUser(new UserSessionData()
            {
                UserName = _accountName,
                Password = _accountPassword,
            });

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

            return result;
        }

        public async Task<IResult<bool>> LogoutAsync()
        {
            if (!_apiConnector.IsUserAuthenticated)
            {
                return Result.Fail<bool>("no user authenticated");
            }

            return await _apiConnector.LogoutAsync();
        }

        #endregion

        #region Private

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

        void EraseSession()
        {
            var fullPathFile = @$"{Environment.CurrentDirectory}\{_accountName.ToLower()}.bin";

            if (File.Exists(fullPathFile))
            {
                File.Delete(fullPathFile);
            }
        }

        #endregion

        async Task<bool> Renew()
        {
            var request = await _apiConnector.UserProcessor.GetCurrentUserAsync();
            var renew = (!request.Succeeded && (request.Info.ResponseType == ResponseType.LoginRequired));

            if (renew)
            {
                EraseSession();
            }

            return renew;
        }

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
