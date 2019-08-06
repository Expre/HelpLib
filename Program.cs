using System;
using System.Text;

namespace Assist
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dateTime = DataFormater.Convert<DateTime, string>("2019-08-06 11:27:25");
            CacheManager.Add("test", "sunzhimin", 1);
            for (int i = 0; i < 1000; i++)
            {
                System.Threading.Thread.Sleep(1000);
                Console.Write(i);
                Console.WriteLine(CacheManager.Get<string>("test"));
            }
            Console.ReadKey();
        }
    }
}
