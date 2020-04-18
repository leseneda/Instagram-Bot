using InstagramApiSharp.Classes;
using MeConecta.Gram.Domain.Entity;
using System.Threading.Tasks;

namespace MeConecta.Gram.Domain.Interface
{
    public interface ICoreConnector : IBuild
    {
        public ICoreMessage Message { get; }
        public ICoreUser User { get; }
        public ICoreHashTag HashTag { get; }
        public ICoreLocation Location { get; }

        Task<ResponseEntity<IResult<InstaLoginResult>>> LoginAsync();
        Task<ResponseEntity<IResult<bool>>> LogoutAsync();
    }
}
