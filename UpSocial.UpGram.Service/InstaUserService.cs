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

        static ICoreUser _userCore;

        #endregion

        #region Constructor

        private InstaUserService(ICoreUser userCore)
        {
            _userCore = userCore;
        }

        public static InstaUserService Build(ICoreUser userCore)
        {
            return new InstaUserService(userCore);
        }

        #endregion

        public async Task<bool> UnfollowAsync()
        {
            var service = BaseService<AccountFollowerRequestEntity>.Build();
            var request = service.GetWhere(cmp => cmp.IsActive 
                && cmp.AccountId == _userCore.Account.Id)
                .FirstOrDefault();

            var followerRequestPk = JsonSerializer.Deserialize<long[]>(request.FollowerRequestPk);
            var result = await _userCore.UnfollowAsync(followerRequestPk.ToArray());

            if (result.Succeeded)
            {
                var followerRemained = followerRequestPk.Except(result.ResponseData);
                var hasfollowerRemained = (followerRemained.Count() > 0);

                request.FollowerRequestPk = hasfollowerRemained ? JsonSerializer.Serialize(followerRemained) : null;
                request.IsActive = hasfollowerRemained ? true : false;

                return await service.PutAsync(request);
            }
            else
            {
                // Activity log
            }

            return false;
        }

        public async Task<long> FollowAsync(string fromAccountName)
        {
            var serviceRequest = BaseService<AccountFollowerRequestEntity>.Build();
            var baseRequest = serviceRequest.GetWhere(cmp => cmp.IsActive
                && cmp.AccountId == _userCore.Account.Id)
                .LastOrDefault();

            var result = await _userCore.FollowAsync(fromAccountName, baseRequest?.FromMaxId ?? string.Empty);

            using (var scope = new TransactionScope())
            {
                if (result.Succeeded)
                {
                    var isAllThrough = string.IsNullOrWhiteSpace(result.ResponseData.NextMaxId);

                    await serviceRequest.PostAsync(new AccountFollowerRequestEntity()
                    {
                        AccountId = _userCore.Account.Id,
                        AccountFollowerId = baseRequest.AccountFollowerId,
                        FromMaxId = result.ResponseData.NextMaxId,
                        Message = result.Message,
                        Succeeded = result.Succeeded,
                        ResponseType = "OK",
                        FollowerRequestPk = JsonSerializer.Serialize(result.ResponseData.RequestedUserId),
                        IsActive = isAllThrough ? false : true
                    });

                    if (isAllThrough)
                    {
                        var serviceFollower = BaseService<AccountFollowerEntity>.Build();
                        var baseFollower = await serviceFollower.GetAsync(baseRequest.AccountFollowerId);

                        baseFollower.IsActive = false;

                        await serviceFollower.PutAsync(baseFollower);
                    }
                }
                else
                {
                    // Activity log

                    return 0;
                }

                scope.Complete();
            }

            return 1;
        }

    }
}
