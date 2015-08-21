﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DBTek.Crypto
{
    /// <summary>
    /// CRC32 hasher implementation
    /// </summary>
    public class CRC32_Hsr : IHasher
    {

        #region Strings

        /// <summary>
        /// Hash a string using CRC32
        /// </summary>
        /// <param name="sourceString">The string to hash</param>
        /// <returns>The hash</returns>
        public string HashString(string sourceString)
        {
            if (sourceString != null)
            {
                byte[] message = Encoding.UTF8.GetBytes(sourceString);
                var hashString = new SHA1Managed();
                string hex = "";
                foreach (byte x in HashBytes(message))
                    hex += Convert.ToString(x, 16).PadLeft(2, '0');
                return hex;
            }
            else
                return String.Empty;
        }

        #endregion

        #region Files

        /// <summary>
        /// Hash a file using CRC32
        /// </summary>
        /// <param name="sourceFile">The file to hash complete path</param>
        /// <returns>The hash</returns>
        public string HashFile(string sourceFile)
        {
            if (string.IsNullOrWhiteSpace(sourceFile) || !File.Exists(sourceFile))
                throw new FileNotFoundException("Cannot find the specified source file", sourceFile ?? "null");

            byte[] message = File.ReadAllBytes(sourceFile);
            var hashString = new SHA1Managed();
            string hex = "";
            foreach (byte x in HashBytes(message))
                hex += Convert.ToString(x, 16).PadLeft(2, '0');
            return hex;
        }

        #endregion

        #region Utils

        private byte[] HashBytes(byte[] input)
        {
            var crc = new Crc32();
            return crc.ComputeHash(input);
        }

        private class Crc32 : HashAlgorithm
        {
            public const UInt32 DefaultPolynomial = 0xedb88320;
            public const UInt32 DefaultSeed = 0xffffffff;

            private UInt32 hash;
            private UInt32 seed;
            private UInt32[] table;
            private static UInt32[] defaultTable;

            public Crc32()
            {
                table = InitializeTable(DefaultPolynomial);
                seed = DefaultSeed;
                Initialize();
            }

            public Crc32(UInt32 polynomial, UInt32 seed)
            {
                table = InitializeTable(polynomial);
                this.seed = seed;
                Initialize();
            }

            public override void Initialize()
                => hash = seed;

            protected override void HashCore(byte[] buffer, int start, int length)
                => hash = CalculateHash(table, hash, buffer, start, length);

            protected override byte[] HashFinal()
            {
                byte[] hashBuffer = UInt32ToBigEndianBytes(~hash);
                this.HashValue = hashBuffer;
                return hashBuffer;
            }

            public override int HashSize
                => 32;

            public static UInt32 Compute(byte[] buffer)
                => ~CalculateHash(InitializeTable(DefaultPolynomial), DefaultSeed, buffer, 0, buffer.Length);

            public static UInt32 Compute(UInt32 seed, byte[] buffer)
                => ~CalculateHash(InitializeTable(DefaultPolynomial), seed, buffer, 0, buffer.Length);

            public static UInt32 Compute(UInt32 polynomial, UInt32 seed, byte[] buffer)
                => ~CalculateHash(InitializeTable(polynomial), seed, buffer, 0, buffer.Length);

            private static UInt32[] InitializeTable(UInt32 polynomial)
            {
                if (polynomial == DefaultPolynomial && defaultTable != null)
                    return defaultTable;

                UInt32[] createTable = new UInt32[256];
                for (int i = 0; i < 256; i++)
                {
                    UInt32 entry = (UInt32)i;
                    for (int j = 0; j < 8; j++)
                        if ((entry & 1) == 1)
                            entry = (entry >> 1) ^ polynomial;
                        else
                            entry = entry >> 1;
                    createTable[i] = entry;
                }

                if (polynomial == DefaultPolynomial)
                    defaultTable = createTable;

                return createTable;
            }

            private static UInt32 CalculateHash(UInt32[] table, UInt32 seed, byte[] buffer, int start, int size)
            {
                UInt32 crc = seed;
                for (int i = start; i < size; i++)
                    unchecked
                    {
                        crc = (crc >> 8) ^ table[buffer[i] ^ crc & 0xff];
                    }
                return crc;
            }

            private byte[] UInt32ToBigEndianBytes(UInt32 x)
            {
                return new byte[]
                {
                    (byte)((x >> 24) & 0xff),
                    (byte)((x >> 16) & 0xff),
                    (byte)((x >> 8) & 0xff),
                    (byte)(x & 0xff)
                };
            }
        }
        #endregion
    }
}