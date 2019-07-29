using System;
using System.Text;

namespace System
{
    public class StringHelper
    {
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
    }
}
