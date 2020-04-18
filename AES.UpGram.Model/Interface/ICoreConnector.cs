using InstagramApiSharp.Classes;
using System.Threading.Tasks;

namespace MeConecta.Gram.Domain.Interface
{
    public interface ICoreConnector : IBuild
    {
        public ICoreMessage Message { get; }
        public ICoreUser User { get; }
        public ICoreHashTag HashTag { get; }
        public ICoreLocation Location { get; }

        Task<bool> LoginAsync();
        Task<IResult<bool>> LogoutAsync();
    }
}
