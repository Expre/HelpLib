using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HelpLib.Crypto
{
    public class MD5Helper
    {
        public static string PasswordSalt => "landic";
        public static string GetHash(string input)
        {
            string result = ComputeHash((md5) => md5.ComputeHash(Encoding.ASCII.GetBytes(input)));
            return result;
        }
        public static string GetHash(Stream input)
        {
            string result = ComputeHash((md5) => md5.ComputeHash(input));
            return result;
        }
        /// <summary>
        /// using资源管理、横线替换、大小写转换，这里是统一管理，需要改只改一个地方，无需两个方法都改
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        private static string ComputeHash(Func<MD5, byte[]> func)
        {
            using (var md5 = MD5.Create())
            {
                byte[] value = func(md5);
                string result = BitConverter.ToString(value);
                return result.Replace("-", "").ToLower();
            }
        }

        public static byte[] GetHashBytes(string input)
        {
            byte[] result = ComputeHashBytes((md5) => md5.ComputeHash(Encoding.ASCII.GetBytes(input)));
            return result;
        }
        public static byte[] GetHashBytes(Stream input)
        {
            byte[] result = ComputeHashBytes((md5) => md5.ComputeHash(input));
            return result;
        }
        private static byte[] ComputeHashBytes(Func<MD5, byte[]> func)
        {
            using (var md5 = MD5.Create())
            {
                byte[] value = func(md5);
                return value;
            }
        }
        public static string GetPassword(string password, string passwordSalt = "")
        {
            if (string.IsNullOrEmpty(passwordSalt))
                passwordSalt = PasswordSalt;
            string res = GetHash(password + passwordSalt);
            return res;
        }
    }
}
