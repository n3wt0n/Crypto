using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DBTek.Crypto.Test
{
    [TestClass]
    public class RijndaelTest
    {
        Rijndael rij = new Rijndael();

        #region String

        [TestMethod]
        public void RijndaelEncodeNullString()
        {
            Assert.AreEqual(string.Empty, rij.EncodeString(null));
        }

        [TestMethod]
        public void RijndaelEncodeEmptyString()
        {
            Assert.AreEqual(string.Empty, rij.EncodeString(string.Empty));
        }

        [TestMethod]
        public void RijndaelEncodeString()
        {
            Assert.AreEqual("PQxx/5VfZ3eCgPXwI3kB9w==", rij.EncodeString("Test string"));
        }

        [TestMethod]
        public void RijndaelDecodeNullString()
        {
            Assert.AreEqual(string.Empty, rij.DecodeString(null));
        }

        [TestMethod]
        public void RijndaelDecodeEmptyString()
        {
            Assert.AreEqual(string.Empty, rij.DecodeString(string.Empty));
        }

        [TestMethod]
        public void RijndaelDecodeString()
        {
            Assert.AreEqual("Test string", rij.DecodeString("PQxx/5VfZ3eCgPXwI3kB9w=="));
        }

        [TestMethod]
        public void RijndaelEncodeDecodeString()
        {
            string original = "Test string che piu bello non si può";
            string result = rij.DecodeString(rij.EncodeString(original));
            Assert.AreEqual(original, result);
        }

        #endregion

        #region File

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void RijndaelEncodeNullFromNullToFile()
        {
            rij.EncodeFile(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void RijndaelEncodeNullFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            rij.EncodeFile(null, destPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void RijndaelEncodeUnexistentFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            rij.EncodeFile("unexistent path", destPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RijndaelEncodeNullToFile()
        {
            var originalPath = FileUtils.createPlainFile();
            rij.EncodeFile(originalPath, null);
            FileUtils.deleteFile(originalPath);
        }

        [TestMethod]
        public void RijndaelEncodeFile()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            rij.EncodeFile(originalPath, destPath);
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void RijndaelDecodeNullFromNullToFile()
        {
            rij.DecodeFile(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void RijndaelDecodeNullFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            rij.DecodeFile(null, destPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void RijndaelDecodeUnexistentFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            rij.DecodeFile("unexistent path", destPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RijndaelDecodeNullToFile()
        {
            var originalPath = FileUtils.createPlainFile();
            rij.DecodeFile(originalPath, null);
            FileUtils.deleteFile(originalPath);
        }
        
        [TestMethod]
        public void RijndaelEncodeDecodeFile()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            var resultPath = FileUtils.createNewFilePath();
            FileUtils.deleteFile(destPath);
            FileUtils.deleteFile(resultPath);
            rij.EncodeFile(originalPath, destPath);
            rij.DecodeFile(destPath, resultPath);
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
        public void RijndaelEncodeStringNullKeyNullIV()
        {
            Assert.IsNull(rij.EncodeString("Test string", null, null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RijndaelEncodeStringNullKey()
        {
            Assert.IsNull(rij.EncodeString("Test string", null, "xxx"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RijndaelEncodeStringNullIV()
        {
            Assert.IsNull(rij.EncodeString("Test string", "xxx", null));            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RijndaelEncodeStringEmptyKeyEmptyIV()
        {
            Assert.IsNotNull(rij.EncodeString("Test string", string.Empty, string.Empty));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RijndaelEncodeStringEmptyKey()
        {
            Assert.IsNotNull(rij.EncodeString("Test string", string.Empty, "xxx"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RijndaelEncodeStringEmptyIV()
        {
            Assert.IsNotNull(rij.EncodeString("Test string", "xxx", string.Empty));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RijndaelDecodeStringNullKeyNullIV()
        {
            Assert.IsNull(rij.DecodeString("GHC4g/RhBX8VWMN2FNx3AA==", null, null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RijndaelDecodeStringNullKey()
        {
            Assert.IsNull(rij.DecodeString("GHC4g/RhBX8VWMN2FNx3AA==", null, "xxx"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RijndaelDecodeStringNullIV()
        {
            Assert.IsNull(rij.DecodeString("GHC4g/RhBX8VWMN2FNx3AA==", "xxx", null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RijndaelDecodeStringEmptyKeyEmptyIV()
        {
            Assert.IsNotNull(rij.DecodeString("59HXqEQrn84WEM/HNKL5Aw==", string.Empty, string.Empty));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RijndaelDecodeStringEmptyKey()
        {
            Assert.IsNotNull(rij.DecodeString("a1vooIbu9UACI7aLpqy7cg==", string.Empty, "xxx"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RijndaelDecodeStringEmptyIV()
        {
            Assert.IsNotNull(rij.DecodeString("vqkne61mqaUknhHip8KC2g==", "xxx", string.Empty));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RijndaelEncodeDecodeFileNullKeyNullIV()
        {
            var originalPath = FileUtils.createPlainFile();                        
            var originalContent = System.IO.File.ReadAllText(originalPath);
            rij.EncodeFile(originalPath, null, null, null);
            rij.DecodeFile(originalPath, null, null, null);
            var resultContent = System.IO.File.ReadAllText(originalPath);
            Assert.AreEqual(originalContent, resultContent);
            FileUtils.deleteFile(originalPath);            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RijndaelEncodeDecodeFileNullKey()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            var destPath2 = FileUtils.createNewFilePath();
            var originalContent = System.IO.File.ReadAllText(originalPath);
            rij.EncodeFile(originalPath, destPath, null, "xxx");
            rij.DecodeFile(destPath, destPath2, null, "xxx");
            var resultContent = System.IO.File.ReadAllText(originalPath);
            Assert.AreEqual(originalContent, resultContent);
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
            FileUtils.deleteFile(destPath2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RijndaelEncodeDecodeFileNullIV()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            var destPath2 = FileUtils.createNewFilePath();
            var originalContent = System.IO.File.ReadAllText(originalPath);
            rij.EncodeFile(originalPath, destPath, "xxx", null);
            rij.DecodeFile(destPath, destPath2, "xxx", null);
            var resultContent = System.IO.File.ReadAllText(originalPath);
            Assert.AreEqual(originalContent, resultContent);
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
            FileUtils.deleteFile(destPath2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RijndaelEncodeDecodeFileEmptyKeyEmptyIV()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            var destPath2 = FileUtils.createNewFilePath();
            var originalContent = System.IO.File.ReadAllText(originalPath);
            rij.EncodeFile(originalPath, destPath, String.Empty, String.Empty);
            rij.DecodeFile(destPath, destPath2, String.Empty, String.Empty);
            var resultContent = System.IO.File.ReadAllText(originalPath);
            Assert.AreEqual(originalContent, resultContent);
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
            FileUtils.deleteFile(destPath2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RijndaelEncodeDecodeFileEmptyKey()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            var destPath2 = FileUtils.createNewFilePath();
            var originalContent = System.IO.File.ReadAllText(originalPath);
            rij.EncodeFile(originalPath, destPath, String.Empty, "xxx");
            rij.DecodeFile(destPath, destPath2, String.Empty, "xxx");
            var resultContent = System.IO.File.ReadAllText(originalPath);
            Assert.AreEqual(originalContent, resultContent);
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
            FileUtils.deleteFile(destPath2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RijndaelEncodeDecodeFileEmptyIV()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            var destPath2 = FileUtils.createNewFilePath();
            var originalContent = System.IO.File.ReadAllText(originalPath);
            rij.EncodeFile(originalPath, destPath, "xxx", String.Empty);
            rij.DecodeFile(destPath, destPath2, "xxx", String.Empty);
            var resultContent = System.IO.File.ReadAllText(originalPath);
            Assert.AreEqual(originalContent, resultContent);
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
            FileUtils.deleteFile(destPath2);
        }

        #endregion
    }
}
