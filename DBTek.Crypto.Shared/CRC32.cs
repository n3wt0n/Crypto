using DBTek.Crypto.Extensions;
using DBTek.Crypto.Helpers;
using System;
using System.IO;

namespace DBTek.Crypto
{
    /// <summary>
    /// CRC32 hasher implementation
    /// </summary>
    public class CRC32_Hsr : IHasher
    {

        #region Strings

        /// <summary>
        /// Hash a string using CRC32
        /// </summary>
        /// <param name="sourceString">The string to hash</param>
        /// <returns>The hash</returns>
        public string HashString(string sourceString)
        {
            if (sourceString != null)
            {
                using (var crc32 = new CRC32Helper())
                {
                    return crc32.ComputeChecksumAsString(sourceString.ToByteArray());
                }
            }
            else
                return String.Empty;
        }

        #endregion

        #region Files
#if !WINDOWS_APP && !WINDOWS_PHONE_APP && !WINDOWS_PHONE && !WINDOWS_UWP
        /// <summary>
        /// Hash a file using CRC32
        /// </summary>
        /// <param name="sourceFile">The file to hash complete path</param>
        /// <returns>The hash</returns>
        public string HashFile(string sourceFile)
        {
            if (sourceFile.IsNullOrWhiteSpace() || !File.Exists(sourceFile))
                throw new FileNotFoundException("Cannot find the specified source file", sourceFile ?? "null");

            byte[] fileBytes = File.ReadAllBytes(sourceFile);
            using (var crc32 = new CRC32Helper())
            {
                return crc32.ComputeChecksumAsString(fileBytes);
            }
        }
#endif
#endregion
    }
}