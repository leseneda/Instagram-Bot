using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;
using MeConecta.Gram.Service;

namespace MeConecta.Gram.Console
{
    public class FollowerRequesting
    {
        public void Execute(IConnectorCore connector, long accountUserNameId)
        {
            var service = InstaUserService.Build(connector.User);
            var request = service.FollowAsync(accountUserNameId).Result;

            if (!request)
            { 
            
            }
        }
    }
}
