using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HelpLib.Crypto
{
    public class StreamHelper
    {
        public static byte[] GetBytes(Stream scoureStream)
        {
            if (!scoureStream.CanRead)
                return null;
            scoureStream.Seek(0, SeekOrigin.Begin);
            var buffer = new byte[scoureStream.Length];
            scoureStream.Read(buffer, 0, buffer.Length);
            return buffer;
        }
    }
}
