using System.Threading.Tasks;

namespace MeConecta.Gram.Domain.Interface
{
    public interface IConnector : IBuild
    {
        Task<bool> LoginAsync();
        void LogoutAsync();
        public IMessage Message { get; }
        public IInstaUser User { get; }
        public IHashTag HashTag { get; }
        public ILocation Location { get; }
    }
}
