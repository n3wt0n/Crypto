using System;
using System.Collections.Generic;
using System.IO;

namespace DBTek.Crypto
{
    /// <summary>
    /// QuotedPrintable encoder implementation
    /// </summary>
    public class QuotedPrintable : iEncoder
    {

        #region Strings

        /// <summary>
        /// Encode a string using QuotedPrintable
        /// </summary>
        /// <param name="sourceString">The string to encode</param>
        /// <returns>The encoded string</returns>
        public string EncodeString(string sourceString)
        {
            if (!string.IsNullOrWhiteSpace(sourceString))
            {
                string str = "";
                List<string> Coded = EncodeBytes(Array.ConvertAll<char, byte>(sourceString.ToCharArray(), new Converter<char, byte>(Convert.ToByte)));

                for (int i = 0; i < Coded.Count; i++)
                {
                    str += Coded[i];
                    if (i < Coded.Count - 1) str += "\r\n";
                }

                return str;
            }
            else
                return String.Empty;
        }

        /// <summary>
        /// Decode a string encoded with QuotedPrintable
        /// </summary>
        /// <param name="sourceString">The encoded string to decode</param>
        /// <returns>The decoded string</returns>
        public string DecodeString(string sourceString)
        {
            if (!string.IsNullOrWhiteSpace(sourceString))
            {
                string returnStr = "";

                char[] chars = sourceString.ToCharArray();

                for (int i = 0; i < chars.Length; i++)
                {
                    char dec = chars[i];

                    if (dec == 61) //CompareTo("=")
                    {
                        string hex = sourceString.Substring(i + 1, 2);
                        dec = Convert.ToChar(Byte.Parse(hex, System.Globalization.NumberStyles.AllowHexSpecifier));
                        i += 2;
                    }

                    returnStr += dec;
                }

                return returnStr;
            }
            else
                return String.Empty;
        }

        #endregion

        #region Files

        /// <summary>
        /// Encode a file using QuotedPrintable
        /// </summary>
        /// <param name="sourceFile">The file to encrypt complete path</param>
        /// <param name="destFile">Destination file complete path. If the file doesn't exist, it creates it</param>
        public void EncodeFile(string sourceFile, string destFile)
        {
            if (string.IsNullOrWhiteSpace(sourceFile) || !File.Exists(sourceFile))
                throw new FileNotFoundException("Cannot find the specified source file", sourceFile ?? "null");

            if (string.IsNullOrWhiteSpace(destFile))
                throw new ArgumentException("Please specify the path of the output path", "destFile");

            byte[] binput = File.ReadAllBytes(sourceFile);
            TextWriter tw = new StreamWriter(destFile);
            foreach (string l in EncodeBytes(binput))
                tw.WriteLine(l);
            tw.Close();
        }

        /// <summary>
        /// Decode a file encoded with QuotedPrintable
        /// </summary>
        /// <param name="sourceFile">The file to decrypt complete path</param>
        /// <param name="destFile">Destination file complete path. If the file doesn't exist, it creates it</param>
        public void DecodeFile(string sourceFile, string destFile)
        {
            if (string.IsNullOrWhiteSpace(sourceFile) || !File.Exists(sourceFile))
                throw new FileNotFoundException("Cannot find the specified source file", sourceFile ?? "null");

            if (string.IsNullOrWhiteSpace(destFile))
                throw new ArgumentException("Please specify the path of the output path", "destFile");

            string[] input = File.ReadAllLines(sourceFile);
            FileStream fs = new FileStream(destFile, FileMode.Create);

            for (int i = 0; i < input.Length; i++)
            {
                bool endline = true;
                string s = input[i];
                char[] trimmer = new char[] { ' ' };
                s.TrimEnd(trimmer);
                while (s.EndsWith("="))
                {
                    s = s.Remove(s.Length - 1);
                    if (s.Length > 1000)
                    {
                        endline = false;
                        break;
                    }
                    i++;
                    s += input[i];
                    s.TrimEnd(trimmer);
                }

                byte[] output = Array.ConvertAll<char, byte>(DecodeString(s).ToCharArray(), new Converter<char, byte>(Convert.ToByte));
                fs.Write(output, 0, output.Length);
                if (i < input.Length - 1 && endline)
                {
                    fs.WriteByte(13);
                    fs.WriteByte(10);
                }
                fs.Flush();
            }

            fs.Close();
        }

        #endregion

        #region Utils

        private List<string> EncodeBytes(byte[] stringa)
        {
            string returnStr = "";
            List<string> retList = new List<string>();

            for (int i = 0; i < stringa.Length; i++)
            {
                if (returnStr.Length > 72)
                {
                    returnStr += "=";
                    retList.Add(returnStr);
                    returnStr = "";
                }

                byte b = stringa[i];

                if (b < 32 || b == 61 || b > 126)
                {
                    if (b == 13 && i < stringa.Length - 1 && stringa[i + 1] == 10)
                    {
                        if (returnStr.EndsWith(" "))
                            returnStr = returnStr.Remove(returnStr.Length - 1) + "=20";
                        retList.Add(returnStr);
                        returnStr = "";
                        i++;
                        continue;
                    }
                    returnStr += "=";
                    returnStr += (String.Format("{0:x2}", b).ToUpper());
                    continue;
                }
                returnStr += (char)b;
            }
            retList.Add(returnStr);

            return retList;
        }

        #endregion

    }
}