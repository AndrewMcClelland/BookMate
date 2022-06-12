// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System;
using System.Collections.Generic;
using Twilio.Rest.Api.V2010.Account;

namespace BookMate.Core.Api.Brokers.Notifications
{
    public partial class NotificationBroker
    {
        public MessageResource SendSms(string smsBody, string fromNumber, string toNumber, List<Uri> mediaUrlList = null)
        {
            return MessageResource.Create(
                body: smsBody,
                from: fromNumber,
                to: toNumber,
                mediaUrl: mediaUrlList);
        }

        public List<MessageResource> SendSmsToMultipleRecipients(string smsBody, string fromNumber, List<string> toNumbers, List<Uri> mediaUrlList = null)
        {
            var messages = new List<MessageResource>();
            MessageResource message;
            
            foreach(string toNumber in toNumbers)
            {
                message = this.SendSms(
                    smsBody,
                    fromNumber,
                    toNumber,
                    mediaUrlList);

                messages.Add(message);
            }
            
            return messages;
        }
    }
}
