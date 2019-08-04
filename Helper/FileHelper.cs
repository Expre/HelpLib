using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace System
{
    public class FileHelper
    {
        /// <summary>
        /// 计算文件大小函数(保留两位小数),Size为字节大小
        /// </summary>
        /// <param name="size">初始文件大小</param>
        /// <returns>B，KB，GB，TB</returns>
        public static string GetSizeString(long size)
        {
            string strSize = "";
            long factSize = size;
            if (factSize < 1024.00)
                strSize = factSize.ToString("F2") + " 字节";
            else if (factSize >= 1024.00 && factSize < 1048576)
                strSize = (factSize / 1024.00).ToString("F2") + " KB";
            else if (factSize >= 1048576 && factSize < 1073741824)
                strSize = (factSize / 1024.00 / 1024.00).ToString("F2") + " MB";
            else if (factSize >= 1073741824)
                strSize = (factSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " GB";
            return strSize;
        }
        public static void Write(string filePath, byte[] buffer)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    FileInfo file = new FileInfo(filePath);
                    FileStream fs = file.Create();
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string ReadText(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string res = sr.ReadToEnd();
                return res;
            }
        }
    }
}
