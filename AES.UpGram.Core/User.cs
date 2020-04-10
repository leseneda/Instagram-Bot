﻿using InstagramApiSharp;
using InstagramApiSharp.API.Processors;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using UpSocial.UpGram.Domain.Entity;

namespace UpSocial.UpGram.Core
{
    public class User
    {
        static IUserProcessor _apiUserProcessor;
        static PaginationParameters _paginationParameters;

        public User(IUserProcessor apiUserProcessor, ConfigurationEntity configuration)
        {
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
                        responseBase.ResponseType = request.Info.ResponseType.ToString();
                        responseBase.ResponseData = responseFollow;

                        return responseBase;
                    }
                }
                responseFollow.NextMaxId = users?.NextMaxId ?? string.Empty;
                responseBase.ResponseData = responseFollow;
            }

            return responseBase;
        }

        public async Task<ResponseBaseEntity<IList<long>>> UnFollowAsync(long[] followerRequesting)
        {
            var responseBase = new ResponseBaseEntity<IList<long>>();

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

        public async Task<IResult<InstaUserInfo>> UserAsync(string userName)
        {
            return await _apiUserProcessor.GetUserInfoByUsernameAsync(userName);
        }
    }
}
