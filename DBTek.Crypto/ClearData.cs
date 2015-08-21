using System;

namespace DBTek.Crypto
{
    class ClearData : IEncoder, IHasher
    {
        public string EncodeString(string sourceString) { return sourceString; }

        public string DecodeString(string sourceString) { return sourceString; }

        public void EncodeFile(String sourceFile, String destFile)
        {
            throw new NotImplementedException();
        }

        public void DecodeFile(String sourceFile, String destFileT)
        {
            throw new NotImplementedException();
        }

        public string HashString(string sourceString)
        {
            return sourceString;
        }

        public string HashFile(string sourceFile)
        {
            return sourceFile;
        }
    }
}