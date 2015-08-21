using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DBTek.Crypto.Test
{
    [TestClass]
    public class Base64Test
    {
        Base64 b64 = new Base64();

        #region String
        
        [TestMethod]
        public void Base64EncodeNullString()
        {
            Assert.AreEqual(string.Empty, b64.EncodeString(null));
        }

        [TestMethod]
        public void Base64EncodeEmptyString()
        {
            Assert.AreEqual(string.Empty, b64.EncodeString(string.Empty));
        }

        [TestMethod]
        public void Base64EncodeString()
        {
            Assert.AreEqual("VGVzdCBzdHJpbmc=", b64.EncodeString("Test string"));
        }

        [TestMethod]
        public void Base64DecodeNullString()
        {
            Assert.AreEqual(string.Empty, b64.DecodeString(null));
        }

        [TestMethod]
        public void Base64DecodeEmptyString()
        {
            Assert.AreEqual(string.Empty, b64.DecodeString(string.Empty));
        }

        [TestMethod]
        public void Base64DecodeString()
        {
            Assert.AreEqual("Test string", b64.DecodeString("VGVzdCBzdHJpbmc="));
        }

        [TestMethod]
        public void Base64EncodeDecodeString()
        {
            string original = "Test string";
            string result = b64.DecodeString(b64.EncodeString(original));
            Assert.AreEqual(original,result);
        }

        #endregion

        #region File       

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Base64EncodeNullFromNullToFile()
        {
            b64.EncodeFile(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Base64EncodeNullFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            b64.EncodeFile(null, destPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Base64EncodeUnexistentFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            b64.EncodeFile("unexistent path", destPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Base64EncodeNullToFile()
        {
            var originalPath = FileUtils.createPlainFile();
            b64.EncodeFile(originalPath, null);
            FileUtils.deleteFile(originalPath);
        }

        [TestMethod]
        public void Base64EncodeFile()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            b64.EncodeFile(originalPath, destPath);
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Base64DecodeNullFromNullToFile()
        {
            b64.DecodeFile(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Base64DecodeNullFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            b64.DecodeFile(null, destPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Base64DecodeUnexistentFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            b64.DecodeFile("unexistent path", destPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Base64DecodeNullToFile()
        {
            var originalPath = FileUtils.createPlainFile();
            b64.DecodeFile(originalPath, null);
            FileUtils.deleteFile(originalPath);
        }

        [TestMethod]
        public void Base64DecodeFileExistingTo()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            b64.DecodeFile(originalPath, destPath);
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        public void Base64DecodeFile()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            FileUtils.deleteFile(destPath);
            b64.DecodeFile(originalPath, destPath);
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        public void Base64EncodeDecodeFile()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            var resultPath = FileUtils.createNewFilePath();
            FileUtils.deleteFile(destPath);
            FileUtils.deleteFile(resultPath);
            b64.EncodeFile(originalPath, destPath);
            b64.DecodeFile(destPath, resultPath);
            var originalContent = System.IO.File.ReadAllText(originalPath);
            var resultContent = System.IO.File.ReadAllText(resultPath);
            Assert.AreEqual(originalContent, resultContent);
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
        }

        #endregion
    }
}
