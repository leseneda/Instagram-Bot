using InstagramApiSharp;
using InstagramApiSharp.API.Processors;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using System.Collections.Generic;
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

        public User(IUserProcessor apiUserProcessor, ConfigurationEntity configuration)
        {
            _configuration = configuration;
            _apiUserProcessor = apiUserProcessor;
            _paginationParameters = PaginationParameters.MaxPagesToLoad(configuration.MaxPagesToLoad);
        }

        public async Task<ResponseEntity<dynamic>> FollowAsync(string userName, string fromNextId = null)
        {
            var param = (!string.IsNullOrEmpty(fromNextId) ?
                _paginationParameters.StartFromMaxId(fromNextId) :
                _paginationParameters);

            var result = await _apiUserProcessor.GetUserFollowersAsync(userName, param);
            
            var response = new ResponseEntity<dynamic>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
            };

            if (result.Succeeded)
            {
                var RequestingUser = new List<long>();
                var users = result.Value;

                foreach (var user in users)
                {
                    if ((await _apiUserProcessor.FollowUserAsync(user.Pk)).Succeeded)
                    {
                        RequestingUser.Add(user.Pk);
                    }
                    else
                    {

                    }
                }

                response.ResponseData = new
                {
                    RequestingUser,
                    NextMaxId = users?.NextMaxId ?? string.Empty
                };
            }

            return response;
        }

        public async void UnFollowAsyncNew(long[] FollowerRequesting)
        {
            IResult<InstaFriendshipFullStatus> user;

            foreach (var userPk in FollowerRequesting)
            {
                user = await _apiUserProcessor.UnFollowUserAsync(userPk);

                if (user.Succeeded)
                {

                }
                else
                {

                }
            }
        }

        public async Task<InstaUserShortList> UnFollowAsync()
        {
            var result = await _apiUserProcessor.GetUserFollowingAsync(_configuration.UserName, _paginationParameters);

            if (result.Succeeded)
            {
                result.Value.RemoveRange(_configuration.PagingData - 1, result.Value.Count() - _configuration.PagingData);

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
