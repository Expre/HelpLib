using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System
{
    public class CustomerCache : ICache
    {
        private static readonly ConcurrentDictionary<string, KeyValuePair<DateTime, object>> CustomerCacheDictionary = new ConcurrentDictionary<string, KeyValuePair<DateTime, object>>();
        static CustomerCache()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (CustomerCacheDictionary.Count < 1)
                        continue;
                    foreach (var item in CustomerCacheDictionary)
                    {
                        if (DateTime.Now < item.Value.Key)//没过期
                            continue;
                        //过期key
                        CustomerCacheDictionary.TryRemove(item.Key, out _);
                    }
                    Thread.Sleep(1000 * 60);
                }
            });
        }

        public object this[string key]
        {
            get => Get<object>(key);
            set { Add(key, value); }
        }
        public int Count => CustomerCacheDictionary.Count;
        public List<string> Keys => CustomerCacheDictionary.Keys.ToList();
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheMinutes">缓存时间，默认30分钟</param>
        public void Add(string key, object data, double cacheMinutes = 30)
        {
            if (CustomerCacheDictionary.ContainsKey(key))
                CustomerCacheDictionary.TryRemove(key, out _);
            CustomerCacheDictionary.TryAdd(key, new KeyValuePair<DateTime, object>(DateTime.Now.AddMinutes(cacheMinutes), data));
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            if (!CustomerCacheDictionary.ContainsKey(key))
                return default;
            var tryResult = CustomerCacheDictionary.TryGetValue(key, out KeyValuePair<DateTime, object> keyValuePair);
            if (!tryResult)
                return default;
            if (DateTime.Now > keyValuePair.Key)
                return default;
            if (keyValuePair.Value is T)
                return (T)keyValuePair.Value;
            CustomerCacheDictionary.TryRemove(key, out _);
            return default;
        }
        public T Get<T>(string key, Func<T> acquire, double cacheMinutes = 30)
        {
            if (Contains(key))
                return Get<T>(key);
            T result = acquire.Invoke();
            Add(key, result, cacheMinutes);
            return result;
        }
        /// <summary>
        /// 是否具有此key的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            if (!CustomerCacheDictionary.ContainsKey(key))
                return false;
            KeyValuePair<DateTime, object> keyValuePair = CustomerCacheDictionary[key];
            if (DateTime.Now < keyValuePair.Key)
                return true;
            CustomerCacheDictionary.TryRemove(key, out _);
            return false;
        }


        public void Remove(string key)
        {
            if (CustomerCacheDictionary.ContainsKey(key))
                CustomerCacheDictionary.TryRemove(key, out _);
        }

        public void RemoveAll()
        {
            CustomerCacheDictionary.Clear();
            //List<string> keys = new List<string>();
            //foreach (var item in _CustomerCacheDictionary)
            //{
            //    keys.Add(item.Key);
            //}
            //foreach (var item in keys)
            //{
            //    _CustomerCacheDictionary.TryRemove(item, out _);
            //}
        }
    }
}
