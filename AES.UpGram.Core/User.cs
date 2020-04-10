using InstagramApiSharp;
using InstagramApiSharp.API.Processors;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;

namespace MeConecta.Gram.Core
{
    public class User : IInstaUser
    {
        static IUserProcessor _apiUserProcessor;
        static PaginationParameters _paginationParameters;

        private User(IUserProcessor apiUserProcessor, ConfigurationEntity configuration)
        {
            _apiUserProcessor = apiUserProcessor;
            _paginationParameters = PaginationParameters.MaxPagesToLoad(configuration.MaxPagesToLoad);
        }

        public static IInstaUser Builder(IUserProcessor apiUserProcessor, ConfigurationEntity configuration)
        {
            return new User(apiUserProcessor, configuration);
        }

        public async Task<ResponseEntity<ResponseFollowerEntity>> RequestFollowersAsync(string userName, string nextMaxId = null)
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

            return responseBase;
        }

        public async Task<ResponseEntity<IList<long>>> UnfollowAsync(long[] followerRequesting)
        {
            var responseBase = new ResponseEntity<IList<long>>();

            IResult<InstaFriendshipFullStatus> user;
            var unFollowed = new List<long>();

            foreach (var userPk in followerRequesting)
            {
                user = await _apiUserProcessor.UnFollowUserAsync(userPk);

                if (user.Succeeded)
                {
                    unFollowed.Add(userPk);
                }
                else 
                {
                    responseBase.Succeeded = user.Succeeded;
                    responseBase.ResponseData = unFollowed;

                    break;
                }
            }

            return responseBase;
        }

        public async Task<IResult<InstaUserInfo>> GetUserAsync(string userName)
        {
            return await _apiUserProcessor.GetUserInfoByUsernameAsync(userName);
        }
    }
}
