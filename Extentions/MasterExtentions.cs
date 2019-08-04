using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// 为了不污染调用，此为主扩展，添加此包就可以使用；如若使用更丰富扩展那么请引用 HelpLib 命名空间
    /// </summary>
    public static partial class MasterExtentions
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
        public static bool IsEmpty<T>(this T scoure) where T : struct
        {
            return DataFormater.IsEmpty(scoure);
        }
        public static TOut MapperTo<TIn, TOut>(this TIn obj)
        {
            TOut res = ExpressionGenericMapper<TIn, TOut>.MapperTo(obj);
            return res;
        }
        public static List<TOut> MapperTo<TIn, TOut>(this List<TIn> objs)
        {
            List<TOut> outs = ExpressionGenericMapper<TIn, TOut>.MapperTo(objs);
            return outs;
        }
    }
}
