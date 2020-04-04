using InstagramApiSharp;
using InstagramApiSharp.API.Processors;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using System.Linq;
using System.Threading.Tasks;
using UpSocial.UpGram.Domain.Entity;

namespace UpSocial.UpGram.Core
{
    public class User
    {
        static IUserProcessor _apiUserProcessor;
        static PaginationParameters _paginationParameters;
        static ConfigurationEntity _configuration;
        static readonly int _pagingData = 100;

        public User(IUserProcessor apiUserProcessor, ConfigurationEntity configuration)
        {
            _configuration = configuration;
            _apiUserProcessor = apiUserProcessor;
            _paginationParameters = PaginationParameters.MaxPagesToLoad(1);
        }

        public async Task<ResponseEntity<IResult<InstaFriendshipFullStatus>>> FollowAsync(string userName, string fromNextId = null)
        {
            var param = (!string.IsNullOrEmpty(fromNextId) ?
                _paginationParameters.StartFromMaxId(fromNextId) :
                _paginationParameters);

            var result = await _apiUserProcessor.GetUserFollowersAsync(userName, param);
            
            var response = new ResponseEntity<IResult<InstaFriendshipFullStatus>>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
                ResponseData = fromNextId
            };

            if (result.Succeeded)
            {
                IResult<InstaFriendshipFullStatus> followResult = null;
                var users = result.Value;

                foreach (var user in users)
                {
                    followResult = await _apiUserProcessor.FollowUserAsync(user.Pk);
                    
                    if (!followResult.Succeeded)
                    {
                        response.Data = followResult;

                        return response;
                    }
                }

                response.Data = followResult;
                response.ResponseData = users?.NextMaxId ?? string.Empty;
            }

            return response;
        }

        public async Task<InstaUserShortList> UnFollowAsync()
        {
            var result = await _apiUserProcessor.GetUserFollowingAsync(_configuration.UserName, _paginationParameters);

            if (result.Succeeded)
            {
                result.Value.RemoveRange(_pagingData - 1, result.Value.Count() - _pagingData);

                var users = result.Value;

                foreach (var user in users)
                {
                    if ((await _apiUserProcessor.UnFollowUserAsync(user.Pk)).Succeeded)
                    {

                    }
                    else
                    {

                    }
                }

                return users;
            }

            return null;
        }

        public async Task<IResult<InstaUserInfo>> UserAsync(string userName)
        {
            return await _apiUserProcessor.GetUserInfoByUsernameAsync(userName);
        }
    }
}
