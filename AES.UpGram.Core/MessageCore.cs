using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Processors;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace MeConecta.Gram.Core
{
    public class MessageCore : IMessageCore
    {
        #region Field

        static IUserProcessor _apiUserProcessor;
        static IMessagingProcessor _apiMessagingProcessor;
        static PaginationParameters _paginationParameters;

        #endregion

        #region Constructor

        private MessageCore(IInstaApi apiConnector, ConfigurationEntity configuration)
        {
            _apiUserProcessor = apiConnector.UserProcessor;
            _apiMessagingProcessor = apiConnector.MessagingProcessor;
            _paginationParameters = PaginationParameters.MaxPagesToLoad(configuration.MaxPagesToLoad);
        }

        public static IMessageCore Build(IInstaApi apiConnector, ConfigurationEntity configuration)
        {
            return new MessageCore(apiConnector, configuration);
        }

        #endregion

        #region Messaging

        public async Task<ResponseEntity<IResult<InstaDirectInboxThreadList>>> SendDirectMessage(string userName, string message)
        {
            var user = await _apiUserProcessor.GetUserAsync(userName)
                .ConfigureAwait(false);

            var userPk = (user?.Succeeded ?? false) ?
                user?.Value?.Pk.ToString() ?? string.Empty :
                string.Empty;

            var result = await _apiMessagingProcessor.SendDirectTextAsync(userPk, null, message)
                .ConfigureAwait(false);

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
                user = await _apiUserProcessor.GetUserAsync(userName)
                    .ConfigureAwait(false);

                recipients = (user?.Succeeded ?? false) ?
                    string.Concat(recipients, user?.Value?.Pk.ToString() ?? string.Empty, ",") :
                    recipients;
            }

            var result = await _apiMessagingProcessor.SendDirectTextAsync(recipients, null, message)
                .ConfigureAwait(false);

            return new ResponseEntity<IResult<InstaDirectInboxThreadList>>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
                ResponseData = result
            };
        }

        public async Task<ResponseEntity<IResult<bool>>> SendDirectMessageLink(string userName, string link, string message)
        {
            var inbox = await _apiMessagingProcessor.GetDirectInboxAsync(_paginationParameters)
                .ConfigureAwait(false);
            var thread = inbox.Value.Inbox.Threads
                .FirstOrDefault(u => u.Users.FirstOrDefault().UserName.ToLower() == userName.ToLower());

            var result = await _apiMessagingProcessor.SendDirectLinkAsync(message, link, thread?.ThreadId)
                .ConfigureAwait(false);

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
