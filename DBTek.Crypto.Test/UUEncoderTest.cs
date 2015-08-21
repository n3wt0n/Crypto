using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DBTek.Crypto.Test
{
    [TestClass]
    public class UUEncoderTest
    {
        UUEncoder uu = new UUEncoder();

        #region String

        [TestMethod]
        public void UUEncodeNullString()
        {
            Assert.AreEqual("`", uu.EncodeString(null));
        }

        [TestMethod]
        public void UUEncodeEmptyString()
        {
            Assert.AreEqual("`", uu.EncodeString(string.Empty));
        }

        [TestMethod]
        public void UUEncodeString()
        {
            Assert.AreEqual("+5&5S=\"!S=')I;F< ", uu.EncodeString("Test string"));
        }

        [TestMethod]
        public void UUDecodeNullString()
        {
            Assert.AreEqual(string.Empty, uu.DecodeString(null));
        }

        [TestMethod]
        public void UUDecodeEmptyString()
        {
            Assert.AreEqual(string.Empty, uu.DecodeString(string.Empty));
        }

        [TestMethod]
        public void UUDecodeString()
        {
            Assert.AreEqual("Test string", uu.DecodeString("+5&5S=\"!S=')I;F< "));
        }

        [TestMethod]
        public void UUEncodeDecodeString()
        {
            string original = "Test string";
            string result = uu.DecodeString(uu.EncodeString(original));
            Assert.AreEqual(original, result);
        }

        #endregion

        #region File

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void UUEncodeNullFromNullToFile()
        {
            uu.EncodeFile(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void UUEncodeNullFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            uu.EncodeFile(null, destPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void UUEncodeUnexistentFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            uu.EncodeFile("unexistent path", destPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UUEncodeNullToFile()
        {
            var originalPath = FileUtils.createPlainFile();
            uu.EncodeFile(originalPath, null);
            FileUtils.deleteFile(originalPath);
        }

        [TestMethod]
        public void UUEncodeFile()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            uu.EncodeFile(originalPath, destPath);
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void UUDecodeNullFromNullToFile()
        {
            uu.DecodeFile(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void UUDecodeNullFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            uu.DecodeFile(null, destPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void UUDecodeUnexistentFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            uu.DecodeFile("unexistent path", destPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UUDecodeNullToFile()
        {
            var originalPath = FileUtils.createPlainFile();
            uu.DecodeFile(originalPath, null);
            FileUtils.deleteFile(originalPath);
        }

        [TestMethod]
        public void UUDecodeFileExistingTo()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            uu.DecodeFile(originalPath, destPath);
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        public void UUDecodeFile()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            FileUtils.deleteFile(destPath);
            uu.DecodeFile(originalPath, destPath);
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        public void UUEncodeDecodeFile()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            var resultPath = FileUtils.createNewFilePath();
            FileUtils.deleteFile(destPath);
            FileUtils.deleteFile(resultPath);
            uu.EncodeFile(originalPath, destPath);
            uu.DecodeFile(destPath, resultPath);
            var originalContent = System.IO.File.ReadAllText(originalPath);
            var resultContent = System.IO.File.ReadAllText(resultPath);
            Assert.AreEqual(originalContent, resultContent);
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
        }

        #endregion
    }
}
