using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace System
{
    public class DataFormater
    {
        private static readonly Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");

        #region 基础值类型检查
        public static bool IsDateTime(string dateTimeString)
        {
            return DateTime.TryParse(dateTimeString, out _);
        }
        public static bool IsBoolean(object booleanString)
        {
            return bool.TryParse(booleanString.ToString(), out _);
        }
        public static bool IsEmpty<T>(T scoure) where T : struct
        {
            return scoure.Equals(default(T));
        }
        public static bool IsByte(string byteString)
        {
            return byte.TryParse(byteString, out _);
        }
        public static bool IsChar(string charString)
        {
            return char.TryParse(charString, out _);
        }
        public static bool IsDecimal(string decimalString)
        {
            return decimal.TryParse(decimalString, out _);
        }
        public static bool IsDouble(string doubleString)
        {
            return double.TryParse(doubleString, out _);
        }
        public static bool IsEnum<TEnum>(string enumString) where TEnum : struct
        {
            return IsEnum<TEnum>(enumString, out _);
        }
        public static bool IsEnum<TEnum>(string enumString, out TEnum e) where TEnum : struct
        {
            if (!Enum.TryParse(enumString, out e))
                return false;
            if (!Enum.IsDefined(typeof(TEnum), e))
                return false;
            return true;
        }
        public static bool IsFloat(string floatString)
        {
            return float.TryParse(floatString, out _);
        }
        public static bool IsInt(string intString)
        {
            return int.TryParse(intString, out _);
        }
        public static bool IsLong(string longString)
        {
            return long.TryParse(longString, out _);
        }
        public static bool IsSByte(string sbyteString)
        {
            return sbyte.TryParse(sbyteString, out _);
        }
        public static bool IsShort(string shortString)
        {
            return short.TryParse(shortString, out _);
        }
        #endregion
        #region 引用类型检查
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
        #endregion
        #region 常用数据检查
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
        public static string GetIDNO(object val)
        {
            if (val == null || val == DBNull.Value)
            {
                return "";
            }
            if (!IsIDNO(val.ToString()))
            {
                return "";
            }
            return val.ToString().ToUpper();
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
            int sexint = 0;
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
                sexint = int.Parse(val.Substring(13, 1));
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
                int nTemp = 0;
                for (int i = 0; i < 17; i++)
                {
                    nTemp += int.Parse(val.Substring(i, 1)) * arrInt[i];
                }
                string valnum = arrCh[nTemp % 11];
                if (valnum != val.Substring(17, 1))
                {
                    return false;
                }
                sexint = int.Parse(val.Substring(16, 1));
            }
            sex = sexint % 2 == 0 ? "女" : "男";
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
        public static bool IsBase64String(string str)
        {
            return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");
        }
        #endregion
        #region 不同类型的值获取
        public static DateTime GetDateTime(string dateTime)
        {
            if (string.IsNullOrEmpty(dateTime))
                return DateTime.MinValue;
            if (IsDateTime(dateTime))
                return DateTime.Parse(dateTime);
            return DateTime.MinValue;
        }
        public static Guid GetGuid(object val)
        {
            if (val == null || val == DBNull.Value)
                return Guid.Empty;
            if (!Guid.TryParse(val.ToString(), out Guid gid))
                return Guid.Empty;
            else
                return gid;
        }
        public static int GetInt(object val, int defaultValue)
        {
            if (val.IsNull() || !IsInt(val.ToString()))
                return defaultValue;
            return int.Parse(val.ToString());
        }
        public static int GetInt(object val)
        {
            return GetInt(val, 0);
        }
        public static int GetInt(object val, int min, int max)
        {
            int result = GetInt(val, 0);
            if (result < min)
                return min;
            if (result > max)
                return max;
            return result;
        }

        public static long GetLong(object val, int defaultValue)
        {
            if (val.IsNull() || !IsLong(val.ToString()))
                return defaultValue;
            return long.Parse(val.ToString());
        }
        public static long GetLong(object val)
        {
            return GetLong(val, 0);
        }

        public static bool GetBoolean(object val)
        {
            if (val.IsNull())
                return false;
            if (!IsBoolean(val.ToString()))
                return false;
            if (IsBoolean(val))
                return bool.Parse(val.ToString());
            return false;
        }

        #endregion
        #region 字符串操作
        public static bool CheckString(string val, int min, int max)
        {
            val = val.Trim();
            if (val.Length < min || val.Length > max)
                return false;
            else
                return true;
        }
        public static string GetSafeString(object val)
        {
            return GetSafeString(val, "");
        }
        public static string GetSafeString(object val, string defaultval)
        {
            if (val == null || val == DBNull.Value)
            {
                return defaultval;
            }
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
        #region 字符串脱敏
        public static string Desensitized(string scoureStr, DesensitizedType desensitizedType)
        {
            string finallyDesensitized;
            switch (desensitizedType)
            {
                case DesensitizedType.未知:
                    finallyDesensitized = scoureStr;
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
            未知 = 0,
            手机号 = 1,
            银行卡号 = 2,
        }
        #endregion
        #region 流转字节数组、base64字符串
        public static byte[] ToBytes(Stream scoureStream)
        {
            if (!scoureStream.CanRead)
                return null;
            scoureStream.Seek(0, SeekOrigin.Begin);
            var buffer = new byte[scoureStream.Length];
            scoureStream.Read(buffer, 0, buffer.Length);
            return buffer;
        }
        public static string ToBase64(Stream scoureStream)
        {
            string base64 = Convert.ToBase64String(ToBytes(scoureStream));
            return base64;
        }
        #endregion
        #region 对私有字段和属性值的设置及获取、私有方法的执行
        //1、得到私有字段的值
        public static T GetPrivateField<T>(object instance, string fieldname)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            FieldInfo field = type.GetField(fieldname, flag);
            return (T)field.GetValue(instance);
        }

        //2、得到私有属性的值：
        public static T GetPrivateProperty<T>(object instance, string propertyname)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            PropertyInfo field = type.GetProperty(propertyname, flag);
            return (T)field.GetValue(instance, null);
        }

        //3、设置私有字段的值：
        public static void SetPrivateField(object instance, string fieldname, object value)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            FieldInfo field = type.GetField(fieldname, flag);
            field.SetValue(instance, value);
        }

        //4、设置私有属性的值： 
        public static void SetPrivateProperty(object instance, string propertyname, object value)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            PropertyInfo field = type.GetProperty(propertyname, flag);
            field.SetValue(instance, value, null);
        }

        //5、调用私有方法：
        public static T InvokePrivateMethod<T>(object instance, string name, params object[] param)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            MethodInfo method = type.GetMethod(name, flag);
            return (T)method.Invoke(instance, param);
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
