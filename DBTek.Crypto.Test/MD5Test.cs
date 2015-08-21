using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DBTek.Crypto.Test
{
    [TestClass]
    public class MD5Test
    {
        MD5_Hsr md5 = new MD5_Hsr();

        [TestMethod]
        public void MD5HashNullString()
        {
            Assert.AreEqual(string.Empty, md5.HashString(null));
        }

        [TestMethod]
        public void MD5HashEmptyString()
        {
            Assert.AreEqual("d41d8cd98f00b204e9800998ecf8427e", md5.HashString(string.Empty));
        }

        [TestMethod]
        public void MD5HashString()
        {
            Assert.AreEqual("0fd3dbec9730101bff92acc820befc34", md5.HashString("Test string"));
        }

        [TestMethod]
        public void MD5HashStringComplex()
        {
            var complexString = "In C++ code we just declare strings as wchar_t (wide char?) instead of char and use the wcs functions instead of the str functions (for example wcscat and wcslen instead of strcat and strlen). To create a literal UCS-2 string in C code you just put an L before it as so:";
            var expected = "386b47ade851ac7df0aaa40e91e4d3e9";
            Assert.AreEqual(expected, md5.HashString(complexString));
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void MD5HashNullFile()
        {
            md5.HashFile(null);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void MD5HashEmptyPathFile()
        {
            md5.HashFile(String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void MD5HashUnexistentFile()
        {
            var path = "Not a real path";
            md5.HashFile(path);
        }

        [TestMethod]
        public void MD5HashEmptyFile()
        {
            var path = FileUtils.createNewFilePath();
            Assert.AreEqual("d41d8cd98f00b204e9800998ecf8427e", md5.HashFile(path));
            FileUtils.deleteFile(path);
        }        
    }
}
