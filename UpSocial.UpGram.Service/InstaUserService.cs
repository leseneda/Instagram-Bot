using MeConecta.Gram.Domain.Entity;
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

        #endregion

        #region Constructor

        private InstaUserService(ICoreUser coreUser)
        {
            _coreUser = coreUser;
        }

        public static InstaUserService Build(ICoreUser coreUser)
        {
            return new InstaUserService(coreUser);
        }

        #endregion

        public async Task<bool> UnfollowAsync()
        {
            var serviceRequest = BaseService<FollowerRequestEntity>.Build();
            var baseRequest = serviceRequest.GetFirst(cmp => cmp.IsActive 
                && cmp.AccountId == _coreUser.Account.Id);

            var followerPk = JsonSerializer.Deserialize<long[]>(baseRequest.FollowerRequestPk);
            var result = await _coreUser.UnfollowAsync(followerPk.ToArray());

            if (result.Succeeded)
            {
                var followerLeft = followerPk.Except(result.ResponseData);
                var hasFollowerLeft = (followerLeft.Count() > 0);

                baseRequest.FollowerRequestPk = hasFollowerLeft ? JsonSerializer.Serialize(followerLeft) : null;
                baseRequest.IsActive = hasFollowerLeft ? true : false;

                await serviceRequest.PutAsync(baseRequest);

                // Activity log
            }
            else
            {
                // Activity log
            }

            return result.Succeeded;
        }

        public async Task<bool> FollowAsync(string fromAccountName)
        {
            var serviceRequest = BaseService<FollowerRequestEntity>.Build();
            var baseRequest = serviceRequest.GetLast(cmp => cmp.IsActive
                && cmp.AccountId == _coreUser.Account.Id);

            var result = await _coreUser.FollowAsync(fromAccountName, baseRequest.FromMaxId ?? string.Empty);

            if (result.Succeeded)
            {
                var hasNextMaxId = !string.IsNullOrWhiteSpace(result.ResponseData.NextMaxId);

                using var scope = new TransactionScope();
                {
                    await serviceRequest.PostAsync(new FollowerRequestEntity()
                    {
                        AccountId = _coreUser.Account.Id,
                        AccountFollowerId = baseRequest.AccountFollowerId,
                        FromMaxId = result.ResponseData.NextMaxId,
                        Message = result.Message,
                        Succeeded = result.Succeeded,
                        ResponseType = "OK",
                        FollowerRequestPk = JsonSerializer.Serialize(result.ResponseData.RequestedUserId),
                        IsActive = hasNextMaxId ? true : false
                    });

                    if (!hasNextMaxId)
                    {
                        var serviceFollower = BaseService<AccountUserNameEntity>.Build();
                        var baseFollower = await serviceFollower.GetAsync(baseRequest.AccountFollowerId);

                        baseFollower.IsActive = false;

                        await serviceFollower.PutAsync(baseFollower);
                    }

                    // Activity log

                    scope.Complete();
                }
            }
            else
            {
                // Activity log
            }
                
            return result.Succeeded;
            //}
        }
    }
}
