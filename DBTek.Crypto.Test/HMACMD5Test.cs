using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DBTek.Crypto.Test
{
    [TestClass]
    public class HMACMD5Test
    {
        cHMACMD5 hmac = new cHMACMD5();
        string key = "chiave";

        #region File

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void HMACMD5EncodeNullFromNullToFile()
        {
           hmac.EncodeFile(key, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void HMACMD5EncodeNullFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            hmac.EncodeFile(key, null, destPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HMACMD5EncodeNullToFile()
        {
            var originalPath = FileUtils.createPlainFile();
            hmac.EncodeFile(key, originalPath, null);
            FileUtils.deleteFile(originalPath);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void HMACMD5EncodeUnexistentFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            hmac.EncodeFile(key, "unexistent path", destPath);
            FileUtils.deleteFile(destPath);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HMACMD5EncodeFileNullKey()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            hmac.EncodeFile(null, originalPath, destPath);
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HMACMD5EncodeFileEmptyKey()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            hmac.EncodeFile(String.Empty, originalPath, destPath);
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        public void HMACMD5EncodeFile()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            hmac.EncodeFile(key, originalPath, destPath);
            Assert.IsTrue(File.Exists(destPath));
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void HMACMD5DecodeNullFromNullToFile()
        {
            Assert.IsFalse(hmac.DecodeFile(key, null, null));
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void HMACMD5DecodeNullFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            Assert.IsFalse(hmac.DecodeFile(key, null, destPath));
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HMACMD5DecodeNullToFile()
        {
            var originalPath = FileUtils.createPlainFile();
            Assert.IsFalse(hmac.DecodeFile(key, originalPath, null));
            FileUtils.deleteFile(originalPath);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void HMACMD5DecodeUnexistentFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            Assert.IsFalse(hmac.DecodeFile(key, "unexistent path", destPath));
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HMACMD5DecodeFileNullKey()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            FileUtils.deleteFile(destPath);
            Assert.IsFalse(hmac.DecodeFile(null, originalPath, destPath));
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HMACMD5DecodeFileEmptyKey()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            FileUtils.deleteFile(destPath);
            Assert.IsFalse(hmac.DecodeFile(String.Empty, originalPath, destPath));
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
        }
       
        [TestMethod]
        public void HMACMD5EncodeDecodeFile()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            var resultPath = FileUtils.createNewFilePath();
            FileUtils.deleteFile(destPath);
            FileUtils.deleteFile(resultPath);
            hmac.EncodeFile(key, originalPath, destPath);
            Assert.IsTrue(File.Exists(destPath));
            Assert.IsTrue(hmac.DecodeFile(key, destPath, resultPath));
            var originalContent = System.IO.File.ReadAllText(originalPath);
            var resultContent = System.IO.File.ReadAllText(resultPath);
            Assert.AreEqual(originalContent, resultContent);
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
        }

        #endregion

    }
}
