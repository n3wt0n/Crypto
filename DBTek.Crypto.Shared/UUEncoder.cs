using DBTek.Crypto.Extensions;
using System;
using System.Collections.Generic;
using System.IO;

namespace DBTek.Crypto
{
    /// <summary>
    /// UUEncoding encoder implementation
    /// </summary>
    public class UUEncoder : IEncoder
    {

        #region Strings

        /// <summary>
        /// Encode a string using UUEncoder
        /// </summary>
        /// <param name="sourceString">The source string to encode</param>
        /// <returns>The encoded string</returns>
        public string EncodeString(string sourceString)
        {
            if (!sourceString.IsNullOrWhiteSpace())
            {
                int len = sourceString.Length;
                string extraStr = "";
                if (len > 45)
                {
                    extraStr = EncodeString(sourceString.Substring(45));
                    sourceString = sourceString.Substring(0, 45);
                }

                string returnStr = Convert.ToChar(sourceString.Length + 32).ToString();

                while (sourceString.Length % 3 != 0) sourceString += ((char)0).ToString();

                for (int i = 0; i < sourceString.Length; i += 3)
                {
                    string ftet = ((char)(32 + (byte)sourceString[i] / 4)).ToString();
                    ftet += ((char)(32 + ((byte)sourceString[i] % 4) * 16 + (byte)sourceString[i + 1] / 16)).ToString();
                    ftet += ((char)(32 + ((byte)sourceString[i + 1] % 16) * 4 + (byte)sourceString[i + 2] / 64)).ToString();
                    ftet += ((char)(32 + (byte)sourceString[i + 2] % 64)).ToString();

                    returnStr += ftet;
                }

                return returnStr + (extraStr.Length == 0 ? "" : "\r\n") + extraStr;
            }
            else
                return "`";
        }

        /// <summary>
        /// Decode a string encoded with UUEncoder
        /// </summary>
        /// <param name="sourceString">The encoded string to decode</param>
        /// <returns>The decoded string</returns>
        public string DecodeString(string sourceString)
        {
            if (!sourceString.IsNullOrWhiteSpace() && sourceString[0] != '`')
            {
                string returnStr = "";

                string[] lines = sourceString.Split(new char[] { (char)10 }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string str in lines)
                {
                    int len = str[0] - 32;
                    string ret = "";

                    for (int i = 1; i < str.Length; i += 4)
                    {
                        ret += (char)((str[i] - 32) * 4 + (str[i + 1] - 32) / 16);
                        if (ret.Length == len) break;
                        ret += (char)(((str[i + 1] - 32) % 16) * 16 + (str[i + 2] - 32) / 4);
                        if (ret.Length == len) break;
                        ret += (char)(((str[i + 2] - 32) % 4) * 64 + (str[i + 3] - 32));
                        if (ret.Length == len) break;
                    }

                    returnStr += ret;
                }

                return returnStr;
            }
            else
                return String.Empty;
        }

        #endregion

#if !WINDOWS_APP && !WINDOWS_PHONE_APP && !WINDOWS_PHONE
        #region Files

        /// <summary>
        /// Encode a File using UUEncoder
        /// </summary>
        /// <param name="sourceFile">The file to encrypt complete path</param>
        /// <param name="destFile">Destination file complete path. If the file doesn't exist, it creates it</param>
        public void EncodeFile(string sourceFile, string destFile)
        {
            if (sourceFile.IsNullOrWhiteSpace() || !File.Exists(sourceFile))
                throw new FileNotFoundException("Cannot find the specified source file", sourceFile ?? "null");

            if (destFile.IsNullOrWhiteSpace())
                throw new ArgumentException("Please specify the path of the output path", nameof(destFile));

            byte[] bin = File.ReadAllBytes(sourceFile);

            TextWriter fs = new StreamWriter(destFile);

            for (int i = 0; i <= bin.Length; i += 45)
            {
                int l = ((bin.Length - i) > 45) ? 45 : bin.Length - i;
                byte[] linea = new byte[(l % 3 == 0) ? l : l + 3 - l % 3];
                Array.ConstrainedCopy(bin, i, linea, 0, l);
                fs.WriteLine(Array.ConvertAll<byte, char>(EncodeBytes(linea, l), Convert.ToChar));
            }

            fs.Close();
        }

        /// <summary>
        /// Decode a File encoded with UUEncoder
        /// </summary>
        /// <param name="sourceFile">The file to decrypt complete path</param>
        /// <param name="destFile">Destination file complete path. If the file doesn't exist, it creates it</param>
        public void DecodeFile(string sourceFile, string destFile)
        {
            if (sourceFile.IsNullOrWhiteSpace() || !File.Exists(sourceFile))
                throw new FileNotFoundException("Cannot find the specified source file", sourceFile ?? "null");

            if (destFile.IsNullOrWhiteSpace())
                throw new ArgumentException("Please specify the path of the output path", nameof(destFile));

            FileStream fs = null;

            try
            {
                string[] input = File.ReadAllLines(sourceFile);

                fs = new FileStream(destFile, FileMode.Create);
                foreach (string str in input)
                {
                    byte[] dec = Array.ConvertAll<char, byte>(DecodeString(str).ToCharArray(), Convert.ToByte);
                    fs.Write(dec, 0, dec.Length);
                    fs.Flush();
                }
                fs.Close();
            }
            catch
            {

            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        #endregion
#endif

        #region Utils

        private byte[] EncodeBytes(byte[] input, int len)
        {
            if (len == 0) return new byte[] { 96, 13, 10 };

            List<byte> cod = new List<byte>();
            cod.Add((byte)(len + 32));

            for (int i = 0; i < len; i += 3)
            {
                cod.Add((byte)(32 + input[i] / 4));
                cod.Add((byte)(32 + (input[i] % 4) * 16 + input[i + 1] / 16));
                cod.Add((byte)(32 + (input[i + 1] % 16) * 4 + input[i + 2] / 64));
                cod.Add((byte)(32 + input[i + 2] % 64));
            }

            return cod.ToArray();
        }

        #endregion

    }
}