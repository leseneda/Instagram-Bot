using MeConecta.Gram.Domain.Interface;
using MeConecta.Gram.Service;

namespace MeConecta.Gram.Console
{
    public class UnFollowRequested
    {
        public void Execute(ICoreConnector connector)
        {
            var service = InstaUserService.Build(connector.User);
            var request = service.UnfollowAsync().Result;
        }
    }
}
