using System.Text;
using System;
using System.Security.Cryptography;
using DBTek.Crypto.Extensions;

namespace DBTek.Crypto.UnixCryptAlg
{
    internal class SplittedHash
    {
        public string Protocol { get; set; }
        public string Rounds { get; set; }
        public string Hash { get; set; }

        /// <summary>
        /// Actual salt param of the hash, does NOT include the protocol and rounds.
        /// </summary>
        public string Salt { get; set; }

        public static SplittedHash Parse(string str)
        {
            var sh = new SplittedHash();

            var ret = str.Split(new char[] { '$' }, 4, StringSplitOptions.RemoveEmptyEntries);
            if (ret.Length < 3)            
                throw new ArgumentException("Invalid MCF string");            

            sh.Protocol = ret[0];

            if (!ret[1].StartsWith("rounds="))
            {
                sh.Salt = ret[1];
                sh.Hash = ret[2];
            }
            else
            {
                sh.Rounds = ret[1];
                sh.Salt = ret[2];
                sh.Hash = ret[3];
            }

            return sh;
        }

        public string GetFullSalt()
        {
            if (string.IsNullOrEmpty(Rounds))            
                return $"${Protocol}${Salt}";            
            
            return $"${Protocol}${Rounds}${Salt}";
        }
    }

    internal class CryptUtils
    {
        public const string TypeMd5 = "$1$";
        public const string TypeSha256 = "$5$";
        public const string TypeSha512 = "$6$";

        public const string DefaultType = TypeSha512;

        public static string Crypt(string password, string salt)
        {
            var keyPtr = new ArrayPointer<byte>((password + "\0").ToByteArray());
            var saltPtr = new ArrayPointer<byte>((salt + "\0").ToByteArray());

            /* Try to find out whether we have to use MD5 encryption replacement.  */
            if (CryptImpl.strncmp(CryptImpl.md5_salt_prefix, saltPtr, CryptImpl.strlen(CryptImpl.md5_salt_prefix)) == 0)            
                return CryptImpl.CryptMd5(keyPtr, saltPtr);            

            /* Try to find out whether we have to use SHA256 encryption replacement.  */
            if (CryptImpl.strncmp(CryptImpl.sha256_salt_prefix, saltPtr, CryptImpl.strlen(CryptImpl.sha256_salt_prefix)) == 0)            
                return CryptImpl.CryptSha256(keyPtr, saltPtr);            

            /* Try to find out whether we have to use SHA512 encryption replacement.  */
            if (CryptImpl.strncmp(CryptImpl.sha512_salt_prefix, saltPtr, CryptImpl.strlen(CryptImpl.sha512_salt_prefix)) == 0)            
                return CryptImpl.CryptSha512(keyPtr, saltPtr);            

            throw new ArgumentException("Unsupported algorithm");
        }

        public static bool Verify(string hash, string password)
        {
            var sh = SplittedHash.Parse(hash);
            string salt = sh.GetFullSalt();

            // Has the password with the salt from the hash
            string newHash = Crypt(password, salt);

            return hash == newHash;
        }

        public static string MakeSalt()
            => MakeSalt(DefaultType);        

        private static int GetRounds()
            => new Random().Next(10000, 60000);        

        public static string MakeSalt(string algoType)
        {
            int saltChars = 16;
            if (algoType == TypeMd5)            
                saltChars = 8;            

            // Find out how many random bytes we need for the saltChars as
            // base64 has overhead of 4/3
            double base64Overhead = 4.0 / 3.0;

            int bytesNeeded = (int)((double)saltChars / base64Overhead);

            byte[] randomBytes = new byte[bytesNeeded];

            var random = new RNGCryptoServiceProvider();
            random.GetNonZeroBytes(randomBytes);

            return $"{algoType}rounds={ GetRounds()}${Convert.ToBase64String(randomBytes)}";
        }
    }
}
