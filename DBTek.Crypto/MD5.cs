using DBTek.Crypto.Extensions;
using System;
using System.IO;

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
                return Helpers.MD5.GetHashString(sourceString);            
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
            if (sourceFile.IsNullOrWhiteSpace() || !File.Exists(sourceFile))
                throw new FileNotFoundException("Cannot find the specified source file", sourceFile ?? "null");

            byte[] message = File.ReadAllBytes(sourceFile);
            return Helpers.MD5.GetHashString(message);
        }

        #endregion

    }
}