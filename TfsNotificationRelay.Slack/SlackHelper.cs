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

namespace DevCore.TfsNotificationRelay.Slack
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class SlackHelper
    {
        public static Message CreateSlackMessage(IEnumerable<string> lines, SlackConfiguration slackConfiguration, string channel, string color) 
        {
            if (lines == null || !lines.Any()) return null;

            string header = lines.First();
            var fields = from line in lines.Skip(1) select new AttachmentField() { Value = line, IsShort = false };

            return CreateSlackMessage(header, fields.ToList(), slackConfiguration, channel, color);
        }

        public static Message CreateSlackMessage(string header, IList<AttachmentField> fields, SlackConfiguration slackConfiguration, string channel, string color)
        {
            if (header == null) return null;

            var message = new Message()
            {
                Channel = channel,
                Username = slackConfiguration.Username,
                Attachments = new[] { 
                    new Attachment() {
                        Fallback = header,
                        Pretext = header,
                        Color = color,
                        Fields = fields
                    }
                }
            };
            if (!String.IsNullOrEmpty(slackConfiguration.IconUrl))
                message.IconUrl = slackConfiguration.IconUrl;
            else if (!String.IsNullOrEmpty(slackConfiguration.IconEmoji))
                message.IconEmoji = slackConfiguration.IconEmoji;

            return message;
        }
    }
}
