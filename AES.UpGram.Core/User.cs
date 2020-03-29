using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Processors;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using System.Linq;
using System.Threading.Tasks;

namespace AES.UpGram.Core
{
    public class User
    {
        private static IUserProcessor _apiUser;
        private static PaginationParameters _paginationParameters;
        string _accountName = string.Empty;

        public User(IInstaApi apiConnector, int maxPageToLoad, string accountName)
        {
            _apiUser = apiConnector.UserProcessor;
            _paginationParameters = PaginationParameters.MaxPagesToLoad(maxPageToLoad);
            _accountName = accountName;
        }

        public async Task<string> FollowFrom(string accountName, string fromNextId = null)
        {
            var param = (!string.IsNullOrEmpty(fromNextId) ?
                _paginationParameters.StartFromMaxId(fromNextId) :
                _paginationParameters);

            var users = await _apiUser.GetUserFollowersAsync(accountName, param);

            if (users.Succeeded)
            {
                return string.Empty;
            }

            IResult<InstaFriendshipFullStatus> result;

            foreach (var user in users.Value)
            {
                result = await _apiUser.FollowUserAsync(user.Pk);
            }

            return users.Value?.NextMaxId ?? string.Empty;
        }

        public async Task<bool> UnFollowFrom()
        {
            var followers = await _apiUser.GetUserFollowersAsync(_accountName, _paginationParameters);
            var following = await _apiUser.GetUserFollowingAsync(_accountName, _paginationParameters);
            var nonFollowing = followers.Value.Except(following.Value);

            IResult<InstaFriendshipFullStatus> result;

            foreach (InstaUserShort user in nonFollowing)
            {
                result = await _apiUser.UnFollowUserAsync(user.Pk);
            }

            return true;
        }
    }
}
