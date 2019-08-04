using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace HelpLib
{
    public static partial class Extentions
    {
        public static long ToTicks(this DateTime dateTime)
        {
            long result = TimeHelper.GetTicks(dateTime);
            return result;
        }
        public static bool IsMin(this DateTime dateTime)
        {
            return !(dateTime != DateTime.MinValue);
        }
    }
}
