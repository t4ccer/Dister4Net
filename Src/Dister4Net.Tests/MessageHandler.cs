using System;
using Dister4Net.Communication;
using Xunit;

namespace Dister4Net.Tests
{
    public class MessageHandler
    {
        [Theory]
        [InlineData("foo/bar", "foo/bar", true)]
        [InlineData("foo/bar", "foo/*", true)]
        [InlineData("foo/bar/baz", "foo/+/baz", true)]
        [InlineData("foo/bar/baz", "foo/*", true)]
        [InlineData("foo/bar/baz", "bar/*", false)]
        [InlineData("foo/bar/baz", "foo/bar", false)]
        [InlineData("foo", "foo/bar", false)]
        public void MatchTopic(string topic, string pattern, bool isMatch)
        {
            var packet = new MessagePacket { topic = topic };
            Assert.Equal(isMatch, packet.MatchTopic(pattern));
        }
    }
}
