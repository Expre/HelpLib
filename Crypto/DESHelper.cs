using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HelpLib.Crypto
{
    public class DESHelper
    {
        public static string Decrypt(string base64Data, string key)
        {
            byte[] encryptedBytes = Convert.FromBase64String(base64Data);
            byte[] bytes = Decrypt(encryptedBytes, key);
            if (bytes == null)
            {
                return null;
            }
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>  
        /// 加密
        /// </summary>  
        /// <param name="data">加密数据</param>  
        /// <param name="key">加密key必须为24位</param>  
        /// <returns>加密完成的字节数组</returns>  
        public static byte[] Decrypt(byte[] data, string key)
        {
            byte[] encryptedBytes = data;
            byte[] bKey = new byte[24];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);

            using (MemoryStream Memory = new MemoryStream(encryptedBytes))
            {
                using (TripleDES des = TripleDES.Create())
                {
                    des.Mode = CipherMode.ECB;
                    des.Padding = PaddingMode.PKCS7;
                    des.Key = bKey;
                    using (CryptoStream cryptoStream = new CryptoStream(Memory, des.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        byte[] tmp = new byte[encryptedBytes.Length];
                        int len = cryptoStream.Read(tmp, 0, encryptedBytes.Length);
                        byte[] ret = new byte[len];
                        Array.Copy(tmp, 0, ret, 0, len);
                        return ret;
                    }
                }
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">解密数据</param>
        /// <param name="key">解密key必须是24位</param>
        /// <returns>base64字符串</returns>
        public static string Encrypt(string data, string key)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(data);
            var encryptBytes = Encrypt(plainBytes, key);
            if (encryptBytes == null)
                return null;
            return Convert.ToBase64String(encryptBytes);
        }

        /// <summary>  
        /// 解密
        /// </summary>  
        /// <param name="data">解密数据</param>  
        /// <param name="key">解密key必须是24位</param>  
        /// <returns>解密完成的字节数组</returns>  
        public static byte[] Encrypt(byte[] data, string key)
        {
            using (MemoryStream Memory = new MemoryStream())
            {
                using (TripleDES des = TripleDES.Create())
                {
                    byte[] plainBytes = data;
                    byte[] bKey = new byte[24];
                    Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);

                    des.Mode = CipherMode.ECB;
                    des.Padding = PaddingMode.PKCS7;
                    des.Key = bKey;
                    using (CryptoStream cryptoStream = new CryptoStream(Memory, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        return Memory.ToArray();
                    }
                }
            }
        }
    }
}
