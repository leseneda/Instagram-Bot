using InstagramApiSharp.Classes;
using System.Threading.Tasks;

namespace MeConecta.Gram.Domain.Interface
{
    public interface IConnector : IBuild
    {
        public IMessage Message { get; }
        public IInstaUser User { get; }
        public IHashTag HashTag { get; }
        public ILocation Location { get; }

        Task<bool> LoginAsync();
        Task<IResult<bool>> LogoutAsync();
    }
}
