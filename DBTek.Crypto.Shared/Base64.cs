﻿using DBTek.Crypto.Extensions;
using System;
using System.IO;

namespace DBTek.Crypto
{
    /// <summary>
    /// Base64 encoder implementation
    /// </summary>
    public class Base64 : IEncoder
    {

        #region Strings

        /// <summary>
        /// Encode a string using Base64 format
        /// </summary>
        /// <param name="sourceString">The source string to encode</param>
        /// <returns>The encoded string</returns>
        public string EncodeString(String sourceString)
        {
            if (!sourceString.IsNullOrWhiteSpace())
            {
                byte[] filebytes = sourceString.ToByteArray();
                return Convert.ToBase64String(filebytes);
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// Decode a string encoded in Base64 format
        /// </summary>
        /// <param name="sourceString">The encoded string to decode</param>
        /// <returns>The decoded string</returns>
        public string DecodeString(String sourceString)
        {
            if (!sourceString.IsNullOrWhiteSpace())
            {
                byte[] filebytes = Convert.FromBase64String(sourceString);
                return Utils.ByteArrayToStr(filebytes);
            }
            else
                return string.Empty;
        }

        #endregion

#if !WINDOWS_APP && !WINDOWS_PHONE_APP && !WINDOWS_PHONE && !WINDOWS_UWP
        #region Files

        /// <summary>
        /// Encode a File using Base64 format
        /// </summary>
        /// <param name="sourceFile">The file to encrypt complete path</param>
        /// <param name="destFile">Destination file complete path. If the file doesn't exist, it creates it</param>
        public void EncodeFile(String sourceFile, String destFile)
        {
            if (sourceFile.IsNullOrWhiteSpace() || !File.Exists(sourceFile))
                throw new FileNotFoundException("Cannot find the specified source file", sourceFile ?? "null");

            if (destFile.IsNullOrWhiteSpace())
                throw new ArgumentException("Please specify the path of the output path", nameof(destFile));

            using (var fs = new BufferedStream(File.OpenRead(sourceFile), 1200000))
            {
                byte[] filebytes = new byte[fs.Length];
                fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
                string encodedData = Convert.ToBase64String(filebytes, Base64FormattingOptions.InsertLineBreaks);
                File.WriteAllText(destFile, encodedData);
            }
        }

        /// <summary>
        /// Decode a File encoded in Base64 format
        /// </summary>
        /// <param name="sourceFile">The file to decrypt complete path</param>
        /// <param name="destFile">Destination file complete path. If the file doesn't exist, it creates it</param>
        public void DecodeFile(String sourceFile, String destFile)
        {
            if (sourceFile.IsNullOrWhiteSpace() || !File.Exists(sourceFile))
                throw new FileNotFoundException("Cannot find the specified source file", sourceFile ?? "null");

            if (destFile.IsNullOrWhiteSpace())
                throw new ArgumentException("Please specify the path of the output path", nameof(destFile));

            string input = File.ReadAllText(sourceFile);
            byte[] filebytes = Convert.FromBase64String(input);
            FileStream fs = new FileStream(destFile, FileMode.Create, FileAccess.Write, FileShare.None);
            fs.Write(filebytes, 0, filebytes.Length);
            fs.Close();
        }

        #endregion
#endif
    }
}
