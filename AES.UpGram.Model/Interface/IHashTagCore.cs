using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using MeConecta.Gram.Domain.Entity;
using System.Threading.Tasks;

namespace MeConecta.Gram.Domain.Interface
{
    public interface IHashTagCore
    {
        Task<ResponseEntity<IResult<InstaSectionMedia>>> GetTopHashtagListAsync(string tagName);
        Task<ResponseEntity<IResult<InstaSectionMedia>>> GetRecentHashtagListAsync(string tagName);
    }
}
