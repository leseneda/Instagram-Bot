﻿using AES.UpGram.Model;
using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Processors;
using InstagramApiSharp.Classes.Models;
using System.Linq;
using System.Threading.Tasks;

namespace AES.UpGram.Core
{
    public class User
    {
        private static IUserProcessor _apiUserProcessor;
        private static PaginationParameters _paginationParameters;
        private static Configuration _configuration;

        public User(IInstaApi apiConnector, Configuration configuration)
        {
            _apiUserProcessor = apiConnector.UserProcessor;
            _configuration = configuration;

            _paginationParameters = PaginationParameters.MaxPagesToLoad(1);
        }

        public async Task<string> Follow(string userName, string fromNextId = null)
        {
            var param = (!string.IsNullOrEmpty(fromNextId) ?
                _paginationParameters.StartFromMaxId(fromNextId) :
                _paginationParameters);

            var result = await _apiUserProcessor.GetUserFollowersAsync(userName, param);

            if (result.Succeeded)
            {
                var users = result.Value;
                
                foreach (var user in users)
                {
                    if ((await _apiUserProcessor.FollowUserAsync(user.Pk)).Succeeded)
                    { 
                    
                    }
                    else
                    {

                    }
                }
                
                return users?.NextMaxId ?? string.Empty;
            }

            return null;
        }

        public async Task<InstaUserShortList> UnFollow()
        {
            var result = await _apiUserProcessor.GetUserFollowingAsync(_configuration.AccountName, _paginationParameters);

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
    }
}
