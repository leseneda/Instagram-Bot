using MeConecta.Gram.Domain.Interface;
using MeConecta.Gram.Service;

namespace MeConecta.Gram.Console
{
    public class UnFollowRequested
    {
        public void Execute(IConnectorCore connector)
        {
            var service = InstaUserService.Build(connector.User);
            var request = service.UnfollowAsync().Result;
        }
    }
}
