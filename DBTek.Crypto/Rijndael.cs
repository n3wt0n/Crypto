using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DBTek.Crypto
{
    /// <summary>
    /// Rijndael encoder implementation
    /// </summary>
    public class Rijndael : IEncoder
    {
       
        #region Config

        private readonly string _psw = "qiKJFd5UFMGUQUc";
        private readonly string _IV = "wALM1JMvr7azxKQ";

        #endregion

        #region Strings

        /// <summary>
        /// Encode a string using Rijndael.
        /// </summary>
        /// <param name="sourceString">The string to encode</param>        
        /// <returns>The encoded string</returns>
        public string EncodeString(string sourceString)
            => EncodeString(sourceString, _psw, _IV);        

        /// <summary>
        /// Decode a string encrypted using Rijndael.
        /// </summary>
        /// <param name="sourceString">The string to decode</param>        
        /// <returns>The decoded string</returns>
        public string DecodeString(string sourceString)
            => DecodeString(sourceString, _psw, _IV);        

        /// <summary>
        /// Encode a string using Rijndael with specified password and IV strings.
        /// </summary>
        /// <param name="sourceString">The string to encode</param>
        /// <param name="password">The password string</param>
        /// <param name="IV">The IV string</param>
        /// <returns>The encoded string</returns>
        public string EncodeString(string sourceString, string password, String IV)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Please specify the password", nameof(password));

            if (string.IsNullOrEmpty(IV))
                throw new ArgumentException("Please specify the Initialize Vector", nameof(IV));

            if (!string.IsNullOrEmpty(sourceString))
            {
                byte[] encrypted;
                // Create an Rijndael object
                // with the specified key and IV.
                using (System.Security.Cryptography.Rijndael rijAlg = System.Security.Cryptography.Rijndael.Create())
                {
                    rijAlg.Key = GeneratePassword(password);
                    rijAlg.IV = GeneratePassword(IV);

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                    // Create the streams used for encryption.
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                //Write all data to the stream.
                                swEncrypt.Write(sourceString);
                            }
                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }
                return Convert.ToBase64String(encrypted);
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// Decode a string endoced using Rijndael with specified password and IV strings.
        /// </summary>
        /// <param name="sourceString">The string to decode</param>
        /// <param name="password">The password string</param>
        /// <param name="IV">The IV string</param> 
        /// <returns>The decoded string</returns>
        public string DecodeString(string sourceString, string password, String IV)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Please specify the password", nameof(password));

            if (string.IsNullOrEmpty(IV))
                throw new ArgumentException("Please specify the Initialize Vector", nameof(IV));

            if (!string.IsNullOrEmpty(sourceString))
            {
                byte[] cipherText = Convert.FromBase64String(sourceString);

                string plaintext = null;

                // Create an Rijndael object
                // with the specified key and IV.
                using (System.Security.Cryptography.Rijndael rijAlg = System.Security.Cryptography.Rijndael.Create())
                {
                    rijAlg.Key = GeneratePassword(password);
                    rijAlg.IV = GeneratePassword(IV);

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                    // Create the streams used for decryption.
                    using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                return plaintext;
            }
            else
                return string.Empty;
        }

        #endregion

        #region Files

        /// <summary>
        /// Encode a file using Rijndael.
        /// </summary>
        /// <param name="sourceFile">The file to encrypt complete path</param>        
        /// <param name="destFile">Destination file complete path. If the file doesn't exist, it creates it</param>        
        public void EncodeFile(string sourceFile, string destFile)
        {
            if (string.IsNullOrWhiteSpace(sourceFile) || !File.Exists(sourceFile))
                throw new FileNotFoundException("Cannot find the specified source file", sourceFile ?? "null");

            if (string.IsNullOrWhiteSpace(destFile))
                throw new ArgumentException("Please specify the path of the output path", nameof(destFile));

            EncodeFile(sourceFile, destFile, _psw, _IV);
        }

        /// <summary>
        /// Decode a file encripted using Rijndael with specified password and IV strings.
        /// </summary>
        /// <param name="sourceFile">The file to decrypt complete path</param>
        /// <param name="destFile">Destination file complete path. If the file doesn't exist, it creates it</param>        
        public void DecodeFile(string sourceFile, string destFile)
        {
            if (string.IsNullOrWhiteSpace(sourceFile) || !File.Exists(sourceFile))
                throw new FileNotFoundException("Cannot find the specified source file", sourceFile ?? "null");

            if (string.IsNullOrWhiteSpace(destFile))
                throw new ArgumentException("Please specify the path of the output path", nameof(destFile));

            DecodeFile(sourceFile, destFile, _psw, _IV);
        }

        /// <summary>
        /// Encode a file using Rijndael with specified password and IV strings.
        /// </summary>
        /// <param name="sourceFile">The file to encrypt complete path</param>
        /// <param name="password">The password string</param>
        /// <param name="IV">The IV string</param>
        /// <param name="destFile">Destination file complete path. If the file doesn't exist, it creates it</param>        
        public void EncodeFile(string sourceFile, string destFile, string password, string IV)
        {
            if (string.IsNullOrWhiteSpace(sourceFile) || !File.Exists(sourceFile))
                throw new FileNotFoundException("Cannot find the specified source file", sourceFile ?? "null");

            if (string.IsNullOrWhiteSpace(destFile))
                throw new ArgumentException("Please specify the path of the output path", nameof(destFile));

            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Please specify the password", nameof(password));

            if (string.IsNullOrEmpty(IV))
                throw new ArgumentException("Please specify the Initialize Vector", nameof(IV));

            RijndaelManaged rijndael = new RijndaelManaged();
            rijndael.IV = GeneratePassword(IV);
            rijndael.Key = GeneratePassword(password);

            byte[] inputBytes = File.ReadAllBytes(sourceFile);

            byte[] outputBytes = rijndael.CreateEncryptor()
                .TransformFinalBlock(inputBytes, 0, inputBytes.Length);
            
            File.WriteAllBytes(destFile, outputBytes);            
        }

        /// <summary>
        /// Decode a file encripted using Rijndael with specified password and IV strings.
        /// </summary>
        /// <param name="sourceFile">The file to decrypt complete path</param>
        /// <param name="destFile">Destination file complete path. If the file doesn't exist, it creates it</param>
        /// <param name="password">The password string</param>
        /// <param name="IV">The IV string</param>        
        public void DecodeFile(string sourceFile, string destFile, string password, string IV)
        {
            if (string.IsNullOrWhiteSpace(sourceFile) || !File.Exists(sourceFile))
                throw new FileNotFoundException("Cannot find the specified source file", sourceFile ?? "null");

            if (string.IsNullOrWhiteSpace(destFile))
                throw new ArgumentException("Please specify the path of the output path", nameof(destFile));

            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Please specify the password", nameof(password));

            if (string.IsNullOrEmpty(IV))
                throw new ArgumentException("Please specify the Initialize Vector", nameof(IV));

            RijndaelManaged rijndael = new RijndaelManaged();
            rijndael.IV = GeneratePassword(IV);
            rijndael.Key = GeneratePassword(password);

            byte[] inputBytes = File.ReadAllBytes(sourceFile);

            byte[] outputBytes = rijndael.CreateDecryptor()
                .TransformFinalBlock(inputBytes, 0, inputBytes.Length);

            File.WriteAllBytes(destFile, outputBytes);
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
