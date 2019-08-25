using System;
using System.Collections.Generic;
using System.Text;

namespace HelpLib.Crypto
{
    public class HexHelper
    {
        public static byte[] HexStringToBytes(string hexValues)
        {
            hexValues = hexValues.Replace("-", string.Empty).Replace(" ", string.Empty);
            byte[] buff = new byte[hexValues.Length / 2];
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i] = Convert.ToByte(hexValues.Substring(i * 2, 2), 16);
            }
            return buff;
        }
        public static string BytesToHexString(byte[] input, bool isSpace = false)
        {
            StringBuilder str = new StringBuilder(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                str.Append(input[i].ToString("X2"));
                if (isSpace)
                    str.Append(" ");
            }
            return str.ToString();
        }
        public static string Add(string hexX, string hexY)
        {
            int intSum = Convert.ToInt32(hexX, 16) + Convert.ToInt32(hexY, 16);
            return Convert.ToString(intSum, 16).ToUpper();
        }
        public static string Mult(string hexX, string hexY)
        {
            int intMult = Convert.ToInt32(hexX, 16) * Convert.ToInt32(hexY, 16);
            return Convert.ToString(intMult, 16).ToUpper();
        }
        /// <summary>
        /// 高低位数据转换
        /// </summary>
        /// <param name="bigEndianOrLittleEndian">高位或低位在前数据</param>
        /// <returns></returns>
        public static string GetReverse(string bigEndianOrLittleEndian)
        {
            byte[] bytes = HexStringToBytes(bigEndianOrLittleEndian);
            Array.Reverse(bytes);
            string hexValues = BytesToHexString(bytes);
            return hexValues;
        }
    }
}
