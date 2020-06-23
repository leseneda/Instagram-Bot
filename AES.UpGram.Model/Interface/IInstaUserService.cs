using System.Threading.Tasks;

namespace MeConecta.Gram.Domain.Interface
{
    public interface IInstaUserService
    {
        Task<bool> UnfollowAsync();
        Task<bool> FollowAsync(long AccountUserNameId);
    }
}
