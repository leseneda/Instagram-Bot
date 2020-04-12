using System.Threading.Tasks;

namespace MeConecta.Gram.Domain.Interface
{
    public interface IInstaConnector : IInstaBuild
    {
        Task<bool> LoginAsync();
        void LogoutAsync();
        public IInstaMessage Message { get; }
        public IInstaUser User { get; }
        public IInstaHashTag HashTag { get; }
        public IInstaLocation Location { get; }
    }
}
