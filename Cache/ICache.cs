using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public interface ICache
    {
        List<string> Keys { get; }
        T Get<T>(string key);

        void Add(string key, object data, double cacheMinutes = 30);

        T Get<T>(string key, Func<T> acquire, double cacheMinutes = 30);
        bool Contains(string key);

        void Remove(string key);

        void RemoveAll();

        object this[string key] { get; set; }

        int Count { get; }
    }
}
