﻿using InstagramApiSharp.Classes;
using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Enum;
using MeConecta.Gram.Domain.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace MeConecta.Gram.Service
{
    public class InstaUserService : IInstaUserService
    {
        #region Field

        readonly IUserCore _userCore;
        readonly sbyte _amountAttemptUnfollowing = 3;
        readonly IBaseService<FollowerRequestEntity> _followerRequestService;
        readonly IBaseService<AccountUserNameEntity> _accountUserNameService;

        #endregion

        #region Constructor

        private InstaUserService(IUserCore userCore)
        {
            _userCore = userCore;
            _followerRequestService = BaseService<FollowerRequestEntity>.Build();
            _accountUserNameService = BaseService<AccountUserNameEntity>.Build();
        }

        public static IInstaUserService Build(IUserCore coreUser)
        {
            return new InstaUserService(coreUser);
        }

        #endregion

        #region Public

        public async Task<bool> UnfollowAsync()
        {
            var followerRequestBase = _followerRequestService.GetFirst(cmp => cmp.IsActive
                && cmp.AccountId == _userCore.Account.Id);
            
            var followersRequest = JsonSerializer.Deserialize<long[]>(followerRequestBase.FollowerRequestPk);
            var result = await _userCore.UnfollowAsync(followersRequest.ToArray())
                .ConfigureAwait(false);

            followerRequestBase.FollowerRequestPk = (result.ResponseData.Count() > 0) ? 
                JsonSerializer.Serialize(result.ResponseData) : null;

            if (result.Succeeded)
            {
                followerRequestBase.IsActive = false;

                await _followerRequestService.PutAsync(followerRequestBase)
                    .ConfigureAwait(false);
            }
            else
            {
                followerRequestBase.IsActive = !(_amountAttemptUnfollowing == (followerRequestBase.AmountAttemptUnfollowing + 1));
                followerRequestBase.AmountAttemptUnfollowing++;
                followerRequestBase.Message = result.Message;
                followerRequestBase.ResponseType = result.ResponseType.ToString();

                await _followerRequestService.PutAsync(followerRequestBase)
                    .ConfigureAwait(false);
            }

            await InstaActivityLogService.Input(new ActivityLogEntity()
            {
                ActivityType = ActivityTypeEnum.Unfollow.Id,
                ActivityId = followerRequestBase.Id,
                Message = result.Message,
                AccountId = followerRequestBase.AccountId,
                ResponseType = followerRequestBase.IsActive ?
                    "It is not be able to unfollow" :
                    "Reached maximum attempts to unfollow",
                Succeeded = result.Succeeded

            })
                .ConfigureAwait(false);
            
            return result.Succeeded;
        }

        public async Task<bool> FollowAsync(long AccountUserNameId)
        {
            var followerRequestBase = _followerRequestService.GetLast(cmp => cmp.IsActive
                && cmp.AccountId == _userCore.Account.Id);

            var accountUserNameBase = _accountUserNameService.GetFirst(cmp => cmp.IsActive
                && cmp.Id == AccountUserNameId);

            if (accountUserNameBase == null)
            {
                // erro
            }

            long activityId = 0;

            var followerRemainPk = !string.IsNullOrEmpty(followerRequestBase?.FollowerRemainPk) ?
                JsonSerializer.Deserialize<long[]>(followerRequestBase?.FollowerRemainPk) :
                null;

            var result = await _userCore.FollowAsync(accountUserNameBase.UserName, followerRequestBase?.FromMaxId ?? string.Empty, followerRemainPk)
                .ConfigureAwait(false);

            var message = string.Empty;
            var responseType = string.Empty;

            if (result.Succeeded)
            {
                var hasNextMaxId = !string.IsNullOrWhiteSpace(result.ResponseData.NextMaxId);

                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                {
                    message = result.Message;
                    responseType = result.ResponseType.ToString();

                    activityId = await _followerRequestService.PostAsync(new FollowerRequestEntity()
                    {
                        AccountId = _userCore.Account.Id,
                        AccountUserNameId = accountUserNameBase.Id,
                        FromMaxId = result.ResponseData.NextMaxId,
                        Message = message,
                        ResponseType = responseType,
                        FollowerRequestPk = JsonSerializer.Serialize(result.ResponseData.FollowerRequestPk),
                        FollowerRemainPk = JsonSerializer.Serialize(result.ResponseData.FollowerRemainPk),
                        AmountAttemptUnfollowing = 0,
                        IsActive = hasNextMaxId,
                    })
                        .ConfigureAwait(false);

                    if (!hasNextMaxId)
                    {
                        //########## AO ENTRAR AQUI TEM QUE MUDAR OS DADOS AO INSERIR NO aCTIVITYlOG PARA QUE ELE ENTENDA QUE ACABOU A LISTA A SEGUIR
                        accountUserNameBase.IsActive = false;

                        await _accountUserNameService.PutAsync(accountUserNameBase)
                            .ConfigureAwait(false);
                    }

                    scope.Complete();
                }
            }
            else
            {
                if (result.ResponseData.FollowerRemainPk.Count() > 0)
                {
                    message = result.Message;
                    responseType = result.ResponseType.ToString();

                    followerRequestBase.FollowerRemainPk = JsonSerializer.Serialize(result.ResponseData.FollowerRemainPk);
                    followerRequestBase.Message = message;
                    followerRequestBase.ResponseType = responseType;

                    await _followerRequestService.PutAsync(followerRequestBase)
                        .ConfigureAwait(false);

                    activityId = followerRequestBase.Id;
                }
            }

            await InstaActivityLogService.Input(new ActivityLogEntity()
            {
                ActivityType = ActivityTypeEnum.FollowerByAccountName.Id,
                ActivityId = activityId,
                AccountId = _userCore.Account.Id,
                Message = result.Message,
                ResponseType = result.ResponseData.ResponseType,
                Succeeded = result.Succeeded
            })
                .ConfigureAwait(false);

            return result.Succeeded;
        }

        #endregion
    }
}
