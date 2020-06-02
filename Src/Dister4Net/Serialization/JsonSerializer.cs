using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Dister4Net.Serialization
{
    public class JsonSerializer : ISerializer
    {
        public T Deserialize<T>(string str) => JsonConvert.DeserializeObject<T>(str);
        public object Deserialize(string str, Type type)
            => JsonConvert.DeserializeObject(str, type);
        public string Serialize(object obj) => JsonConvert.SerializeObject(obj);
    }
}
