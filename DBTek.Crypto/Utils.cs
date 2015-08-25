using System;
using System.Text;

namespace DBTek.Crypto
{
    /// <summary>
    /// Utility classes
    /// </summary>
    public class Utils
    {        
        // Convert a byte array to a string.
        internal static string ByteArrayToStr(Byte[] bytearray)
        {
            var enc = new UTF8Encoding();
            return enc.GetString(bytearray);
        }
    }
}
