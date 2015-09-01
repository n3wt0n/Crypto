using DBTek.Crypto.Extensions;
using DBTek.Crypto.Helpers;
using System;
using System.IO;
using System.Security.Cryptography;

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
                byte[] message = sourceString.ToByteArray();
                var hashString = new SHA1Managed();
                string hex = "";
                foreach (byte x in HashBytes(message))
                    hex += Convert.ToString(x, 16).PadLeft(2, '0');
                return hex;
            }
            else
                return String.Empty;
        }

        #endregion

        #region Files

        /// <summary>
        /// Hash a file using CRC32
        /// </summary>
        /// <param name="sourceFile">The file to hash complete path</param>
        /// <returns>The hash</returns>
        public string HashFile(string sourceFile)
        {
            if (sourceFile.IsNullOrWhiteSpace() || !File.Exists(sourceFile))
                throw new FileNotFoundException("Cannot find the specified source file", sourceFile ?? "null");

            byte[] message = File.ReadAllBytes(sourceFile);
            var hashString = new SHA1Managed();
            string hex = "";
            foreach (byte x in HashBytes(message))
                hex += Convert.ToString(x, 16).PadLeft(2, '0');
            return hex;
        }

        #endregion

        #region Utils

        private byte[] HashBytes(byte[] input)
        {
            var crc = new Crc32();
            return crc.ComputeHash(input);
        }        
        #endregion
    }
}