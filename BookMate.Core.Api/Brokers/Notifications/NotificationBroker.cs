// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using BookMate.Core.Api.Models.Configurations;
using Microsoft.Extensions.Configuration;
using Twilio;

namespace BookMate.Core.Api.Brokers.Notifications
{
    public partial class NotificationBroker : INotificationBroker
    {
        public NotificationBroker(IConfiguration configuration)
        {
            InitializeTwilioClient(configuration);
        }

        private static void InitializeTwilioClient(IConfiguration configuration)
        {
            LocalConfigurations localConfigurations = configuration.Get<LocalConfigurations>();
            string twilioAccountSid = localConfigurations.TwilioConfiguration.AccountSid;
            string twilioAuthToken = localConfigurations.TwilioConfiguration.AuthToken;
            TwilioClient.Init(twilioAccountSid, twilioAuthToken);
        }
    }
}
