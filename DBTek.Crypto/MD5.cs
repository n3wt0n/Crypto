using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DBTek.Crypto
{
    /// <summary>
    /// MD5 hasher implementation
    /// </summary>
    public class MD5_Hsr : IHasher
    {

        #region Strings

        /// <summary>
        /// Hash a string using MD5
        /// </summary>
        /// <param name="sourceString">The string to hash</param>
        /// <returns>The hash</returns>
        public string HashString(string sourceString)
        {
            if (sourceString != null)
            {
                byte[] message = Encoding.UTF8.GetBytes(sourceString);
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
        /// Hash a file using MD5
        /// </summary>
        /// <param name="sourceFile">The file to hash complete path</param>
        /// <returns>The hash</returns>
        public string HashFile(string sourceFile)
        {
            if (string.IsNullOrWhiteSpace(sourceFile) || !File.Exists(sourceFile))
                throw new FileNotFoundException("Cannot find the specified source file", sourceFile ?? "null");

            byte[] message = File.ReadAllBytes(sourceFile);
            string hex = "";
            foreach (byte x in HashBytes(message))
                hex += Convert.ToString(x, 16).PadLeft(2, '0');
            return hex;
        }

        #endregion

        #region Utils

        private byte[] HashBytes(byte[] input)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            return md5.ComputeHash(input);
        }

        #endregion

    }
}