namespace DBTek.Crypto
{
    /// <summary>
    /// Interface for hashers implementation
    /// </summary>
    public interface IHasher
    {
        /// <summary>
        /// General methof signature for hashing strings
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        string HashString(string sourceString);

#if !WINDOWS_APP && !WINDOWS_PHONE_APP && !WINDOWS_PHONE
        /// <summary>
        /// General methof signature for hashing files
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <returns></returns>
        string HashFile(string sourceFile);
#endif
    }
}
