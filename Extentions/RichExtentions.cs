using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelpLib
{
    public static partial class Extentions
    {
        public static bool IsNotNull<TSource>(this TSource source)
        {
            return !DataFormater.IsNull(source);
        }
        public static bool IsMobile(this string mobile)
        {
            return DataFormater.IsMobile(mobile);
        }
    }
}
