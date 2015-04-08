/*
 * TfsNotificationRelay - http://github.com/kria/TfsNotificationRelay
 * 
 * Copyright (C) 2014 Kristian Adrup
 * 
 * This file is part of TfsNotificationRelay.
 * 
 * TfsNotificationRelay is free software: you can redistribute it and/or 
 * modify it under the terms of the GNU General Public License as published 
 * by the Free Software Foundation, either version 3 of the License, or 
 * (at your option) any later version. See included file COPYING for details.
 */

using System.Linq;
using DevCore.TfsNotificationRelay.Notifications;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevCore.TfsNotificationRelay.Configuration;
using Microsoft.TeamFoundation.Framework.Server;

namespace DevCore.TfsNotificationRelay.Slack
{
    public class SlackNotifier : INotifier
    {
        private readonly ISlackClient _slackClient;
        private readonly ISlackMessageFactory _slackMessageFactory;
        private readonly ISlackConfigurationFactory _slackConfigurationFactory;

        public SlackNotifier(ISlackClient slackClient, ISlackMessageFactory slackMessageFactory, ISlackConfigurationFactory slackConfigurationFactory)
        {
            _slackClient = slackClient;
            _slackMessageFactory = slackMessageFactory;
            _slackConfigurationFactory = slackConfigurationFactory;
        }

        public SlackNotifier()
        {
            _slackClient = new SlackClient();
            _slackMessageFactory = new SlackMessageFactory();
            _slackConfigurationFactory = new SlackConfigurationFactory();
        }

        public async Task NotifyAsync(TeamFoundationRequestContext requestContext, INotification notification, BotElement bot)
        {
            var config = _slackConfigurationFactory.GetConfiguration(bot);
            var tasks = new List<Task>();

            var messages = _slackMessageFactory.GetMessages(notification, config);

            tasks.AddRange(messages.Select(message => _slackClient.SendMessageAsync(message, config.WebhookUrl).ContinueWith(t => t.Result.EnsureSuccessStatusCode())));

            await Task.WhenAll(tasks);
        }
    }
}
