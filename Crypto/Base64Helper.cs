using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HelpLib.Crypto
{
    public class Base64Helper
    {
        public static string Encrypt(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            string result = Convert.ToBase64String(bytes);
            return result;
        }
        public static string Decrypt(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            string result = Encoding.UTF8.GetString(bytes);
            return result;
        }
        public static string Encrypt(Stream scoureStream)
        {
            string base64 = Convert.ToBase64String(StreamHelper.GetBytes(scoureStream));
            return base64;
        }
    }
}
