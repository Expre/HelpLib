using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace System
{
    public class RandomHelper
    {
        private static readonly Dictionary<char, int> letterNumbers = new Dictionary<char, int>();
        static RandomHelper()
        {
            for (char i = 'a'; i <= 'z'; i++)
            {
                if (!letterNumbers.ContainsKey(i))
                    letterNumbers.Add(i, letterNumbers.Count % 10);
            }
            letterNumbers.Add('-', 0);
            for (int i = 0; i < 10; i++)
            {
                if (!letterNumbers.ContainsKey(char.Parse(i.ToString())))
                    letterNumbers.Add(char.Parse(i.ToString()), i);
            }
        }
        public static string Get(int length)
        {
            string guid = Guid.NewGuid().ToString();
            if (length > 36)
                length = 36;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < guid.Length; i++)
            {
                if (stringBuilder.Length >= length)
                    break;
                stringBuilder.Append(letterNumbers[guid[i]]);
            }
            return stringBuilder.ToString();
        }
        public static int Get()
        {
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[8];
                RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider();
                rngServiceProvider.GetBytes(randomBytes);
                int result = BitConverter.ToInt32(randomBytes, 0);
                result = System.Math.Abs(result);  //求绝对值
                return result;
            }
        }
        public static int Get(int min, int max)
        {
            Random random = new Random(int.Parse(Get(1)));
            int res = random.Next(min, max);
            return res;
        }
    }
}
