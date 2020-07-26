using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Processors;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;
using MeConecta.Gram.Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeConecta.Gram.Core
{
    public class UserCore : IUserCore
    {
        #region Field & Property

        static IUserProcessor _apiUserProcessor;
        static IDiscoverProcessor _discoverProcessor;
        static PaginationParameters _paginationParameters;
        
        public AccountEntity Account { get; }

        #endregion

        #region Constructor

        private UserCore(IInstaApi apiConnector, ConfigurationEntity configuration)
        {
            _apiUserProcessor = apiConnector.UserProcessor;
            _discoverProcessor = apiConnector.DiscoverProcessor;
            _paginationParameters = PaginationParameters.MaxPagesToLoad(configuration.MaxPagesToLoad);
            
            Account = configuration.Account;
        }

        public static IUserCore Build(IInstaApi apiConnector, ConfigurationEntity configuration)
        {
            return new UserCore(apiConnector, configuration);
        }

        #endregion

        #region User

        private async Task<IResult<InstaUserShortList>> GetFollowersAsync(string userName, string nextMaxId = null)
        {
            var param = (!string.IsNullOrEmpty(nextMaxId) ?
                _paginationParameters.StartFromMaxId(nextMaxId) :
                _paginationParameters);

            return await _apiUserProcessor.GetUserFollowersAsync(userName, param)
                .ConfigureAwait(false);
        }

        public async Task<ResponseEntity<ResponseFollowerEntity>> FollowAsync(IEnumerable<long> followersPk)
        {
            var responseBase = new ResponseEntity<ResponseFollowerEntity>();
            var responseFollow = new ResponseFollowerEntity();
            IResult<InstaFriendshipFullStatus> request;

            foreach (var followerPk in followersPk)
            {
                request = await _apiUserProcessor.FollowUserAsync(followerPk)
                    .ConfigureAwait(false);

                if (request.Succeeded)
                {
                    responseFollow.FollowerRequestPk.Add(followerPk);
                }
                else
                {
                    if (ResponseManagerService.NonTroubleResponse(request.Info.ResponseType))
                    {
                        continue;
                    }

                    responseFollow.NextMaxId = null;
                    responseFollow.FollowerRemainPk.AddRange(followersPk.Except(responseFollow.FollowerRequestPk));
                    responseFollow.FollowerRequestPk = responseFollow.FollowerRequestPk.Count() > 0 ? responseFollow.FollowerRequestPk : null;

                    responseBase.Succeeded = request.Succeeded;
                    responseBase.Message = request.Info.Message;
                    responseBase.ResponseType = request.Info.ResponseType;
                    responseBase.ResponseData = responseFollow;

                    return responseBase;
                }

            }

            responseFollow.FollowerRemainPk = responseFollow.FollowerRemainPk.Any() ? responseFollow.FollowerRemainPk : null;
            responseBase.ResponseData = responseFollow;

            return responseBase;
        }

        public async Task<ResponseEntity<ResponseFollowerEntity>> FollowAsync(string userName, string nextMaxId = null)
        {
            var result = await GetFollowersAsync(userName, nextMaxId);

            if (result.Succeeded)
            {
                var usersPk = result.Value
                    .Select(cmp => cmp.Pk);

                var response = await FollowAsync(usersPk);

                response.ResponseData.NextMaxId = result.Succeeded ? result.Value?.NextMaxId : nextMaxId;

                return response;
            }
            else
            {
                return new ResponseEntity<ResponseFollowerEntity>()
                {
                    Succeeded = result.Succeeded,
                    Message = result.Info.Message,
                    ResponseData = null,
                    ResponseType = result.Info.ResponseType,
                };
            }
        }

        public async Task<ResponseEntity<IList<long>>> UnfollowAsync(long[] requestedUsersPk)
        {
            var responseBase = new ResponseEntity<IList<long>>();

            IResult<InstaFriendshipFullStatus> user;

            var nonUnfollow = new List<long>();

            foreach (var userPk in requestedUsersPk)
            {
                user = await _apiUserProcessor.UnFollowUserAsync(userPk)
                    .ConfigureAwait(false);

                if (!user.Succeeded)
                {
                    nonUnfollow.Add(userPk);

                    responseBase.Succeeded = user.Succeeded;
                    responseBase.Message = user.Info.Message;
                    responseBase.ResponseType = user.Info.ResponseType;
                }
            }

            responseBase.ResponseData = nonUnfollow;

            return responseBase;
        }

        public async Task<ResponseEntity<IResult<InstaUserInfo>>> GetUserAsync(string userName)
        {
            var result = await _apiUserProcessor.GetUserInfoByUsernameAsync(userName)
                .ConfigureAwait(false);

            return new ResponseEntity<IResult<InstaUserInfo>>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
                ResponseType = result.Info.ResponseType,
                ResponseData = result
            };
        }

        public async Task<ResponseEntity<IResult<InstaDiscoverSearchResult>>> SearchUser(string search, int counterData = 50)
        {
            var result = await _discoverProcessor.SearchPeopleAsync(search, counterData)
                .ConfigureAwait(false);

            return new ResponseEntity<IResult<InstaDiscoverSearchResult>>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
                ResponseType = result.Info.ResponseType,
                ResponseData = result
            };
        }

        #endregion
    }
}
