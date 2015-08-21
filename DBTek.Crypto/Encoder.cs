using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBTek.Crypto
{    
    /// <summary>
    /// Interface for encoders implementation
    /// </summary>
    public interface iEncoder
    {
        /// <summary>
        /// General methof signature for encoding strings
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        string EncodeString(string inputString);

        /// <summary>
        /// General methof signature for decoding  strings
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        string DecodeString(string inputString);

        /// <summary>
        /// General methof signature for encoding files
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destFile"></param>
        void EncodeFile(String sourceFile, String destFile);
        
        /// <summary>
        /// General methof signature for decoding files
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destFile"></param>
        void DecodeFile(String sourceFile, String destFile);

    }

    /// <summary>
    /// Interface for hashers implementation
    /// </summary>
    public interface iHasher
    {
        /// <summary>
        /// General methof signature for hashing strings
        /// </summary>
        /// <param name="stringa"></param>
        /// <returns></returns>
        string HashString(string stringa);

        /// <summary>
        /// General methof signature for hashing files
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <returns></returns>
        string HashFile(string sourceFile);
    }

    class ClearData : iEncoder, iHasher
    {
        public string EncodeString(string stringa) { return stringa; }

        public string DecodeString(string stringa) { return stringa; }

        public void EncodeFile(String sourceFile, String destFile)
        {
            throw new NotImplementedException();
        }

        public void DecodeFile(String sourceFile, String destFileT)
        {
            throw new NotImplementedException();
        }

        public string HashString(string stringa)
        {
            return stringa;
        }

        public string HashFile(string sourceFile)
        {
            return sourceFile;
        }
    }
}