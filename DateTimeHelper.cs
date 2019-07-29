using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public class DateTimeHelper
    {
        public static long GetTicks(DateTime dateTime)
        {
            DateTime startTime = TimeZoneInfo.ConvertTimeToUtc(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (dateTime.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位
            return t;
        }
        public static long GetTicks()
        {
            return GetTicks(DateTime.Now);
        }
        public static DateTime ParseTicks(long ticks)
        {
            DateTime startTime = new DateTime(1970, 1, 1);
             TimeSpan timeStamp = new TimeSpan(ticks);
            return startTime.Add(timeStamp);
        }
        public static DateTime Get(string yyyyMMddHHmmss)
        {
            var isParse = DateTime.TryParseExact(yyyyMMddHHmmss, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AllowInnerWhite, out DateTime res);
            if (isParse)
                return res;
            return DateTime.MinValue;
        }
    }
}
