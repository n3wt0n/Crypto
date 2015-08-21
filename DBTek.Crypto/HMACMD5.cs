using System;
using System.IO;
using System.Security.Cryptography;

namespace DBTek.Crypto
{
    /// <summary>
    /// HMACMD5 encoder implementation
    /// </summary>
    public class cHMACMD5
    {

        #region Files

        /// <summary>
        /// Computes a keyed hash for a source file, creates a target file with the keyed hash
        /// prepended to the contents of the source file, then decrypts the file and compares
        /// the source and the decrypted files.
        /// </summary>
        /// <param name="key">The key to use to encode the file</param>
        /// <param name="sourceFile">The file to encrypt complete path</param>
        /// <param name="destFile">Destination file complete path. If the file doesn't exist, it creates it</param>
        public void EncodeFile(string key, String sourceFile, String destFile)
        {
            if (string.IsNullOrWhiteSpace(sourceFile) || !File.Exists(sourceFile))
                throw new FileNotFoundException("Cannot find the specified source file", sourceFile ?? "null");

            if (string.IsNullOrWhiteSpace(destFile))
                throw new ArgumentException("Please specify the path of the output path", "destFile");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Please specify the key", "key");

            // Create a key using a random number generator. This would be the
            //  secret key shared by sender and receiver.
            byte[] secretkey = Utils.StrToByteArray(key);
            //RNGCryptoServiceProvider is an implementation of a random number generator.
            //RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            // The array is now filled with cryptographically strong random bytes.
            //rng.GetBytes(secretkey);

            // Initialize the keyed hash object.
            HMACMD5 myhmacMD5 = new HMACMD5(secretkey);
            FileStream inStream = new FileStream(sourceFile, FileMode.Open);
            FileStream outStream = new FileStream(destFile, FileMode.Create);
            // Compute the hash of the input file.
            byte[] hashValue = myhmacMD5.ComputeHash(inStream);
            // Reset inStream to the beginning of the file.
            inStream.Position = 0;
            // Write the computed hash value to the output file.
            outStream.Write(hashValue, 0, hashValue.Length);
            // Copy the contents of the sourceFile to the destFile.
            int bytesRead;
            // read 1K at a time
            byte[] buffer = new byte[1024];
            do
            {
                // Read from the wrapping CryptoStream.
                bytesRead = inStream.Read(buffer, 0, 1024);
                outStream.Write(buffer, 0, bytesRead);
            } while (bytesRead > 0);
            myhmacMD5.Clear();
            // Close the streams
            inStream.Close();
            outStream.Close();
        } // end EncodeFile

        /// <summary>
        /// Decrypt the encoded file and compare to original file. It returns false if the file is corrupted.
        /// </summary>
        /// <param name="key">The key used to encode the file</param>
        /// <param name="sourceFile">The file to decrypt complete path</param>
        /// <param name="destFile">Destination file complete path. If the file doesn't exist, it creates it</param>
        /// <returns></returns>
        public bool DecodeFile(string key, String sourceFile, String destFile)
        {
            if (string.IsNullOrWhiteSpace(sourceFile) || !File.Exists(sourceFile))
                throw new FileNotFoundException("Cannot find the specified source file", sourceFile ?? "null");

            if (string.IsNullOrWhiteSpace(destFile))
                throw new ArgumentException("Please specify the path of the output path", "destFile");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Please specify the key", "key");

            // Create a key using a random number generator. This would be the
            //  secret key shared by sender and receiver.
            byte[] secretkey = Utils.StrToByteArray(key);
            //RNGCryptoServiceProvider is an implementation of a random number generator.
            //RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            // The array is now filled with cryptographically strong random bytes.
            //rng.GetBytes(secretkey);

            // Initialize the keyed hash object. 
            HMACMD5 hmacMD5 = new HMACMD5(secretkey);
            // Create an array to hold the keyed hash value read from the file.
            byte[] storedHash = new byte[hmacMD5.HashSize / 8];
            // Create a FileStream for the source file.
            FileStream inStream = new FileStream(sourceFile, FileMode.Open);
            // Read in the storedHash.            
            inStream.Read(storedHash, 0, storedHash.Length);
            // Compute the hash of the remaining contents of the file.
            // The stream is properly positioned at the beginning of the content, 
            // immediately after the stored hash value.
            byte[] computedHash = hmacMD5.ComputeHash(inStream);
            // compare the computed hash with the stored value
            int i;
            for (i = 0; i < storedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i])
                {
                    inStream.Close();
                    return false;
                }

            }

            FileStream outStream = new FileStream(destFile, FileMode.Create);
            // Reset inStream to the beginning of the file.
            inStream.Position = i;
            // Copy the contents of the sourceFile to the destFile.
            int bytesRead;
            // read 1K at a time
            byte[] buffer = new byte[1024];
            do
            {
                // Read from the wrapping CryptoStream.
                bytesRead = inStream.Read(buffer, 0, 1024);
                outStream.Write(buffer, 0, bytesRead);
            } while (bytesRead > 0);
            // Close the streams
            inStream.Close();
            outStream.Close();
            return true;
        } //end DecodeFile

        #endregion               

    } 
}