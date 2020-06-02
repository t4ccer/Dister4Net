using System.Text;

namespace Dister4Net.Communication
{
    public class TopicBuilder
    {
        private readonly StringBuilder result = new StringBuilder("");
        public TopicBuilder One()
        {
            result.Append("+/");
            return this;
        }
        public TopicBuilder Any()
        {
            result.Append("*/");
            return this;
        }
        public TopicBuilder Excatly(string s)
        {
            result.Append(s);
            result.Append("/");
            return this;
        }
        public string Build()
            => result.ToString();
        public static explicit operator string(TopicBuilder topicBuilder)
            => topicBuilder.result.ToString();
    }
}
