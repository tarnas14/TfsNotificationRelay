namespace TfsNotificationRelay.Tests.SlackNotifierSpecification
{
    using DevCore.TfsNotificationRelay.Slack;
    using NUnit.Framework;

    [TestFixture]
    class SlackNotifierConfigurationFactorySpecification
    {
        private SlackConfiguration LoadTestConfig()
        {
            var factory = new SlackConfigurationFactory();
            var slackBot = TestConfigurationHelper.LoadSlackBot(@"SlackNotifierSpecification\slackNotifierTestConfig.config");
            return factory.GetConfiguration(slackBot);
        }

        [Test]
        public void ShouldRetrieveChannelListFromConfiguration()
        {
            //given
            var expected = new[]
            {
                "#general",
                "#b"
            };

            //when
            var config = LoadTestConfig();

            //then
            Assert.That(config.Channels, Is.EquivalentTo(expected));
        }

        [Test]
        public void ShouldStoreBotElementThatWasUsedToInitializeIt()
        {
            //given
            var slackBot = TestConfigurationHelper.LoadSlackBot(@"SlackNotifierSpecification\slackNotifierTestConfig.config");

            //when
            var config = new SlackConfigurationFactory().GetConfiguration(slackBot);

            //then
            Assert.That(config.Bot, Is.EqualTo(slackBot));
        }

        [Test]
        public void ShouldReadUsername()
        {
            //given
            const string expected = "tfsbot";

            //when
            var config = LoadTestConfig();

            //then
            Assert.That(config.Username, Is.EqualTo(expected));
        }

        [Test]
        public void ShouldReadIconUrl()
        {
            //given
            const string expected = "https://raw.githubusercontent.com/kria/TfsNotificationRelay/master/tfsicon.png";

            //when
            var config = LoadTestConfig();

            //then
            Assert.That(config.IconUrl, Is.EqualTo(expected));
        }

        [Test]
        public void ShouldReadColors()
        {
            //given
            const string expectedStandardColor = "#68217a";
            const string expectedSuccessColor = "#1cb841";
            const string expectedErrorColor = "#ca3c3c";

            //when
            var config = LoadTestConfig();

            //then
            Assert.That(config.StandardColor, Is.EqualTo(expectedStandardColor));
            Assert.That(config.SuccessColor, Is.EqualTo(expectedSuccessColor));
            Assert.That(config.ErrorColor, Is.EqualTo(expectedErrorColor));
        }

        [Test]
        public void ShouldReadWebhookUrl()
        {
            //given
            const string expected = "someTestWebhookUrl";

            //when
            var config = LoadTestConfig();

            //then
            Assert.That(config.WebhookUrl, Is.EqualTo(expected));
        }

        [Test]
        public void ShouldReadIconEmoji()
        {
            //given
            const string expected = "testEmoji";

            //when
            var config = LoadTestConfig();

            //then
            Assert.That(config.IconEmoji, Is.EqualTo(expected));
        }

        [Test]
        public void ShouldReadTextFormats()
        {
            //given

            //when
            var config = LoadTestConfig();

            //then
            var formatting = config.TextFormatting;
            Assert.That(formatting.PushFormat, Is.EqualTo("pushFormat"));
            Assert.That(formatting.Pushed, Is.EqualTo("pushed"));
            Assert.That(formatting.ForcePushed, Is.EqualTo("forcePushed"));
            Assert.That(formatting.Commit, Is.EqualTo("commit"));
            Assert.That(formatting.RefPointer, Is.EqualTo("refPointer"));
            Assert.That(formatting.Deleted, Is.EqualTo("deleted"));
            Assert.That(formatting.CommitFormat, Is.EqualTo("commitFormat"));
            Assert.That(formatting.LinesSupressedFormat, Is.EqualTo("linesSuppressedFormat"));
            Assert.That(formatting.DateTimeFormat, Is.EqualTo("dateTimeFormat"));
            Assert.That(formatting.TimeSpanFormat, Is.EqualTo("timeSpanFormat"));
            Assert.That(formatting.BuildFormat, Is.EqualTo("buildFormat"));
            Assert.That(formatting.BuildQualityChangedFormat, Is.EqualTo("buildQualityChangedFormat"));
            Assert.That(formatting.BuildQualityNotSet, Is.EqualTo("buildQualityNotSet"));
            Assert.That(formatting.ProjectCreatedFormat, Is.EqualTo("projectCreatedFormat"));
            Assert.That(formatting.ProjectDeletedFormat, Is.EqualTo("projectDeletedFormat"));
            Assert.That(formatting.CheckinFormat, Is.EqualTo("checkinFormat"));
            Assert.That(formatting.ProjectLinkFormat, Is.EqualTo("projectLinkFormat"));
            Assert.That(formatting.ChangeCountAddFormat, Is.EqualTo("changeCountAddFormat"));
            Assert.That(formatting.ChangeCountDeleteFormat, Is.EqualTo("changeCountDeleteFormat"));
            Assert.That(formatting.ChangeCountEditFormat, Is.EqualTo("changeCountEditFormat"));
            Assert.That(formatting.ChangeCountRenameFormat, Is.EqualTo("changeCountRenameFormat"));
            Assert.That(formatting.ChangeCountSourceRenameFormat, Is.EqualTo("changeCountSourceRenameFormat"));
            Assert.That(formatting.ChangeCountUnknownFormat, Is.EqualTo("changeCountUnknownFormat"));
            Assert.That(formatting.WorkItemchangedFormat, Is.EqualTo("workItemChangedFormat"));
            Assert.That(formatting.Updated, Is.EqualTo("updated"));
            Assert.That(formatting.Created, Is.EqualTo("created"));
            Assert.That(formatting.State, Is.EqualTo("state"));
            Assert.That(formatting.AssignedTo, Is.EqualTo("assignedTo"));
            Assert.That(formatting.PullRequestCreatedFormat, Is.EqualTo("pullRequestCreatedFormat"));
            Assert.That(formatting.PullRequestStatusUpdateFormat, Is.EqualTo("pullRequestStatusUpdateFormat"));
            Assert.That(formatting.VoteApproved, Is.EqualTo("voteApproved"));
            Assert.That(formatting.VoteRejected, Is.EqualTo("voteRejected"));
            Assert.That(formatting.VoteRescinded, Is.EqualTo("voteRescinded"));
            Assert.That(formatting.Completed, Is.EqualTo("completed"));
            Assert.That(formatting.Abandoned, Is.EqualTo("abandoned"));
            Assert.That(formatting.Reactivated, Is.EqualTo("reactivated"));

        }
    }
}
