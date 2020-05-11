using InstagramApiSharp.Classes;
using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Enum;
using MeConecta.Gram.Domain.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace MeConecta.Gram.Service
{
    public class InstaActivityLogService : IInstaActivityLogService
    {
        #region Field

        static readonly IBaseService<ActivityLogEntity> _activityLogService = BaseService<ActivityLogEntity>.Build();
        static IInstaActivityLogService _activityLogInstance;

        #endregion

        #region Constructor

        public static IInstaActivityLogService Build()
        {
            _activityLogInstance ??= new InstaActivityLogService();

            return _activityLogInstance;
        }

        #endregion

        #region Public

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
                if (activityLogBase.Succeeded)
                {
                    var xx = activityLogBase.ActivityType;

                    var xxx = Enumeration.GetAll<ActivityTypeEnum>().FirstOrDefault(item => item.Id == activityLogBase.ActivityType);

                }
                else
                {
                    ResponseType responseType = (ResponseType)System.Enum.Parse(typeof(ResponseType), activityLogBase.ResponseType);

                    

                    //enum Colors { Red, Green, Blue }
                    //Colors color = (Colors)System.Enum.Parse(typeof(Colors), "Green");

                }

                return true;
            }

            return false;
        }

        #endregion
    }
}
