using System;

namespace Dister4Net.Serialization
{
    public interface ISerializer
    {
        public string Serialize(object obj);
        public T Deserialize<T>(string str);
        public object Deserialize(string str, Type type);
    }
}
