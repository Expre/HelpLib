using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace HelpLib.Crypto
{
    public class SHAHelper
    {
        public static string Sha1(string input)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] bytes_sha1_in = Encoding.UTF8.GetBytes(input);
                byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
                string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
                str_sha1_out = str_sha1_out.Replace("-", "");
                return str_sha1_out;
            }
        }
        public static string Sha256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes_sha256_in = Encoding.UTF8.GetBytes(input);
                byte[] bytes_sha256_out = sha256.ComputeHash(bytes_sha256_in);
                string str_sha256_out = BitConverter.ToString(bytes_sha256_out);
                str_sha256_out = str_sha256_out.Replace("-", "");
                return str_sha256_out;
            }
        }
        public static string Sha384(string input)
        {
            using (SHA384 sha384 = SHA384.Create())
            {
                byte[] bytes_sha384_in = Encoding.UTF8.GetBytes(input);
                byte[] bytes_sha384_out = sha384.ComputeHash(bytes_sha384_in);
                string str_sha384_out = BitConverter.ToString(bytes_sha384_out);
                str_sha384_out = str_sha384_out.Replace("-", "");
                return str_sha384_out;
            }
        }
        public static string Sha512(string input)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] bytes_sha512_in = Encoding.UTF8.GetBytes(input);
                byte[] bytes_sha512_out = sha512.ComputeHash(bytes_sha512_in);
                string str_sha512_out = BitConverter.ToString(bytes_sha512_out);
                str_sha512_out = str_sha512_out.Replace("-", "");
                return str_sha512_out;
            }
        }

        public static string HMACSHA1(string input, string key)
        {
            byte[] secrectKey = Encoding.UTF8.GetBytes(key);
            using (HMACSHA1 hmac = new HMACSHA1(secrectKey))
            {
                hmac.Initialize();
                byte[] bytes_hmac_in = Encoding.UTF8.GetBytes(input);
                byte[] bytes_hamc_out = hmac.ComputeHash(bytes_hmac_in);
                string str_hamc_out = BitConverter.ToString(bytes_hamc_out);
                str_hamc_out = str_hamc_out.Replace("-", "");
                return str_hamc_out;
            }
        }
        public static string HMACSHA256(string input, string key)
        {
            byte[] secrectKey = Encoding.UTF8.GetBytes(key);
            using (HMACSHA256 hmac = new HMACSHA256(secrectKey))
            {
                hmac.Initialize();
                byte[] bytes_hmac_in = Encoding.UTF8.GetBytes(input);
                byte[] bytes_hamc_out = hmac.ComputeHash(bytes_hmac_in);
                string str_hamc_out = BitConverter.ToString(bytes_hamc_out);
                str_hamc_out = str_hamc_out.Replace("-", "");
                return str_hamc_out;
            }
        }
        public static string HMACSHA384(string input, string key)
        {
            byte[] secrectKey = Encoding.UTF8.GetBytes(key);
            using (HMACSHA384 hmac = new HMACSHA384(secrectKey))
            {
                hmac.Initialize();
                byte[] bytes_hmac_in = Encoding.UTF8.GetBytes(input);
                byte[] bytes_hamc_out = hmac.ComputeHash(bytes_hmac_in);
                string str_hamc_out = BitConverter.ToString(bytes_hamc_out);
                str_hamc_out = str_hamc_out.Replace("-", "");
                return str_hamc_out;
            }
        }
        public static string HMACSHA512(string input, string key)
        {
            byte[] secrectKey = Encoding.UTF8.GetBytes(key);
            using (HMACSHA512 hmac = new HMACSHA512(secrectKey))
            {
                hmac.Initialize();
                byte[] bytes_hmac_in = Encoding.UTF8.GetBytes(input);
                byte[] bytes_hamc_out = hmac.ComputeHash(bytes_hmac_in);
                string str_hamc_out = BitConverter.ToString(bytes_hamc_out);
                str_hamc_out = str_hamc_out.Replace("-", "");
                return str_hamc_out;
            }
        }
    }
}
