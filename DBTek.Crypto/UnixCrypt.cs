using DBTek.Crypto.Extensions;
using System;
using System.IO;
using System.Text;

namespace DBTek.Crypto
{
    /// <summary>
    /// Types of supported Unix-like alghoritm
    /// </summary>
    public enum UnixCryptTypes
    {
        /// <summary>
        /// MD5 Unix-like alghoritm
        /// </summary>
        MD5 = 1,

        /// <summary>
        /// SHA2-256 Unix-like alghoritm
        /// </summary>
        SHA2_256 = 5,

        /// <summary>
        /// SHA2-512 Unix-like alghoritm
        /// </summary>
        SHA2_512 = 6
    }

    /// <summary>
    /// Unix hasher implementation
    /// </summary>
    public class UnixCrypt : IHasher
    {

        #region Strings

        /// <summary>
        /// Hash a string using a Unix-like format SHA2-512
        /// </summary>
        /// <param name="sourceString">The string to hash</param>
        /// <returns>The hash</returns>
        public string HashString(string sourceString)
            => HashString(sourceString, generateSalt(), UnixCryptTypes.SHA2_512);        

        /// <summary>
        /// Hash a string using a Unix-like format with the provided Crypt Alghoritm type
        /// </summary>
        /// <param name="sourceString">The string to hash</param>
        /// <param name="unixCryptType">The Crypt Alghoritm type</param>
        /// <returns>The hash</returns>
        public string HashString(string sourceString, UnixCryptTypes unixCryptType)
            => HashString(sourceString, generateSalt(), unixCryptType);        

        /// <summary>
        /// Hash a string using a Unix-like format with the provided Crypt Alghoritm type and a salt
        /// </summary>
        /// <param name="sourceString">The string to hash</param>
        /// <param name="salt">The salt to apply to the hash</param>
        /// <param name="unixCryptType">The Crypt Alghoritm type</param>
        /// <returns>The hash</returns>
        public string HashString(string sourceString, string salt, UnixCryptTypes unixCryptType)
        {
            if (string.IsNullOrEmpty(salt))
                throw new ArgumentException("Please specify the salt", nameof(salt));

            if (sourceString != null)
            {
                salt = "$" + (int)unixCryptType + "$" + salt;
                return UnixCryptAlg.CryptUtils.Crypt(sourceString, salt);
            }
            else
                return String.Empty;
        }

        #endregion

        #region Files

        /// <summary>
        /// Hash a file using a Unix-like format SHA2-512
        /// </summary>
        /// <param name="sourceFile">The file to hash complete path</param>                
        /// <returns>The hash</returns>
        public string HashFile(string sourceFile)
            => HashFile(sourceFile, generateSalt(), UnixCryptTypes.SHA2_512);        

        /// <summary>
        /// Hash a file using a Unix-like format with the provided Crypt Alghoritm type
        /// </summary>
        /// <param name="sourceFile">The file to hash complete path</param>        
        /// <param name="unixCryptType">The Crypt Alghoritm type</param>
        /// <returns>The hash</returns>
        public string HashFile(string sourceFile, UnixCryptTypes unixCryptType)
            => HashFile(sourceFile, generateSalt(), unixCryptType);        

        /// <summary>
        /// Hash a file using a Unix-like format with the provided Crypt Alghoritm type and a salt
        /// </summary>
        /// <param name="sourceFile">The file to hash complete path</param>
        /// <param name="salt">The salt to apply to the hash</param>
        /// <param name="unixCryptType">The Crypt Alghoritm type</param>
        /// <returns>The hash</returns>
        public string HashFile(string sourceFile, string salt, UnixCryptTypes unixCryptType)
        {
            if (sourceFile.IsNullOrWhiteSpace() || !File.Exists(sourceFile))
                throw new FileNotFoundException("Cannot find the specified source file", sourceFile ?? "null");

            var text = File.ReadAllText(sourceFile);

            return HashString(text, salt, unixCryptType);
        }

        #endregion

        #region Utils

        private static Random random = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// generate a 16char random salt
        /// </summary>
        /// <returns></returns>
        private string generateSalt()
        {
            int size = 16;

            var builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
        #endregion

    }
}
