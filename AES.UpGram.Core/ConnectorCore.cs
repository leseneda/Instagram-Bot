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
    public class ConnectorCore : ICoreConnector
    {
        #region Field

        readonly IInstaApi _apiConnector;
        readonly ConfigurationEntity _configuration;

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

        private ConnectorCore(ConfigurationEntity configuration)
        {
            _apiConnector = InstaApiBuilder.CreateBuilder()
                .UseLogger(new DebugLogger((LogLevel)configuration.LogLevel))
                .SetRequestDelay(RequestDelay.FromSeconds(configuration.RequestDelay, configuration.RequestDelay))
                .SetSessionHandler(new FileSessionHandler()
                {
                    FilePath = $"{configuration.Account.Name.ToLower()}.bin"
                })
                .Build();

            _configuration = configuration;
            
            _user = new Lazy<ICoreUser>(() => Core.UserCore.Build(_apiConnector, configuration));
            _message = new Lazy<ICoreMessage>(() => Core.MessageCore.Build(_apiConnector, configuration));
            _hashTag = new Lazy<ICoreHashTag>(() => Core.HashTagCore.Build(_apiConnector.HashtagProcessor, configuration));
            _location = new Lazy<ICoreLocation>(() => Core.LocationCore.Build(_apiConnector.LocationProcessor, configuration));
        }

        public static ICoreConnector Build(ConfigurationEntity configuration)
        {
            return new ConnectorCore(configuration);
        }

        #endregion

        #region Logging

        public async Task<ResponseEntity<IResult<InstaLoginResult>>> LoginAsync()
        {
            var response = new ResponseEntity<IResult<InstaLoginResult>>();

            LoadSession();

            if (_apiConnector.IsUserAuthenticated)
            {
                if (!await IsToRenew())
                {
                    return response;
                }
            }

            _apiConnector.SetUser(new UserSessionData()
            {
                UserName = _configuration.Account.Name,
                Password = _configuration.Account.Password,
            });

            await _apiConnector.SendRequestsBeforeLoginAsync().ConfigureAwait(false);
            await Task.Delay(_configuration.RequestDelay * 1000).ConfigureAwait(false);

            var result = await _apiConnector.LoginAsync().ConfigureAwait(false);

            if (result.Succeeded)
            {
                await _apiConnector.SendRequestsAfterLoginAsync().ConfigureAwait(false);

                SaveSession();

                response.ResponseData = result;
            }
            else
            {
                if (result.Value == InstaLoginResult.ChallengeRequired)
                {
                    var challenge = await GetChallenge().ConfigureAwait(false);

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

            return response;
        }

        public async Task<ResponseEntity<IResult<bool>>> LogoutAsync()
        {
            IResult<bool> result = !_apiConnector.IsUserAuthenticated ? 
                Result.Fail<bool>("Non authenticated user") : 
                await _apiConnector.LogoutAsync()
                    .ConfigureAwait(false);
            
            return new ResponseEntity<IResult<bool>>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
                ResponseData = result
            };
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
            var fullPathFile = @$"{Environment.CurrentDirectory}\{_configuration.Account.Name.ToLower()}.bin";

            if (File.Exists(fullPathFile))
            {
                File.Delete(fullPathFile);
            }
        }

        #endregion

        async Task<bool> IsToRenew()
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
            var challenge = await _apiConnector.GetChallengeRequireVerifyMethodAsync()
                .ConfigureAwait(false);

            if (challenge.Succeeded)
            {
                if (challenge.Value.SubmitPhoneRequired)
                {
                    var result = await _apiConnector.SubmitPhoneNumberForChallengeRequireAsync(_configuration.Account.PhoneNumber)
                        .ConfigureAwait(false);

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
                        if (!string.IsNullOrEmpty(challenge.Value.StepData.Email))
                        {
                            var result = await _apiConnector.RequestVerifyCodeToEmailForChallengeRequireAsync()
                                .ConfigureAwait(false);

                            if (result.Succeeded)
                            {

                            }
                            else 
                            { 
                            
                            }
                        }
                        else if (!string.IsNullOrEmpty(challenge.Value.StepData.PhoneNumber))
                        {
                            var result = await _apiConnector.RequestVerifyCodeToSMSForChallengeRequireAsync()
                                .ConfigureAwait(false);

                            if (result.Succeeded)
                            {

                            }
                            else
                            {

                            }
                        }

                    }
                }
            }
            
            return challenge;
        }

        #endregion
    }
}
