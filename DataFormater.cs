using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HelpLib;
namespace System
{
    public class DataFormater
    {
        private static readonly Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");
        public static bool CanConvert<DestinationT, SourceT>(SourceT input, out DestinationT destinationObj)
        {
            destinationObj = default;
            Type sourceType = input.GetType();
            Type destinationType = typeof(DestinationT);
            var converter = TypeDescriptor.GetConverter(destinationType);
            if (converter == null)
                return false;
            if (!converter.CanConvertFrom(sourceType))
                return false;
            destinationObj = (DestinationT)converter.ConvertFrom(input);
            return true;
        }
        public static DestinationT Convert<DestinationT, SourceT>(SourceT input)
        {
            if (!CanConvert(input, out DestinationT destinationT))
                return default;
            return destinationT;
        }
        public static bool CanConvert<T>(string input, out T obj)
        {
            obj = default;
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter == null)
                return false;
            if (!converter.CanConvertFrom(typeof(string)))
                return false;
            obj = (T)converter.ConvertFromString(input);
            return true;
        }
        public static T Convert<T>(string input)
        {
            if (!CanConvert<T>(input, out T obj))
                return default;
            return obj;
        }
        public static bool IsNullOrEmpty<TSource>(IEnumerable<TSource> source)
        {
            if (source != null && source.Count() > 0)
                return false;
            return true;
        }
        public static bool IsNull<TSource>(TSource source)
        {
            if (source != null && !source.Equals(default(TSource)))
                return false;
            return true;
        }
        public static bool IsEmpty<TSource>(TSource source) where TSource : struct
        {
            bool result = source.Equals(default(TSource));
            return result;
        }

        public static bool IsMobile(string mobile)
        {
            //string patt = @"^13[0-9]{9}|15[012356789][0-9]{8}|18[0-9][0-9]{8}|14[57][0-9]{8}$|^17[0135678][0-9]{8}$";
            //return Regex.IsMatch(mobile, patt);
            if (mobile.IsNullOrEmpty())
                return false;
            if (mobile.Length != 11)
                return false;
            if (!mobile.StartsWith('1'))
                return false;
            return true;
        }
        public static bool IsIDNO(string val, out DateTime birthday, out string sex)
        {
            birthday = DateTime.MinValue;
            sex = "";
            string pattern = @"^(\d{15})|(\d{17}[0-9|X])$";
            val = val.ToUpper().Trim();
            Regex re = new Regex(pattern);
            Match match;
            string datestr;
            bool convert;
            int sexInt = 0;
            if (!re.IsMatch(val))
            {
                return false;
            }

            if (val.Length == 15)
            {
                pattern = @"\d{6}(\d{2})(\d{2})(\d{2})\d{3}";
                re = new Regex(pattern);
                match = re.Match(val);
                datestr = string.Format("19{0}-{1}-{2}", match.Groups[1].Value, match.Groups[2], match.Groups[3]);
                convert = DateTime.TryParse(datestr, out birthday);
                if (!convert)
                    return false;
                sexInt = int.Parse(val.Substring(13, 1));
            }

            if (val.Length == 18)
            {
                pattern = @"\d{6}(\d{4})(\d{2})(\d{2})\d{3}([0-9|X])";
                re = new Regex(pattern);
                match = re.Match(val);
                datestr = string.Format("{0}-{1}-{2}", match.Groups[1].Value, match.Groups[2], match.Groups[3]);
                convert = DateTime.TryParse(datestr, out birthday);
                if (!convert)
                    return false;
                int[] arrInt = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
                string[] arrCh = new string[] { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };
                int index = 0;
                for (int i = 0; i < 17; i++)
                {
                    index += int.Parse(val.Substring(i, 1)) * arrInt[i];
                }
                string valnum = arrCh[index % 11];
                if (valnum != val.Substring(17, 1))
                {
                    return false;
                }
                sexInt = int.Parse(val.Substring(16, 1));
            }
            sex = sexInt % 2 == 0 ? "女" : "男";
            return true;
        }
        public static bool IsIDNO(string val)
        {
            var res = IsIDNO(val, out _, out _);
            return res;
        }

        /// <summary>
        /// 检测用户名格式是否有效,长度4-20个字符,只能是汉字、字母、下划线、数字
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool IsUserName(string userName)
        {
            if (userName.IsNullOrEmpty())
                return false;
            int userNameLength = userName.Length;
            if (userNameLength >= 4 && userNameLength <= 20 && Regex.IsMatch(userName, @"^([\u4e00-\u9fa5A-Za-z_0-9]{0,})$"))
                return true;
            return false;
        }

        /// <summary>
        /// 密码有效性
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool IsPassword(string password)
        {
            return Regex.IsMatch(password, @"^[A-Za-z_0-9]{6,16}$");
        }
        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsHasCHZN(string inputData)
        {
            Match m = RegCHZN.Match(inputData);
            return m.Success;
        }
        /// <summary> 
        /// 检测含有中文字符串的实际长度 
        /// </summary> 
        /// <param name="str">字符串</param> 
        public static int GetCHZNLength(string inputData)
        {
            ASCIIEncoding n = new ASCIIEncoding();
            byte[] bytes = n.GetBytes(inputData);

            int length = 0; // l 为字符串之实际长度 
            for (int i = 0; i <= bytes.Length - 1; i++)
            {
                if (bytes[i] == 63) //判断是否为汉字或全脚符号 
                {
                    length++;
                }
                length++;
            }
            return length;

        }
        public static bool IsURL(string url)
        {
            return Regex.IsMatch(url, @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&%\$#\=~])*[^\.\,\)\(\s]$");
        }
        #region 字符串操作
        public static string GetSafeString(string val, string defaultval = "")
        {
            if (val.IsNullOrEmpty())
                return defaultval;
            string result = val.ToString().Trim().Replace("'", "''");
            string word = @"exec|insert|select|delete|update|master|truncate|char|declare|join|iframe|href|script|<|>|request";
            if (Regex.IsMatch(result, word))
            {
                Regex reg = new Regex(word, RegexOptions.IgnoreCase);
                result = reg.Replace(result, "");
            }
            return result;
        }
        #endregion

        /// <summary>
        /// 转全角(SBC case)
        /// </summary>
        /// <param name="input">要转换的字符串</param>
        /// <returns></returns>
        public static string ToSBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        ///  转半角(DBC case)
        /// </summary>
        /// <param name="input">要转换的字符串</param>
        /// <returns></returns>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
        /// <summary>
        /// 获取html中的文字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string HtmlToText(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            string result = new Text.RegularExpressions.Regex("</?.+?>").Replace(input, "");
            return result;
        }
        public static string GetLatters()
        {
            StringBuilder latters = new StringBuilder();
            for (char i = 'a'; i <= 'z'; i++)
            {
                latters.Append(i);
            }
            return latters.ToString();
        }
        #region 字符串脱敏
        public static string Desensitized(string scoureStr, DesensitizedType desensitizedType)
        {
            if (scoureStr.IsNullOrEmpty())
                return string.Empty;
            string finallyDesensitized;
            switch (desensitizedType)
            {
                case DesensitizedType.默认:
                    if (scoureStr.Length > 2)
                        finallyDesensitized = scoureStr.Substring(0, 1) + "****" + scoureStr.Substring(scoureStr.Length - 2, 1);
                    else
                        finallyDesensitized = scoureStr.Substring(0, 1) + "****";
                    break;
                case DesensitizedType.手机号:
                    finallyDesensitized = scoureStr.Substring(0, 3) + "****" + scoureStr.Substring(7, 4);
                    break;
                case DesensitizedType.银行卡号:
                    finallyDesensitized = scoureStr.Substring(0, 6) + "****" + scoureStr.Substring(scoureStr.Length - 4, 3);
                    break;
                default:
                    finallyDesensitized = scoureStr;
                    break;
            }
            return finallyDesensitized;
        }
        public enum DesensitizedType
        {
            默认 = 0,
            手机号 = 1,
            银行卡号 = 2,
        }
        #endregion
        #region 首字母转大小写
        /// <summary>
        /// 首字母小写写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstCharToLower(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            string str = input.First().ToString().ToLower() + input.Substring(1);
            return str;
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstCharToUpper(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            string str = input.First().ToString().ToUpper() + input.Substring(1);
            return str;
        }
        #endregion
    }
}
