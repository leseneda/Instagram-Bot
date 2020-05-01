using InstagramApiSharp.Classes;
using MeConecta.Gram.Domain.Entity;
using System.Threading.Tasks;

namespace MeConecta.Gram.Domain.Interface
{
    public interface IConnectorCore
    {
        public IMessageCore Message { get; }
        public IUserCore User { get; }
        public IHashTagCore HashTag { get; }
        public ILocationCore Location { get; }

        Task<ResponseEntity<IResult<InstaLoginResult>>> LoginAsync();
        Task<ResponseEntity<IResult<bool>>> LogoutAsync();
    }
}
