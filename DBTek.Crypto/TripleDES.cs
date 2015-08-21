using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DBTek.Crypto
{
    /// <summary>
    /// TripleDES encoder implementation
    /// </summary>
    public class TripleDES : IEncoder
    {

        #region Config

        private readonly string _psw = "44Nq56Qua8nIEQv";
        private readonly string _IV = "LtWIz6BlyY9gXzs";

        #endregion

        #region Strings

        /// <summary>
        /// Encode a string using TripleDES
        /// </summary>
        /// <param name="sourceString">The source string to encode</param>
        /// <returns>The encoded string</returns>
        public string EncodeString(string sourceString)
            => EncodeString(sourceString, _psw, _IV);        

        /// <summary>
        /// Decode a string encoded in TripleDES
        /// </summary>
        /// <param name="sourceString">The encoded string to decode</param>
        /// <returns>The decoded string</returns>
        public string DecodeString(string sourceString)
            => DecodeString(sourceString, _psw, _IV);        

        /// <summary>
        /// Encode a string using TripleDES with specified password and IV strings.
        /// </summary>
        /// <param name="sourceString">The string to encode</param>
        /// <param name="password">The password string</param>
        /// <param name="IV">The IV string</param>
        /// <returns>The encoded string</returns>
        public string EncodeString(string sourceString, string password, string IV)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Please specify the password", nameof(password));

            if (string.IsNullOrEmpty(IV))
                throw new ArgumentException("Please specify the Initialize Vector", nameof(IV));

            if (!string.IsNullOrEmpty(sourceString))
            {
                byte[] PSS = GeneratePassword(password);
                byte[] IVb = GeneratePassword(IV);

                ICryptoTransform ct = new TripleDESCryptoServiceProvider().CreateEncryptor(PSS, IVb);

                byte[] input = Encoding.Unicode.GetBytes(sourceString);

                return Convert.ToBase64String(ct.TransformFinalBlock(input, 0, input.Length));
            }
            else
                return null;
        }

        /// <summary>
        /// Decode a string encoded using TripleDES with specified password and IV strings.
        /// </summary>
        /// <param name="sourceString">The string to decode</param>
        /// <param name="password">The password string</param>
        /// <param name="IV">The IV string</param> 
        /// <returns>The decoded string</returns>
        public string DecodeString(string sourceString, string password, string IV)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Please specify the password", nameof(password));

            if (string.IsNullOrEmpty(IV))
                throw new ArgumentException("Please specify the Initialize Vector", nameof(IV));

            if (!string.IsNullOrEmpty(sourceString))
            {
                byte[] PSS = GeneratePassword(password);
                byte[] IVb = GeneratePassword(IV);

                ICryptoTransform ct = new TripleDESCryptoServiceProvider().CreateDecryptor(PSS, IVb);

                byte[] input = Convert.FromBase64String(sourceString);

                byte[] output = ct.TransformFinalBlock(input, 0, input.Length);
                return Encoding.Unicode.GetString(output);
            }
            else
                return null;
        }

        #endregion

        #region Files

        /// <summary>
        /// Encode a File using TripleDES
        /// </summary>
        /// <param name="sourceFile">The file to encrypt complete path</param>
        /// <param name="destFile">Destination file complete path. If the file doesn't exist, it creates it</param>
        public void EncodeFile(string sourceFile, string destFile)
            => EncodeFile(sourceFile, destFile, _psw, _IV);        

        /// <summary>
        /// Decode a File encoded in TripleDES
        /// </summary>
        /// <param name="sourceFile">The file to decrypt complete path</param>
        /// <param name="destFile">Destination file complete path. If the file doesn't exist, it creates it</param>
        public void DecodeFile(string sourceFile, string destFile)
            => DecodeFile(sourceFile, destFile, _psw, _IV);        

        /// <summary>
        /// Encode a file using Rijndael with specified password and IV strings.
        /// </summary>
        /// <param name="sourceFile">The file to encrypt complete path</param>
        /// <param name="destFile">Destination file complete path. If the file doesn't exist, it creates it</param>
        /// <param name="password">The password string</param>
        /// <param name="IV">The IV string</param>        
        public void EncodeFile(String sourceFile, String destFile, String password, String IV)
        {
            if (string.IsNullOrWhiteSpace(sourceFile) || !File.Exists(sourceFile))
                throw new FileNotFoundException("Cannot find the specified source file", sourceFile ?? "null");

            if (string.IsNullOrWhiteSpace(destFile))
                throw new ArgumentException("Please specify the path of the output path", nameof(destFile));

            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Please specify the password", nameof(password));

            if (string.IsNullOrEmpty(IV))
                throw new ArgumentException("Please specify the Initialize Vector", nameof(IV));

            byte[] Key = GeneratePassword(password);
            byte[] IVb = GeneratePassword(IV);

            byte[] data = File.ReadAllBytes(sourceFile);

            FileStream fsCipherText = new FileStream(destFile, FileMode.Create, FileAccess.Write);
            fsCipherText.SetLength(0);

            // Create a Crypto Stream that transforms the file stream using the chosen 
            // encryption and writes it to the output FileStream object.

            CryptoStream cs = new CryptoStream(fsCipherText, new TripleDESCryptoServiceProvider().CreateEncryptor(Key, IVb), CryptoStreamMode.Write);
            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();
            // Clean up. There is no need to call fsCipherText.Close() because closing the
            // crypto stream automatically encloses the stream that was passed into it.
            cs.Close();
        }

        /// <summary>
        /// Decode a file encripted using TripleDES with specified password and IV strings.
        /// </summary>
        /// <param name="sourceFile">The file to decrypt complete path</param>
        /// <param name="destFile">Destination file complete path. If the file doesn't exist, it creates it</param>
        /// <param name="password">The password string</param>
        /// <param name="IV">The IV string</param>        

        public void DecodeFile(String sourceFile, String destFile, String password, String IV)
        {
            if (string.IsNullOrWhiteSpace(sourceFile) || !File.Exists(sourceFile))
                throw new FileNotFoundException("Cannot find the specified source file", sourceFile ?? "null");

            if (string.IsNullOrWhiteSpace(destFile))
                throw new ArgumentException("Please specify the path of the output path", nameof(destFile));

            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Please specify the password", nameof(password));

            if (string.IsNullOrEmpty(IV))
                throw new ArgumentException("Please specify the Initialize Vector", nameof(IV));

            byte[] Key = GeneratePassword(password);
            byte[] IVb = GeneratePassword(IV);

            byte[] data = File.ReadAllBytes(sourceFile);

            FileStream fsPlainText = new FileStream(destFile, FileMode.Create, FileAccess.Write);
            fsPlainText.SetLength(0);

            // Create a Crypto Stream that transforms the file stream using the chosen 
            // encryption and writes it to the output FileStream object.

            CryptoStream cs = new CryptoStream(fsPlainText, new TripleDESCryptoServiceProvider().CreateDecryptor(Key, IVb), CryptoStreamMode.Write);
            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();
            // Clean up. There is no need to call fsCipherText.Close() because closing the
            // crypto stream automatically encloses the stream that was passed into it.
            cs.Close();
        }

        #endregion

        #region Utils

        private byte[] GeneratePassword(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] input = Encoding.UTF8.GetBytes(password);
            return md5.ComputeHash(input, 0, input.Length);
        }

        #endregion

    }
}
