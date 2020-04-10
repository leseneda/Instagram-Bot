using System;
using System.Threading.Tasks;

namespace UpSocial.UpGram.Domain.Interface
{
    public interface IInstaConnector : IInstaBuild
    {
        Task<bool> LoginAsync();
        void LogoutAsync();
        public IInstaMessage Message { get; }
        public IInstaUser User { get; }
    }
}
