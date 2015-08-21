using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DBTek.Crypto.Test
{
    [TestClass]
    public class CRC32Test
    {
        CRC32_Hsr crc32 = new CRC32_Hsr();

        [TestMethod]
        public void CRC32HashNullString()
        {
            Assert.AreEqual(string.Empty, crc32.HashString(null));
        }

        [TestMethod]
        public void CRC32HashEmptyString()
        {
            Assert.AreEqual("00000000", crc32.HashString(string.Empty));
        }

        [TestMethod]
        public void CRC32HashString()
        {
            Assert.AreEqual("95db9a92", crc32.HashString("Test string"));
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CRC32HashNullFile()
        {
            crc32.HashFile(null);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CRC32HashEmptyPathFile()
        {
           crc32.HashFile(String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CRC32HashUnexistentFile()
        {
            var path = "Not a real path";
           crc32.HashFile(path);            
        }        

        [TestMethod]
        public void CRC32HashEmptyFile()
        {
            var path = FileUtils.createNewFilePath();
            Assert.AreEqual("00000000", crc32.HashFile(path));
            FileUtils.deleteFile(path);
        }        
    }
}
