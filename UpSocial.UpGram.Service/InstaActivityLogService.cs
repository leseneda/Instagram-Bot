using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;
using System.Threading.Tasks;

namespace MeConecta.Gram.Service
{
    public class InstaActivityLogService : IInstaActivityLogService
    {
        static readonly IBaseService<ActivityLogEntity> _activityLogService = BaseService<ActivityLogEntity>.Build();
        static IInstaActivityLogService _activityLogInstance;

        public static IInstaActivityLogService Build()
        {
            _activityLogInstance ??= new InstaActivityLogService();

            return _activityLogInstance;
        }

        public static async Task<bool> Input(ActivityLogEntity activityLog)
        {
            var activityLogBase = _activityLogService
                .GetFirst(item => item.AccountId == activityLog.AccountId);

            if (activityLogBase != null)
            {
                activityLog.Id = activityLogBase.Id;
                activityLogBase = activityLog;

                return await _activityLogService.PutAsync(activityLogBase).ConfigureAwait(false);
            }

            return (await _activityLogService.PostAsync(activityLog).ConfigureAwait(false) > 0);
        }

        public bool SetNextActivity(long accountId)
        {
            var activityLogBase = _activityLogService
                .GetFirst(item => item.AccountId == accountId);

            if (activityLogBase != null)
            {


                return true;
            }

            return false;
        }
    }
}
