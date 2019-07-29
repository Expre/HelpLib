using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace System
{
    public class Crypto
    {
        public static string PasswordSalt => "landic";
        public static string MD5Hash(string input)
        {
            string result = MD5HashExeute((md5) => md5.ComputeHash(Encoding.ASCII.GetBytes(input)));
            return result;
        }
        public static string MD5Hash(Stream inputStream)
        {
            string result = MD5HashExeute((md5) => md5.ComputeHash(inputStream));
            return result;
        }
        /// <summary>
        /// using资源管理、横线替换、大小写转换，这里是统一管理，需要改只改一个地方，无需两个方法都改
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        private static string MD5HashExeute(Func<MD5, byte[]> func)
        {
            using (var md5 = MD5.Create())
            {
                var value = func(md5);
                var result = BitConverter.ToString(value);
                return result.Replace("-", "").ToLower();
            }
        }
        public static string GetPassword(string password)
        {
            var res = MD5Hash(password + PasswordSalt);
            return res;
        }
    }
}
