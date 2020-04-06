﻿using InstagramApiSharp.API;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UpSocial.UpGram.Core
{
    public class Message
    {
        static IInstaApi _apiConnector;

        public Message(IInstaApi apiConnector)
        {
            _apiConnector = apiConnector;
        }

        public async Task<IResult<InstaDirectInboxThreadList>> DirectMessage(string userName, string message)
        {
            var user = await _apiConnector.UserProcessor.GetUserAsync(userName);

            var userPk = (user?.Succeeded ?? false) ? 
                user?.Value?.Pk.ToString() ?? string.Empty : 
                string.Empty; 

            return await SenderDirectMessage(userPk, message);
        }

        public async Task<IResult<InstaDirectInboxThreadList>> DirectMessage(string[] usersName, string message)
        {
            IResult<InstaUser> user;
            var recipients = string.Empty;

            foreach (var userName in usersName)
            {
                user = await _apiConnector.UserProcessor.GetUserAsync(userName);

                recipients = (user?.Succeeded ?? false) ? 
                    string.Concat(recipients, user?.Value?.Pk.ToString() ?? string.Empty, ",") : 
                    recipients;
            }

            return await SenderDirectMessage(recipients, message);
        }

        private async Task<IResult<InstaDirectInboxThreadList>> SenderDirectMessage(string recipients, string message)
        {
            return await _apiConnector.MessagingProcessor.SendDirectTextAsync(recipients, null, message);
        }
    }
}