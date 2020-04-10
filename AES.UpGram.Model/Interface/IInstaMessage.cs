using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using System.Threading.Tasks;
using MeConecta.Gram.Domain.Entity;

namespace MeConecta.Gram.Domain.Interface
{
    public interface IInstaMessage : IInstaBuild
    {
        Task<ResponseEntity<IResult<InstaDirectInboxThreadList>>> DirectMessage(string userName, string message);
        Task<ResponseEntity<IResult<InstaDirectInboxThreadList>>> DirectMessage(string[] usersName, string message);
        Task<ResponseEntity<IResult<bool>>> DirectMessageLink(string userName, string link, string message);
    }
}
