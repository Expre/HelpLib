using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public class Hex
    {
        //https://docs.microsoft.com/zh-cn/dotnet/csharp/programming-guide/types/how-to-convert-between-hexadecimal-strings-and-numeric-types
        public static byte[] HexStringToBytes(string hexValues)
        {
            //string[] hexValuesSplit = hexValues.Split(' ');
            byte[] buff = new byte[hexValues.Length / 2];
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i] = Convert.ToByte(hexValues.Substring(i * 2, 2), 16);
            }
            return buff;
        }
        public static string BytesToHexString(byte[] input, bool isSpace = false)
        {
            int i;
            StringBuilder str = new StringBuilder(input.Length);
            for (i = 0; i < input.Length; i++)
            {
                str.Append(input[i].ToString("X2"));
                if (isSpace)
                    str.Append(" ");
            }
            return str.ToString();
        }
        public static string AddBetweenHex(string x, string y)
        {
            int intSum = Convert.ToInt32(x, 16) + Convert.ToInt32(y, 16);
            return Convert.ToString(intSum, 16);
        }
        public static string StringToHex(string str)
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
