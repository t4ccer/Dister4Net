using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Dister4Net.Communication
{
    public class MessagePacket
    {
        public string topic;
        public string content;
        [Newtonsoft.Json.JsonIgnore]
        public string[] SplittedTopic => topic.Split('/');
        public bool MatchTopic(string topicPattern)
        {
            //+ -> Single level wildcard
            //* -> Multi level wildcard
            var splittedPattern = topicPattern.Split('/');

            if (splittedPattern.Length > SplittedTopic.Length)
                return false;

            for (int i = 0; i < SplittedTopic.Length; i++)
            {
                if (i >= splittedPattern.Length)
                    return false;
                else if (splittedPattern[i] == SplittedTopic[i] || splittedPattern[i] == "+")
                    continue;
                else if (splittedPattern[i] == "*")
                    return true;
                else
                    return false;
            }
            return true;
        }
    }
}
