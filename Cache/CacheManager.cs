using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public class CacheManager
    {
        private static readonly ICache cache = null;

        static CacheManager()
        {
            cache = (ICache)Activator.CreateInstance(typeof(CustomerCache));
        }
        public static List<string> Keys => cache.Keys;
        /// <summary>
        /// 当前缓存数据项的个数
        /// </summary>
        public static int Count => cache.Count;

        /// <summary>
        /// 如果缓存中已存在数据项键值，则返回true
        /// </summary>
        /// <param name="key">数据项键值</param>
        /// <returns>数据项是否存在</returns>
        public static bool Contains(string key) => cache.Contains(key);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key) => cache.Get<T>(key);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存的项</param>
        /// <param name="acquire">没有缓存的时候获取数据的方式</param>
        /// <param name="cacheMinutes">缓存时间，默认30分钟</param>
        /// <returns></returns>
        public static T Get<T>(string key, Func<T> acquire, double cacheMinutes = 30) => cache.Get(key, acquire, cacheMinutes);

        /// <summary>
        /// 添加缓存数据。
        /// 如果另一个相同键值的数据已经存在，原数据项将被删除，新数据项被添加。
        /// </summary>
        /// <param name="key">缓存数据的键值</param>
        /// <param name="value">缓存的数据，可以为null值</param>
        /// <param name="cacheMinutes">缓存时间，默认30分钟</param>
        public static void Add(string key, object value, double cacheMinutes = 30) => cache.Add(key, value, cacheMinutes);

        /// <summary>
        /// 删除缓存数据项
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key) => cache.Remove(key);

        /// <summary>
        /// 删除所有缓存数据项
        /// </summary>
        public static void RemoveAll() => cache.RemoveAll();
    }
}
