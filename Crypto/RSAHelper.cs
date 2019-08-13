using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace HelpLib.Crypto
{
    public class RSAHelper
    {
        //1. CreatKeyRSA创建密钥
        //2. EncryptRSA加密
        //3. DecryptRSA解密
        //4. Hash原数据哈希值
        //5. SignatureFormatterRSA数字签名
        //6. SignatureDeformatterRSA签名验证

        public static string Encrypt(string input)
        {
            UTF8Encoding byteConverter = new UTF8Encoding();
            byte[] dataToEncrypt = byteConverter.GetBytes(input);
            byte[] encryptedData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                encryptedData = RSAEncrypt(dataToEncrypt, RSA.ExportParameters(false), false);
                string result = byteConverter.GetString(encryptedData);
                return result;
            }
        }

        public static string Decrypt(string input)
        {
            UTF8Encoding byteConverter = new UTF8Encoding();
            byte[] dataToDecrypt = byteConverter.GetBytes(input);
            byte[] decryptedData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                decryptedData = RSADecrypt(dataToDecrypt, RSA.ExportParameters(true), false);
                string result = byteConverter.GetString(decryptedData);
                return result;
            }
        }

        public static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            byte[] encryptedData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.ImportParameters(RSAKeyInfo);
                encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
            }
            return encryptedData;
        }

        public static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            byte[] decryptedData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.ImportParameters(RSAKeyInfo);
                decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
            }
            return decryptedData;
        }

    }
    public class RSASecret
    {
        public string Public { get; set; }
        public string Private { get; set; }
    }
}
