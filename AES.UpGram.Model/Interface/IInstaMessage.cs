using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using System.Threading.Tasks;
using MeConecta.Gram.Domain.Entity;

namespace MeConecta.Gram.Domain.Interface
{
    public interface IInstaMessage : IInstaBuild
    {
        Task<ResponseBaseEntity<IResult<InstaDirectInboxThreadList>>> DirectMessage(string userName, string message);
        Task<ResponseBaseEntity<IResult<InstaDirectInboxThreadList>>> DirectMessage(string[] usersName, string message);
        Task<ResponseBaseEntity<IResult<bool>>> DirectMessageLink(string userName, string link, string message);
    }
}
