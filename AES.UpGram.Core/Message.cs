using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Processors;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using System.Linq;
using System.Threading.Tasks;
using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;

namespace MeConecta.Gram.Core
{
    public class Message : IMessage
    {
        #region Field

        static IUserProcessor _apiUserProcessor;
        static IMessagingProcessor _apiMessagingProcessor;
        static PaginationParameters _paginationParameters;

        #endregion

        private Message(IInstaApi apiConnector, ConfigurationEntity configuration)
        {
            _apiUserProcessor = apiConnector.UserProcessor;
            _apiMessagingProcessor = apiConnector.MessagingProcessor;
            _paginationParameters = PaginationParameters.MaxPagesToLoad(configuration.MaxPagesToLoad);
        }

        public static IMessage Build(IInstaApi apiConnector, ConfigurationEntity configuration)
        {
            return new Message(apiConnector, configuration);
        }

        #region Messaging

        public async Task<ResponseEntity<IResult<InstaDirectInboxThreadList>>> SendDirectMessage(string userName, string message)
        {
            var user = await _apiUserProcessor.GetUserAsync(userName);

            var userPk = (user?.Succeeded ?? false) ?
                user?.Value?.Pk.ToString() ?? string.Empty :
                string.Empty;

            var result = await _apiMessagingProcessor.SendDirectTextAsync(userPk, null, message);

            return new ResponseEntity<IResult<InstaDirectInboxThreadList>>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
                ResponseData = result
            };
        }

        public async Task<ResponseEntity<IResult<InstaDirectInboxThreadList>>> SendDirectMessage(string[] usersName, string message)
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

            return new ResponseEntity<IResult<InstaDirectInboxThreadList>>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
                ResponseData = result
            };
        }

        public async Task<ResponseEntity<IResult<bool>>> SendDirectMessageLink(string userName, string link, string message)
        {
            var inbox = await _apiMessagingProcessor.GetDirectInboxAsync(_paginationParameters);
            var thread = inbox.Value.Inbox.Threads
                .FirstOrDefault(u => u.Users.FirstOrDefault().UserName.ToLower() == userName.ToLower());

            var result = await _apiMessagingProcessor.SendDirectLinkAsync(message, link, thread?.ThreadId);

            return new ResponseEntity<IResult<bool>>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
                ResponseData = result
            };
        }

        #endregion
    }
}
