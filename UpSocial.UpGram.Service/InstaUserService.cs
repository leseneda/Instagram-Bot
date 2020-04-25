using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Enum;
using MeConecta.Gram.Domain.Interface;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace MeConecta.Gram.Service
{
    public class InstaUserService : IBuild
    {
        #region Field

        static ICoreUser _coreUser;
        readonly sbyte _amountAttemptUnfollowing = 3;
        readonly IBaseService<FollowerRequestEntity> _serviceFollowerRequest;
        readonly IBaseService<ActivityLogEntity> _serviceActivityLog;

        #endregion

        #region Constructor

        private InstaUserService(ICoreUser coreUser)
        {
            _coreUser = coreUser;
            _serviceFollowerRequest = BaseService<FollowerRequestEntity>.Build();
            _serviceActivityLog = BaseService<ActivityLogEntity>.Build();
        }

        public static InstaUserService Build(ICoreUser coreUser)
        {
            return new InstaUserService(coreUser);
        }

        #endregion

        #region Public

        public async Task<bool> UnfollowAsync()
        {
            var baseRequest = GetLastFollowerRequest();
            var followerPk = JsonSerializer.Deserialize<long[]>(baseRequest.FollowerRequestPk);
            var result = await _coreUser.UnfollowAsync(followerPk.ToArray())
                .ConfigureAwait(false);

            if (result.Succeeded)
            {
                var followerLeft = followerPk.Except(result.ResponseData);
                var hasFollowerLeft = (followerLeft.Count() > 0);

                baseRequest.FollowerRequestPk = hasFollowerLeft ? JsonSerializer.Serialize(followerLeft) : null;
                baseRequest.IsActive = hasFollowerLeft ? true : false;

                await _serviceFollowerRequest.PutAsync(baseRequest)
                    .ConfigureAwait(false);
            }
            else
            {
                baseRequest.IsActive = !(_amountAttemptUnfollowing == (baseRequest.AmountAttemptUnfollowing + 1));
                baseRequest.AmountAttemptUnfollowing++;

                await _serviceFollowerRequest.PutAsync(baseRequest)
                    .ConfigureAwait(false);
            }

            await _serviceActivityLog.PostAsync(new ActivityLogEntity()
            {
                ActivityType = ActivityTypeEnum.Unfollow.Id,
                TableId = baseRequest.Id,
                Message = result.Message,
                ResponseType = baseRequest.IsActive ?
                    "It is not be able to unfollow" :
                    "Reached maximum attempts to unfollow",
                Succeeded = result.Succeeded
            })
                .ConfigureAwait(false);

            return result.Succeeded;
        }

        public async Task<bool> FollowAsync(string fromAccountName)
        {
            var baseRequest = GetLastFollowerRequest();

            var result = await _coreUser.FollowAsync(fromAccountName, baseRequest.FromMaxId ?? string.Empty)
                .ConfigureAwait(false);

            if (result.Succeeded)
            {
                var hasNextMaxId = !string.IsNullOrWhiteSpace(result.ResponseData.NextMaxId);

                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                {
                    await _serviceFollowerRequest.PostAsync(new FollowerRequestEntity()
                    {
                        AccountId = _coreUser.Account.Id,
                        AccountUserNameId = baseRequest.AccountUserNameId,
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
                        var serviceFollower = BaseService<AccountUserNameEntity>.Build();
                        var baseFollower = await serviceFollower.GetAsync(baseRequest.AccountUserNameId)
                            .ConfigureAwait(false);

                        baseFollower.IsActive = false;

                        await serviceFollower.PutAsync(baseFollower)
                            .ConfigureAwait(false);
                    }

                    scope.Complete();
                }
            }

            await _serviceActivityLog.PostAsync(new ActivityLogEntity()
            {
                ActivityType = ActivityTypeEnum.FollowerByAccountName.Id,
                TableId = baseRequest.Id,
                Message = result.Message,
                ResponseType = result.ResponseData.ResponseType,
                Succeeded = result.Succeeded
            })
                .ConfigureAwait(false);

            return result.Succeeded;
        }


        #endregion

        #region Private

        private FollowerRequestEntity GetLastFollowerRequest()
        {
            return _serviceFollowerRequest.GetLast(cmp => cmp.IsActive
                && cmp.AccountId == _coreUser.Account.Id);
        }

        #endregion
    }
}
