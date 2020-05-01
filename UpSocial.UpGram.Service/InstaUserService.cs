using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Enum;
using MeConecta.Gram.Domain.Interface;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace MeConecta.Gram.Service
{
    public class InstaUserService : IInstaUserService
    {
        #region Field

        static IUserCore _userCore;
        readonly sbyte _amountAttemptUnfollowing = 3;
        readonly IBaseService<FollowerRequestEntity> _followerRequestService;
        readonly IBaseService<ActivityLogEntity> _activityLogService;
        readonly IBaseService<AccountUserNameEntity> _accountUserNameService;

        #endregion

        #region Constructor

        private InstaUserService(IUserCore userCore)
        {
            _userCore = userCore;
            _followerRequestService = BaseService<FollowerRequestEntity>.Build();
            _activityLogService = BaseService<ActivityLogEntity>.Build();
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

            if (result.Succeeded)
            {
                var followerLeft = followersRequest.Except(result.ResponseData);
                var hasFollowerLeft = (followerLeft.Count() > 0);

                followerRequestBase.FollowerRequestPk = hasFollowerLeft ? JsonSerializer.Serialize(followerLeft) : null;
                followerRequestBase.IsActive = hasFollowerLeft ? true : false;

                await _followerRequestService.PutAsync(followerRequestBase)
                    .ConfigureAwait(false);
            }
            else
            {
                followerRequestBase.IsActive = !(_amountAttemptUnfollowing == (followerRequestBase.AmountAttemptUnfollowing + 1));
                followerRequestBase.AmountAttemptUnfollowing++;

                await _followerRequestService.PutAsync(followerRequestBase)
                    .ConfigureAwait(false);
            }

            await _activityLogService.PostAsync(new ActivityLogEntity()
            {
                ActivityType = ActivityTypeEnum.Unfollow.Id,
                TableId = followerRequestBase.Id,
                Message = result.Message,
                ResponseType = followerRequestBase.IsActive ?
                    "It is not be able to unfollow" :
                    "Reached maximum attempts to unfollow",
                Succeeded = result.Succeeded
            })
                .ConfigureAwait(false);

            return result.Succeeded;
        }

        public async Task<bool> FollowAsync(string fromAccountName)
        {
            var followerRequestBase = _followerRequestService.GetLast(cmp => cmp.IsActive
                && cmp.AccountId == _userCore.Account.Id);

            var result = await _userCore.FollowAsync(fromAccountName, followerRequestBase.FromMaxId ?? string.Empty)
                .ConfigureAwait(false);

            if (result.Succeeded)
            {
                var hasNextMaxId = !string.IsNullOrWhiteSpace(result.ResponseData.NextMaxId);

                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                {
                    await _followerRequestService.PostAsync(new FollowerRequestEntity()
                    {
                        AccountId = _userCore.Account.Id,
                        AccountUserNameId = followerRequestBase.AccountUserNameId,
                        FromMaxId = result.ResponseData.NextMaxId,
                        Message = result.Message,
                        ResponseType = "OK",
                        FollowerRequestPk = JsonSerializer.Serialize(result.ResponseData.RequestedUserId),
                        AmountAttemptUnfollowing = 0,
                        IsActive = hasNextMaxId ? true : false,
                    })
                        .ConfigureAwait(false);

                    if (!hasNextMaxId)
                    {
                        var accountUserNameBase = await _accountUserNameService.GetAsync(followerRequestBase.AccountUserNameId)
                            .ConfigureAwait(false);

                        accountUserNameBase.IsActive = false;

                        await _accountUserNameService.PutAsync(accountUserNameBase)
                            .ConfigureAwait(false);
                    }

                    scope.Complete();
                }
            }

            await _activityLogService.PostAsync(new ActivityLogEntity()
            {
                ActivityType = ActivityTypeEnum.FollowerByAccountName.Id,
                TableId = followerRequestBase.Id,
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
