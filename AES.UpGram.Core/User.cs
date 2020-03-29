using AES.UpGram.Core.Utils;
using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Processors;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AES.UpGram.Core
{
    public class User
    {
        private static IUserProcessor _apiUser;
        private static PaginationParameters _paginationParameters;
        string _accountName = string.Empty;

        public User(IInstaApi apiConnector, string accountName)
        {
            _apiUser = apiConnector.UserProcessor;
            _accountName = accountName;
            _paginationParameters = PaginationParameters.MaxPagesToLoad(1);
        }

        public async Task<string> Follow(string fromAccountName, string fromNextId = null)
        {
            var param = (!string.IsNullOrEmpty(fromNextId) ?
                _paginationParameters.StartFromMaxId(fromNextId) :
                _paginationParameters);

            var users = await _apiUser.GetUserFollowersAsync(fromAccountName, param);

            if (users.Succeeded)
            {
                foreach (var user in users.Value)
                {
                    if ((await _apiUser.FollowUserAsync(user.Pk)).Succeeded)
                    { 
                    
                    }
                }
            }

            return users.Value?.NextMaxId ?? string.Empty;
        }

        public async Task<bool> UnFollow()
        {
            var unFollowing = await UnFollowFromSource();
            var unFollowed = new List<long>();

            foreach (var pk in unFollowing)
            {
                if ((await _apiUser.UnFollowUserAsync(pk)).Succeeded)
                {
                    unFollowed.Add(pk);
                }
            }

            unFollowing.RemoveAll(cmp => unFollowed.Any(pk => pk == cmp));

            FileManagement.WriteToBinaryFile(@"C:\Source Code\following.txt", unFollowing);

            return true;
        }

        private async Task<List<long>> UnFollowFromSource()
        {
            FileManagement.CreatePath(@"C:\Source Code\UnFollow\");

            if (FileManagement.FromFile(@"C:\Source Code\UnFollow\following.txt"))
            {
                return FileManagement.ReadFromBinaryFile<List<long>>(@"C:\Source Code\following.txt");
            }
            else
            {
                var following = (await _apiUser.GetUserFollowingAsync(_accountName, _paginationParameters)).Value
                    .Select(cmp => cmp.Pk)
                    .ToList();

                FileManagement.WriteToBinaryFile(@"C:\Source Code\UnFollow\following.txt", following);

                return following;
            }
        }

       
    }
}
