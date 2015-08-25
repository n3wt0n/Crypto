﻿using System;
using System.Text;

namespace DBTek.Crypto.Helpers
{
    /// <summary>
    /// Computes a Hash-based Message Authentication Code (HMAC) using the <see cref="T:xBrainLab.Security.Cryptography.MD5" /> hash function
    /// </summary>
    internal class HMACMD5
    {
        private const int BLOCK_SIZE = 64;

        private byte[] m_Key = null;
        private byte[] m_inner = null;
        private byte[] m_outer = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="HMACMD5"/> class using the supplied key with UT8 encoding.
        /// </summary>
        /// <param name="key">The key.</param>
        public HMACMD5(string key)
            : this(key, Encoding.UTF8)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HMACMD5"/> class using the supplied key with supplied encoding.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="encoding">The encoding used to read the key.</param>
        public HMACMD5(string key, Encoding encoding)
            : this(encoding.GetBytes(key))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HMACMD5"/> class the supplied key.
        /// </summary>
        /// <param name="key">The key.</param>
        public HMACMD5(byte[] key)
        {
            this.InitializeKey(key);
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public byte[] Key
        {
            get
            {
                return this.m_Key;
            }
            set
            {
                this.InitializeKey(value);
            }
        }

        /// <summary>
        /// Computes the hash value for the specified string (UTF8 default encoding).
        /// </summary>
        /// <param name="buffer">The input to compute the hash code for. </param>
        /// <returns>The computed hash code</returns>
        public byte[] ComputeHash(string buffer)
            => this.ComputeHash(buffer, Encoding.UTF8);        

        /// <summary>
        /// Computes the hash value for the specified string.
        /// </summary>
        /// <param name="buffer">The input to compute the hash code for.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// The computed hash code
        /// </returns>
        public byte[] ComputeHash(string buffer, Encoding encoding)
            => this.ComputeHash(encoding.GetBytes(buffer));        

        /// <summary>
        /// Computes the hash value for the specified byte array.
        /// </summary>
        /// <param name="buffer">The input to compute the hash code for.</param>
        /// <returns>
        /// The computed hash code
        /// </returns>
        public byte[] ComputeHash(byte[] buffer)
        {
            if (buffer == null)            
                throw new ArgumentNullException(nameof(buffer), "The input cannot be null.");
            
            return MD5.GetHash(this.Combine(this.m_outer, MD5.GetHash(this.Combine(this.m_inner, buffer))));
        }

        /// <summary>
        /// Computes the hash for the specified string (UTF8 default encoding) to base64 string.
        /// </summary>
        /// <param name="buffer">The input to compute the hash code for.</param>
        /// <returns>The computed hash code in base64 string</returns>
        public string ComputeHashToBase64String(string buffer)
            => Convert.ToBase64String(this.ComputeHash(buffer, Encoding.UTF8));        

        /// <summary>
        /// Computes the hash for the specified string to base64 string.
        /// </summary>
        /// <param name="buffer">The input to compute the hash code for.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// The computed hash code in base64 string
        /// </returns>
        public string ComputeHashToBase64String(string buffer, Encoding encoding)
            => Convert.ToBase64String(this.ComputeHash(buffer, encoding));        

        /// <summary>
        /// Initializes the key.
        /// </summary>
        /// <param name="key">The key.</param>
        private void InitializeKey(byte[] key)
        {
            if (key == null)            
                throw new ArgumentNullException(nameof(key), "The Key cannot be null.");            

            if (key.Length > BLOCK_SIZE)            
                this.m_Key = MD5.GetHash(key);            
            else            
                this.m_Key = key;            

            this.UpdateIOPadBuffers();
        }

        /// <summary>
        /// Updates the IO pad buffers.
        /// </summary>
        private void UpdateIOPadBuffers()
        {
            if (this.m_inner == null)            
                this.m_inner = new byte[BLOCK_SIZE];            

            if (this.m_outer == null)            
                this.m_outer = new byte[BLOCK_SIZE];            

            for (int i = 0; i < BLOCK_SIZE; i++)
            {
                this.m_inner[i] = 54;
                this.m_outer[i] = 92;
            }

            for (int i = 0; i < this.Key.Length; i++)
            {
                byte[] s1 = this.m_inner;
                int s2 = i;
                s1[s2] ^= this.Key[i];
                byte[] s3 = this.m_outer;
                int s4 = i;
                s3[s4] ^= this.Key[i];
            }
        }

        /// <summary>
        /// Combines two array (a1 and a2).
        /// </summary>
        /// <param name="a1">The Array 1.</param>
        /// <param name="a2">The Array 2.</param>
        /// <returns>Combinaison of a1 and a2</returns>
        private byte[] Combine(byte[] a1, byte[] a2)
        {
            byte[] final = new byte[a1.Length + a2.Length];
            for (int i = 0; i < a1.Length; i++)            
                final[i] = a1[i];            

            for (int i = 0; i < a2.Length; i++)            
                final[i + a1.Length] = a2[i];            

            return final;
        }
    }
}