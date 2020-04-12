using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MeConecta.Gram.Domain.Entity;

namespace MeConecta.Gram.Domain.Interface
{
    public interface IInstaUser : IInstaBuild
    {
        Task<ResponseEntity<ResponseFollowerEntity>> RequestFollowersAsync(string userName, string nextMaxId = null);
        Task<ResponseEntity<IList<long>>> UnfollowAsync(long[] followerRequesting);
        Task<ResponseEntity<IResult<InstaUserInfo>>> GetUserAsync(string userName);
        Task<ResponseEntity<IResult<InstaDiscoverSearchResult>>> SearchUser(string search, int counterData);
    }
}
