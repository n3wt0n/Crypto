using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DBTek.Crypto.Test
{
    [TestClass]
    public class SHA1Test
    {
        SHA1_Hsr sha1 = new SHA1_Hsr();

        [TestMethod]
        public void SHA1HashNullString()
        {
            Assert.AreEqual(string.Empty, sha1.HashString(null));
        }

        [TestMethod]
        public void SHA1HashEmptyString()
        {
            Assert.AreEqual("da39a3ee5e6b4b0d3255bfef95601890afd80709", sha1.HashString(string.Empty));
        }

        [TestMethod]
        public void SHA1HashString()
        {
            Assert.AreEqual("18af819125b70879d36378431c4e8d9bfa6a2599", sha1.HashString("Test string"));
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void SHA1HashNullFile()
        {
            sha1.HashFile(null);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void SHA1HashEmptyPathFile()
        {
            sha1.HashFile(String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void SHA1HashUnexistentFile()
        {
            var path = "Not a real path";
            sha1.HashFile(path);
        }

        [TestMethod]
        public void SHA1HashEmptyFile()
        {
            var path = FileUtils.createNewFilePath();
            Assert.AreEqual("da39a3ee5e6b4b0d3255bfef95601890afd80709", sha1.HashFile(path).ToLower());
            FileUtils.deleteFile(path);
        }  
    }
}
