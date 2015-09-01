﻿using System;

namespace DBTek.Crypto.Helpers
{
    internal class CRC32Helper : IDisposable
    {
        uint[] table;

        public CRC32Helper()
        {
            uint poly = 0xedb88320;
            table = new uint[256];
            uint temp = 0;
            for (uint i = 0; i < table.Length; ++i)
            {
                temp = i;
                for (int j = 8; j > 0; --j)
                {
                    if ((temp & 1) == 1)                    
                        temp = (uint)((temp >> 1) ^ poly);                    
                    else                    
                        temp >>= 1;                    
                }
                table[i] = temp;
            }
        }

        public uint ComputeChecksum(byte[] bytes)
        {
            uint crc = 0xffffffff;
            for (int i = 0; i < bytes.Length; ++i)
            {
                byte index = (byte)(((crc) & 0xff) ^ bytes[i]);
                crc = (uint)((crc >> 8) ^ table[index]);
            }
            return ~crc;
        }

        public byte[] ComputeChecksumAsBytes(byte[] bytes)
            => BitConverter.GetBytes(ComputeChecksum(bytes));        


        public string ComputeChecksumAsString(byte[] bytes)
        {
            byte[] message = ComputeChecksumAsBytes(bytes);
            string hex = "";
            Array.Reverse(message);
            foreach (byte x in message)
                hex += Convert.ToString(x, 16).PadLeft(2, '0');
            return hex;
        }

        public void Dispose()
        {
            table = null;
        }
    }
}
