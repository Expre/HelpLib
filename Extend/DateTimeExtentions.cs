using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace System
{
    public static partial class Extentions
    {
        public static long ToTicks(this DateTime dateTime)
        {
            long result = DateTimeHelper.GetTicks(dateTime);
            return result;
        }
        public static bool IsMin(this DateTime dateTime)
        {
            return !(dateTime != DateTime.MinValue);
        }
    }
}
