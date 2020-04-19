using MeConecta.Gram.Core;
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

        private InstaUserService(ConfigurationEntity configuration)
        {
            var coreConnector = CoreConnector.Build(configuration);
            var result = coreConnector.LoginAsync().Result;

            _coreUser = result.Succeeded ? coreConnector.User : null;
        }

        private InstaUserService(ICoreUser coreUser)
        {
            _coreUser = coreUser;
        }

        #endregion

        #region Build

        public static InstaUserService Build(ConfigurationEntity configuration)
        {
            return new InstaUserService(configuration);
        }

        public static InstaUserService Build(string accountName)
        {
            var serviceConfig = BaseServiceReadOnly<ConfigurationEntity>.Build();
            var baseConfig = serviceConfig.GetFirst();

            var serviceAccount = BaseServiceReadOnly<AccountEntity>.Build();
            var baseAccount = serviceAccount
                .GetFirst(cmp => cmp.Name.ToLower() == accountName.ToLower());

            baseConfig.Account = baseAccount;

            return new InstaUserService(baseConfig);
        }

        public static InstaUserService Build(ICoreUser coreUser)
        {
            return new InstaUserService(coreUser);
        }

        #endregion

        public async Task<bool> UnfollowAsync()
        {
            var serviceRequest = BaseService<AccountFollowerRequestEntity>.Build();
            var baseRequest = serviceRequest.GetWhere(cmp => cmp.IsActive 
                && cmp.AccountId == _coreUser.Account.Id)
                .FirstOrDefault();

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
            var serviceRequest = BaseService<AccountFollowerRequestEntity>.Build();
            var baseRequest = serviceRequest.GetWhere(cmp => cmp.IsActive
                && cmp.AccountId == _coreUser.Account.Id)
                .LastOrDefault();

            var result = await _coreUser.FollowAsync(fromAccountName, baseRequest?.FromMaxId ?? string.Empty);

            if (result.Succeeded)
            {
                var hasNextMaxId = !string.IsNullOrWhiteSpace(result.ResponseData.NextMaxId);

                using var scope = new TransactionScope();
                
                await serviceRequest.PostAsync(new AccountFollowerRequestEntity()
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
                    var serviceFollower = BaseService<AccountFollowerEntity>.Build();
                    var baseFollower = await serviceFollower.GetAsync(baseRequest.AccountFollowerId);

                    baseFollower.IsActive = false;

                    await serviceFollower.PutAsync(baseFollower);
                }

                // Activity log

                scope.Complete();
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
