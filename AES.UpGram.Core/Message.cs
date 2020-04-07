using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Processors;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using System.Linq;
using System.Threading.Tasks;

namespace UpSocial.UpGram.Core
{
    public class Message
    {
        static IUserProcessor _apiUserProcessor;
        static IMessagingProcessor _apiMessagingProcessor;

        public Message(IInstaApi apiConnector)
        {
            _apiUserProcessor = apiConnector.UserProcessor;
            _apiMessagingProcessor = apiConnector.MessagingProcessor;
        }

        public async Task<IResult<InstaDirectInboxThreadList>> DirectMessage(string userName, string message)
        {
            var user = await _apiUserProcessor.GetUserAsync(userName);

            var userPk = (user?.Succeeded ?? false) ? 
                user?.Value?.Pk.ToString() ?? string.Empty : 
                string.Empty;

            return await _apiMessagingProcessor.SendDirectTextAsync(userPk, null, message);
        }

        public async Task<IResult<InstaDirectInboxThreadList>> DirectMessage(string[] usersName, string message)
        {
            IResult<InstaUser> user;
            var recipients = string.Empty;

            foreach (var userName in usersName)
            {
                user = await _apiUserProcessor.GetUserAsync(userName);

                recipients = (user?.Succeeded ?? false) ? 
                    string.Concat(recipients, user?.Value?.Pk.ToString() ?? string.Empty, ",") : 
                    recipients;
            }

            return await _apiMessagingProcessor.SendDirectTextAsync(recipients, null, message);
        }

        public async Task<IResult<bool>> DirectMessageLink(string userName, string link, string message)
        {
            var inbox = await _apiMessagingProcessor.GetDirectInboxAsync(PaginationParameters.MaxPagesToLoad(1));
            var thread = inbox.Value.Inbox.Threads
                .FirstOrDefault(u => u.Users.FirstOrDefault().UserName.ToLower() == userName.ToLower());
            
            return await _apiMessagingProcessor.SendDirectLinkAsync(message, link, thread?.ThreadId);
        }
    }
}
