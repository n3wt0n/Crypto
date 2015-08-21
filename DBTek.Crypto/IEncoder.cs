using System;

namespace DBTek.Crypto
{
    /// <summary>
    /// Interface for encoders implementation
    /// </summary>
    public interface IEncoder
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
}
