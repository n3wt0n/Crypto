using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DBTek.Crypto.Test
{
    [TestClass]
    public class TripleDESTest
    {
        TripleDES tDES = new TripleDES();

        #region String

        [TestMethod]
        public void TripleDESEncodeNullString()
        {
            Assert.IsNull(tDES.EncodeString(null));
        }

        [TestMethod]
        public void TripleDESEncodeEmptyString()
        {
            Assert.IsNull(tDES.EncodeString(string.Empty));
        }

        [TestMethod]
        public void TripleDESEncodeString()
        {
            Assert.AreEqual("U/Poe9Qf3VpIOkgMlODrOuAT1bLTFlWM", tDES.EncodeString("Test string"));
        }

        [TestMethod]
        public void TripleDESDecodeNullString()
        {
            Assert.IsNull(tDES.DecodeString(null));
        }

        [TestMethod]
        public void TripleDESDecodeEmptyString()
        {
            Assert.IsNull(tDES.DecodeString(string.Empty));
        }

        [TestMethod]
        public void TripleDESDecodeString()
        {
            Assert.AreEqual("Test string", tDES.DecodeString("U/Poe9Qf3VpIOkgMlODrOuAT1bLTFlWM"));
        }

        [TestMethod]
        public void TripleDESEncodeDecodeString()
        {
            string original = "Test string che piu bello non si può";
            string result = tDES.DecodeString(tDES.EncodeString(original));
            Assert.AreEqual(original, result);
        }

        #endregion

        #region File

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TripleDESEncodeNullFromNullToFile()
        {
            tDES.EncodeFile(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TripleDESEncodeNullFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            tDES.EncodeFile(null, destPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TripleDESEncodeUnexistentFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            tDES.EncodeFile("unexistent path", destPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TripleDESEncodeNullToFile()
        {
            var originalPath = FileUtils.createPlainFile();
            tDES.EncodeFile(originalPath, null);
            FileUtils.deleteFile(originalPath);
        }

        [TestMethod]
        public void TripleDESEncodeFile()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            tDES.EncodeFile(originalPath, destPath);
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TripleDESDecodeNullFromNullToFile()
        {
            tDES.DecodeFile(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TripleDESDecodeNullFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            tDES.DecodeFile(null, destPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TripleDESDecodeUnexistentFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            tDES.DecodeFile("unexistent path", destPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TripleDESDecodeNullToFile()
        {
            var originalPath = FileUtils.createPlainFile();
            tDES.DecodeFile(originalPath, null);
            FileUtils.deleteFile(originalPath);
        }

        [TestMethod]
        public void TripleDESEncodeDecodeFile()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            var resultPath = FileUtils.createNewFilePath();
            FileUtils.deleteFile(destPath);
            FileUtils.deleteFile(resultPath);
            tDES.EncodeFile(originalPath, destPath);
            tDES.DecodeFile(destPath, resultPath);
            var originalContent = System.IO.File.ReadAllText(originalPath);
            var resultContent = System.IO.File.ReadAllText(resultPath);
            Assert.AreEqual(originalContent, resultContent);
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
        }

        #endregion

        #region Manual key and IV
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TripleDESEncodeStringNullKeyNullIV()
        {
            tDES.EncodeString("Test string", null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TripleDESEncodeStringNullKey()
        {
            tDES.EncodeString("Test string", null, "xxx");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TripleDESEncodeStringNullIV()
        {
           tDES.EncodeString("Test string", "xxx", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TripleDESEncodeStringEmptyKeyEmptyIV()
        {
            tDES.EncodeString("Test string", string.Empty, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TripleDESEncodeStringEmptyKey()
        {
            tDES.EncodeString("Test string", string.Empty, "xxx");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TripleDESEncodeStringEmptyIV()
        {
            tDES.EncodeString("Test string", "xxx", string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TripleDESDecodeStringNullKeyNullIV()
        {
            tDES.DecodeString("CUIF5tHmbl1prgMPXMqPq8XP7Vn0jh9d", null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TripleDESDecodeStringNullKey()
        {
            tDES.DecodeString("CUIF5tHmbl1prgMPXMqPq8XP7Vn0jh9d", null, "xxx");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TripleDESDecodeStringNullIV()
        {
            tDES.DecodeString("CUIF5tHmbl1prgMPXMqPq8XP7Vn0jh9d", "xxx", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TripleDESDecodeStringEmptyKeyEmptyIV()
        {
            tDES.DecodeString("59HXqEQrn84WEM/HNKL5Aw==", string.Empty, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TripleDESDecodeStringEmptyKey()
        {
            tDES.DecodeString("a1vooIbu9UACI7aLpqy7cg==", string.Empty, "xxx");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TripleDESDecodeStringEmptyIV()
        {
            tDES.DecodeString("vqkne61mqaUknhHip8KC2g==", "xxx", string.Empty);
        }

        //[TestMethod]
        //public void TripleDESEncodeDecodeFileNullKeyNullIV()
        //{
        //    var originalPath = FileUtils.createPlainFile();
        //    var originalContent = System.IO.File.ReadAllText(originalPath);
        //    tDES.EncodeFile(originalPath, , null, null, true);
        //    tDES.DecodeFile(originalPath, null, null, true);
        //    var resultContent = System.IO.File.ReadAllText(originalPath);
        //    Assert.AreEqual(originalContent, resultContent);
        //    FileUtils.deleteFile(originalPath);
        //}

        //[TestMethod]
        //public void TripleDESEncodeDecodeFileNullKey()
        //{
        //    var originalPath = FileUtils.createPlainFile();
        //    var originalContent = System.IO.File.ReadAllText(originalPath);
        //    tDES.EncodeFile(originalPath, null, "xxx", true);
        //    tDES.DecodeFile(originalPath, null, "xxx", true);
        //    var resultContent = System.IO.File.ReadAllText(originalPath);
        //    Assert.AreEqual(originalContent, resultContent);
        //    FileUtils.deleteFile(originalPath);
        //}

        //[TestMethod]
        //public void TripleDESEncodeDecodeFileNullIV()
        //{
        //    var originalPath = FileUtils.createPlainFile();
        //    var originalContent = System.IO.File.ReadAllText(originalPath);
        //    tDES.EncodeFile(originalPath, "xxx", null, true);
        //    tDES.DecodeFile(originalPath, "xxx", null, true);
        //    var resultContent = System.IO.File.ReadAllText(originalPath);
        //    Assert.AreEqual(originalContent, resultContent);
        //    FileUtils.deleteFile(originalPath);
        //}

        //[TestMethod]
        //public void TripleDESEncodeDecodeFileEmptyKeyEmptyIV()
        //{
        //    var originalPath = FileUtils.createPlainFile();
        //    var originalContent = System.IO.File.ReadAllText(originalPath);
        //    tDES.EncodeFile(originalPath, String.Empty, String.Empty, true);
        //    tDES.DecodeFile(originalPath, String.Empty, String.Empty, true);
        //    var resultContent = System.IO.File.ReadAllText(originalPath);
        //    Assert.AreEqual(originalContent, resultContent);
        //    FileUtils.deleteFile(originalPath);
        //}

        //[TestMethod]
        //public void TripleDESEncodeDecodeFileEmptyKey()
        //{
        //    var originalPath = FileUtils.createPlainFile();
        //    var originalContent = System.IO.File.ReadAllText(originalPath);
        //    tDES.EncodeFile(originalPath, String.Empty, "xxx", true);
        //    tDES.DecodeFile(originalPath, String.Empty, "xxx", true);
        //    var resultContent = System.IO.File.ReadAllText(originalPath);
        //    Assert.AreEqual(originalContent, resultContent);
        //    FileUtils.deleteFile(originalPath);
        //}

        //[TestMethod]
        //public void TripleDESEncodeDecodeFileEmptyIV()
        //{
        //    var originalPath = FileUtils.createPlainFile();
        //    var originalContent = System.IO.File.ReadAllText(originalPath);
        //    tDES.EncodeFile(originalPath, "xxx", String.Empty, true);
        //    tDES.DecodeFile(originalPath, "xxx", String.Empty, true);
        //    var resultContent = System.IO.File.ReadAllText(originalPath);
        //    Assert.AreEqual(originalContent, resultContent);
        //    FileUtils.deleteFile(originalPath);
        //}
        #endregion
    }
}
