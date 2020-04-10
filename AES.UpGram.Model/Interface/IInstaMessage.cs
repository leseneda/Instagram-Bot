using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using System.Threading.Tasks;
using UpSocial.UpGram.Domain.Entity;

namespace UpSocial.UpGram.Domain.Interface
{
    public interface IInstaMessage : IInstaBuild
    {
        Task<ResponseBaseEntity<IResult<InstaDirectInboxThreadList>>> DirectMessage(string userName, string message);
        Task<ResponseBaseEntity<IResult<InstaDirectInboxThreadList>>> DirectMessage(string[] usersName, string message);
        Task<ResponseBaseEntity<IResult<bool>>> DirectMessageLink(string userName, string link, string message);
    }
}
