using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using System.Threading.Tasks;
using MeConecta.Gram.Domain.Entity;

namespace MeConecta.Gram.Domain.Interface
{
    public interface IMessageCore : IBuild
    {
        Task<ResponseEntity<IResult<InstaDirectInboxThreadList>>> SendDirectMessage(string userName, string message);
        Task<ResponseEntity<IResult<InstaDirectInboxThreadList>>> SendDirectMessage(string[] usersName, string message);
        Task<ResponseEntity<IResult<bool>>> SendDirectMessageLink(string userName, string link, string message);
    }
}
