using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace System
{
    public class LogHelper
    {
        private static readonly object lockLog = new object();
        public static void Write(Exception ex)
        {
            string content = string.Format("错误信息：\r{0}\r堆栈信息：{1}", ex.Message, ex.StackTrace);
            Write(content, "error");
        }
        public static void Write(string content, string folder)
        {
            DateTime now = DateTime.Now;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log", folder);
            lock (lockLog)
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string fileName = Path.Combine(path, string.Format("{0}-{1}-{2}.log", now.Year, now.Month, now.Day));
                using (StreamWriter sw = new StreamWriter(fileName, true, Encoding.UTF8))
                {
                    sw.WriteLine(DateTime.Now.ToString());
                    sw.WriteLine(content);
                }
            }
        }

    }
}
