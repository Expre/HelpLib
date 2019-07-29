using System;
using System.Text;

namespace Assist
{
    class Program
    {
        static void Main(string[] args)
        {
            //string sum = AddBetweenHex("E1", "08");
            //E9 +01
            //sum = AddBetweenHex(sum, "01");
            string input = "Hello World!";
            StringBuilder sbHexString = new StringBuilder();
            char[] values = input.ToCharArray();
            foreach (char letter in values)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(letter);
                // Convert the integer value to a hexadecimal value in string form.
                Console.WriteLine($"字符 {letter} 16进制值为 {value:X}");
                sbHexString.Append($"{value:X}");
            }
            Console.WriteLine("16进制编码结果：{0}",sbHexString);
            //////////////////////////////////////////////////////////////////////
            string hexString = "48 65 6C 6C 6F 20 57 6F 72 6C 64 21";
            string[] hexValuesSplit = hexString.Split(' ');
            foreach (string hex in hexValuesSplit)
            {
                // Convert the number expressed in base-16 to an integer.
                int value = Convert.ToInt32(hex, 16);
                // Get the character corresponding to the integral value.
                string stringValue = Char.ConvertFromUtf32(value);
                char charValue = (char)value;
                Console.WriteLine("hexadecimal value = {0}, int value = {1}, char value = {2} or {3}", hex, value, stringValue, charValue);
            }
            Console.ReadKey();
        }
    }
}
