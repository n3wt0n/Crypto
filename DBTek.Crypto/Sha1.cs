using DBTek.Crypto.Extensions;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DBTek.Crypto
{
    /// <summary>
    /// SHA1 encoder implementation
    /// </summary>
    public class SHA1_Hsr : IHasher
    {

        #region Strings

        /// <summary>
        /// Hash a string using SHA1
        /// </summary>
        /// <param name="sourceString">The string to hash</param>
        /// <returns>The hash</returns>
        public string HashString(string sourceString)
        {
            if (sourceString != null)
            {
                byte[] message = sourceString.ToByteArray();
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
        /// Hash a file using SHA1
        /// </summary>
        /// <param name="sourceFile">The file to hash complete path</param>
        /// <returns>The hash</returns>
        public string HashFile(string sourceFile)
        {
            if (string.IsNullOrWhiteSpace(sourceFile) || !File.Exists(sourceFile))
                throw new FileNotFoundException("Cannot find the specified source file", sourceFile ?? "null");

            using (var stream = new BufferedStream(File.OpenRead(sourceFile), 1200000))
            {
                SHA1Managed sha = new SHA1Managed();
                byte[] checksum = sha.ComputeHash(stream);
                return BitConverter.ToString(checksum).Replace("-", String.Empty);
            }
        }

        #endregion

        #region Utils

        private byte[] HashBytes(byte[] input)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            return sha.ComputeHash(input);
        }

        #endregion       
    }
}