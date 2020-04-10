using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Processors;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using System.Linq;
using System.Threading.Tasks;
using UpSocial.UpGram.Domain.Entity;
using UpSocial.UpGram.Domain.Interface;

namespace UpSocial.UpGram.Core
{
    public class Message : IInstaMessage
    {
        static IUserProcessor _apiUserProcessor;
        static IMessagingProcessor _apiMessagingProcessor;

        private Message(IInstaApi apiConnector)
        {
            _apiUserProcessor = apiConnector.UserProcessor;
            _apiMessagingProcessor = apiConnector.MessagingProcessor;
        }

        public static IInstaMessage Builder(IInstaApi apiConnector)
        {
            return new Message(apiConnector);
        }

        public async Task<ResponseBaseEntity<IResult<InstaDirectInboxThreadList>>> DirectMessage(string userName, string message)
        {
            var user = await _apiUserProcessor.GetUserAsync(userName);

            var userPk = (user?.Succeeded ?? false) ? 
                user?.Value?.Pk.ToString() ?? string.Empty : 
                string.Empty;

            var result = await _apiMessagingProcessor.SendDirectTextAsync(userPk, null, message);

            return new ResponseBaseEntity<IResult<InstaDirectInboxThreadList>>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
                ResponseData = result
            };
        }

        public async Task<ResponseBaseEntity<IResult<InstaDirectInboxThreadList>>> DirectMessage(string[] usersName, string message)
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

            var result = await _apiMessagingProcessor.SendDirectTextAsync(recipients, null, message);

            return new ResponseBaseEntity<IResult<InstaDirectInboxThreadList>>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
                ResponseData = result
            };
        }

        public async Task<ResponseBaseEntity<IResult<bool>>> DirectMessageLink(string userName, string link, string message)
        {
            var inbox = await _apiMessagingProcessor.GetDirectInboxAsync(PaginationParameters.MaxPagesToLoad(1));
            var thread = inbox.Value.Inbox.Threads
                .FirstOrDefault(u => u.Users.FirstOrDefault().UserName.ToLower() == userName.ToLower());

            var result = await _apiMessagingProcessor.SendDirectLinkAsync(message, link, thread?.ThreadId);

            return new ResponseBaseEntity<IResult<bool>>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
                ResponseData = result
            };
        }
    }
}
