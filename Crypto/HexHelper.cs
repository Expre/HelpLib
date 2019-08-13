using System;
using System.Collections.Generic;
using System.Text;

namespace HelpLib.Crypto
{
    public class HexHelper
    {
        public static byte[] HexStringToBytes(string hexValues)
        {
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
        public static string AddBetweenHex(string hexX, string hexY)
        {
            int intSum = Convert.ToInt32(hexX, 16) + Convert.ToInt32(hexY, 16);
            return Convert.ToString(intSum, 16).ToUpper();
        }
        public static string MultBetweenHex(string hexX, string hexY)
        {
            int intMult = Convert.ToInt32(hexX, 16) * Convert.ToInt32(hexY, 16);
            return Convert.ToString(intMult, 16).ToUpper();
        }
        public static string GetHex(string str)
        {
            char[] values = str.ToCharArray();
            StringBuilder result = new StringBuilder();
            foreach (char letter in values)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(letter);
                // Convert the integer value to a hexadecimal value in string form.
                result.Append($"{value:X}");
            }
            return result.ToString();
        }
    }
}
