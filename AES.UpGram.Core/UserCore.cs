using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Processors;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;
using MeConecta.Gram.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeConecta.Gram.Core
{
    public class UserCore : ICoreUser
    {
        #region Field

        static IUserProcessor _apiUserProcessor;
        static IDiscoverProcessor _discoverProcessor;
        static PaginationParameters _paginationParameters;

        #endregion

        public AccountEntity Account { get; }

        #region Constructor

        private UserCore(IInstaApi apiConnector, ConfigurationEntity configuration)
        {
            _apiUserProcessor = apiConnector.UserProcessor;
            _discoverProcessor = apiConnector.DiscoverProcessor;
            _paginationParameters = PaginationParameters.MaxPagesToLoad(configuration.MaxPagesToLoad);
            
            Account = configuration.Account;
        }

        public static ICoreUser Build(IInstaApi apiConnector, ConfigurationEntity configuration)
        {
            return new UserCore(apiConnector, configuration);
        }

        #endregion

        #region User

        public async Task<ResponseEntity<ResponseFollowerEntity>> FollowAsync(string userName, string nextMaxId = null)
        {
            var param = (!string.IsNullOrEmpty(nextMaxId) ?
                _paginationParameters.StartFromMaxId(nextMaxId) :
                _paginationParameters);

            var result = await _apiUserProcessor.GetUserFollowersAsync(userName, param);
            
            var responseBase = new ResponseEntity<ResponseFollowerEntity>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
            };

            var responseFollow = new ResponseFollowerEntity();

            if (result.Succeeded)
            {
                var users = result.Value;
                
                IResult<InstaFriendshipFullStatus> request;

                foreach (var user in users)
                {
                    request = await _apiUserProcessor.FollowUserAsync(user.Pk);

                    if (request.Succeeded)
                    {
                        responseFollow.RequestedUserId.Add(user.Pk);
                    }
                    else
                    {
                        if (ResponseManagerService.NonTroubleResponse(request.Info.ResponseType))
                        {
                            continue;
                        }

                        responseFollow.NextMaxId = nextMaxId;

                        responseBase.Succeeded = request.Succeeded;
                        responseBase.Message = request.Info.Message;
                        responseBase.ResponseData = responseFollow;

                        return responseBase;
                    }
                }
                responseFollow.NextMaxId = users?.NextMaxId ?? string.Empty;
                responseBase.ResponseData = responseFollow;
            }
            else
            {

            }

            return responseBase;
        }

        public async Task<ResponseEntity<IList<long>>> UnfollowAsync(long[] requestedUsersPk)
        {
            var responseBase = new ResponseEntity<IList<long>>();

            IResult<InstaFriendshipFullStatus> user;

            var nonUnfollow = new List<long>();

            foreach (var userPk in requestedUsersPk)
            {
                user = await _apiUserProcessor.UnFollowUserAsync(userPk);

                if (!user.Succeeded)
                {
                    nonUnfollow.Add(userPk);
                }
            }

            responseBase.ResponseData = nonUnfollow;

            return responseBase;
        }

        public async Task<ResponseEntity<IResult<InstaUserInfo>>> GetUserAsync(string userName)
        {
            var result = await _apiUserProcessor.GetUserInfoByUsernameAsync(userName);

            return new ResponseEntity<IResult<InstaUserInfo>>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
                ResponseData = result
            };
        }

        public async Task<ResponseEntity<IResult<InstaDiscoverSearchResult>>> SearchUser(string search, int counterData = 50)
        {
            var result = await _discoverProcessor.SearchPeopleAsync(search, counterData);

            return new ResponseEntity<IResult<InstaDiscoverSearchResult>>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
                ResponseData = result
            };
        }

        #endregion
    }
}
