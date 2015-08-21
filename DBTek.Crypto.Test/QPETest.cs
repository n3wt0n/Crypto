using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DBTek.Crypto.Test
{
    [TestClass]
    public class QPETest
    {
        QuotedPrintable qpe = new QuotedPrintable();

        #region String

        [TestMethod]
        public void QPEEncodeNullString()
        {
            Assert.AreEqual(string.Empty, qpe.EncodeString(null));
        }

        [TestMethod]
        public void QPEEncodeEmptyString()
        {
            Assert.AreEqual(string.Empty, qpe.EncodeString(string.Empty));
        }

        [TestMethod]
        public void QPEEncodeString()
        {
            Assert.AreEqual("Test =3D string =C8 3", qpe.EncodeString("Test = string È 3"));
        }

        [TestMethod]
        public void QPEDecodeNullString()
        {
            Assert.AreEqual(string.Empty, qpe.DecodeString(null));
        }

        [TestMethod]
        public void QPEDecodeEmptyString()
        {
            Assert.AreEqual(string.Empty, qpe.DecodeString(string.Empty));
        }

        [TestMethod]
        public void QPEDecodeString()
        {
            Assert.AreEqual("Test = string È 3", qpe.DecodeString("Test =3D string =C8 3"));
        }

        [TestMethod]
        public void QPEEncodeDecodeString()
        {
            string original = "Test string";
            string result = qpe.DecodeString(qpe.EncodeString(original));
            Assert.AreEqual(original, result);
        }

        #endregion

        #region File

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void QPEEncodeNullFromNullToFile()
        {
            qpe.EncodeFile(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void QPEEncodeNullFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            qpe.EncodeFile(null, destPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void QPEEncodeUnexistentFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            qpe.EncodeFile("unexistent path", destPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void QPEEncodeNullToFile()
        {
            var originalPath = FileUtils.createPlainFile();
            qpe.EncodeFile(originalPath, null);
            FileUtils.deleteFile(originalPath);
        }

        [TestMethod]
        public void QPEEncodeFile()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            qpe.EncodeFile(originalPath, destPath);
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void QPEDecodeNullFromNullToFile()
        {
            qpe.DecodeFile(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void QPEDecodeNullFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            qpe.DecodeFile(null, destPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void QPEDecodeUnexistentFromFile()
        {
            var destPath = FileUtils.createNewFilePath();
            qpe.DecodeFile("unexistent path", destPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void QPEDecodeNullToFile()
        {
            var originalPath = FileUtils.createPlainFile();
            qpe.DecodeFile(originalPath, null);
            FileUtils.deleteFile(originalPath);
        }

        [TestMethod]
        public void QPEDecodeFileExistingTo()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            qpe.DecodeFile(originalPath, destPath);
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        public void QPEDecodeFile()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            FileUtils.deleteFile(destPath);
            qpe.DecodeFile(originalPath, destPath);
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
        }

        [TestMethod]
        public void QPEEncodeDecodeFile()
        {
            var originalPath = FileUtils.createPlainFile();
            var destPath = FileUtils.createNewFilePath();
            var resultPath = FileUtils.createNewFilePath();
            FileUtils.deleteFile(destPath);
            FileUtils.deleteFile(resultPath);
            qpe.EncodeFile(originalPath, destPath);
            qpe.DecodeFile(destPath, resultPath);
            var originalContent = System.IO.File.ReadAllText(originalPath);
            var resultContent = System.IO.File.ReadAllText(resultPath);
            Assert.AreEqual(originalContent, resultContent);
            FileUtils.deleteFile(originalPath);
            FileUtils.deleteFile(destPath);
        }

        #endregion
    }
}
