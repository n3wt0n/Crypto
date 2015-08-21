using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBTek.Crypto
{
    /// <summary>
    /// Utility classes
    /// </summary>
    public class Utils
    {
        // Convert a string to a byte array.
        internal static byte[] StrToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

        // Convert a byte array to a string.
        internal static string ByteArrayToStr(Byte[] bytearray)
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            return enc.GetString(bytearray);
        }
    }
}
