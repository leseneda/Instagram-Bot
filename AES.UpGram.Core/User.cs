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

        public User(IUserProcessor apiUserProcessor, ConfigurationEntity configuration)
        {
            _configuration = configuration;
            _apiUserProcessor = apiUserProcessor;
            _paginationParameters = PaginationParameters.MaxPagesToLoad(configuration.MaxPagesToLoad);
        }

        public async Task<ResponseBaseEntity<ResponseFollowerEntity>> FollowAsync(string userName, string nextMaxId = null)
        {
            var param = (!string.IsNullOrEmpty(nextMaxId) ?
                _paginationParameters.StartFromMaxId(nextMaxId) :
                _paginationParameters);

            var result = await _apiUserProcessor.GetUserFollowersAsync(userName, param);
            
            var responseBase = new ResponseBaseEntity<ResponseFollowerEntity>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
                ResponseType = result.Info.ResponseType.ToString()
            };

            var responseFollower = new ResponseFollowerEntity();

            if (result.Succeeded)
            {
                var users = result.Value;
                
                IResult<InstaFriendshipFullStatus> request;

                foreach (var user in users)
                {
                    request = await _apiUserProcessor.FollowUserAsync(user.Pk);

                    if (request.Succeeded)
                    {
                        responseFollower.RequestedUserPk.Add(user.Pk);
                    }
                    else
                    {
                        responseFollower.NextMaxId = nextMaxId;

                        responseBase.Succeeded = request.Succeeded;
                        responseBase.Message = request.Info.Message;
                        responseBase.ResponseType = request.Info.ResponseType.ToString();
                        responseBase.ResponseData = responseFollower;

                        return responseBase;
                    }
                }
                responseFollower.NextMaxId = users?.NextMaxId ?? string.Empty;
                responseBase.ResponseData = responseFollower;
            }

            return responseBase;
        }

        public async Task<ResponseBaseEntity<IResult<InstaFriendshipFullStatus>>> UnFollowAsync(long[] FollowerRequesting)
        {
            var responseBase = new ResponseBaseEntity<IResult<InstaFriendshipFullStatus>>();

            IResult<InstaFriendshipFullStatus> user;

            foreach (var userPk in FollowerRequesting)
            {
                user = await _apiUserProcessor.UnFollowUserAsync(userPk);

                if (!user.Succeeded)
                {
                    responseBase.Succeeded = user.Succeeded;
                    responseBase.ResponseData = user;

                    break;
                }
            }

            return responseBase;
        }

        public async Task<IResult<InstaUserInfo>> UserAsync(string userName)
        {
            return await _apiUserProcessor.GetUserInfoByUsernameAsync(userName);
        }
    }
}
