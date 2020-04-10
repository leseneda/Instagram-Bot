using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using UpSocial.UpGram.Domain.Entity;

namespace UpSocial.UpGram.Domain.Interface
{
    public interface IInstaUser : IInstaBuild
    {
        Task<ResponseBaseEntity<ResponseFollowerEntity>> FollowAsync(string userName, string nextMaxId = null);
        Task<ResponseBaseEntity<IList<long>>> UnFollowAsync(long[] followerRequesting);
        Task<IResult<InstaUserInfo>> UserAsync(string userName);
    }
}
