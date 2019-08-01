using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static partial class Extentions
    {
        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return DataFormater.IsNullOrEmpty(source);
        }
        public static bool IsEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return DataFormater.IsNullOrEmpty(source);
        }
        public static bool IsNull<TSource>(this TSource source)
        {
            return DataFormater.IsNull(source);
        }
        public static bool IsNotNull<TSource>(this TSource source)
        {
            return !IsNull(source);
        }
        public static bool IsMobile(this string mobile)
        {
            return DataFormater.IsMobile(mobile);
        }
        public static bool IsEmpty<T>(this T scoure) where T : struct
        {
            return DataFormater.IsEmpty(scoure);
        }
    }
}
