using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DBTek.Crypto.Test
{
    /// <summary>
    /// Descrizione del riepilogo per UnixCryptTest
    /// </summary>
    [TestClass]
    public class UnixCryptTest
    {
        UnixCrypt uc = new UnixCrypt();

        private string salt = "mySaltString";

        #region "MD5 String"
        [TestMethod]        
        public void UnixCryptMD5HashNullString()
        {
           Assert.AreEqual(string.Empty, uc.HashString(null, UnixCryptTypes.MD5));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UnixCryptMD5HashNullSalt()
        {
            uc.HashString("abc", null, UnixCryptTypes.MD5);
        }

        [TestMethod]
        public void UnixCryptMD5HashEmptyString()
        {
            Assert.AreEqual("$1$oQVN1aHM$tnA1FqCi76jbTsHHuaLV/1", uc.HashString(string.Empty, "oQVN1aHMEGLMELor", UnixCryptTypes.MD5));
        }

        [TestMethod]
        public void UnixCryptMD5HashString()
        {
            Assert.AreEqual("$1$J7GD5M88$kpCzP4IJ6h.DzPJPqCgW90", uc.HashString("Test string", "J7GD5M88GVDA3965", UnixCryptTypes.MD5));
        }
        #endregion

        #region "SHA256 String"
        [TestMethod]
        public void UnixCryptSHA256HashNullString()
        {
            Assert.AreEqual(string.Empty, uc.HashString(null, UnixCryptTypes.SHA2_256));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UnixCryptSHA256HashNullSalt()
        {
            uc.HashString("abc", null, UnixCryptTypes.SHA2_256);
        }

        [TestMethod]
        public void UnixCryptSHA256HashEmptyString()
        {
            Assert.AreEqual("$5$oQVN1aHMEGLMELor$CkNW/OTPEgZOAAZiLQ1k8zyVKcsUZDvlnUKNB2JFiI/", uc.HashString(string.Empty, "oQVN1aHMEGLMELor", UnixCryptTypes.SHA2_256));
        }

        [TestMethod]
        public void UnixCryptSHA256HashString()
        {
            Assert.AreEqual("$5$J7GD5M88GVDA3965$zxGLuBD0tp3pHnFeCwZeAvwgj5su8QngEN4qUO5Sv/0", uc.HashString("Test string", "J7GD5M88GVDA3965", UnixCryptTypes.SHA2_256));
        }
        #endregion

        #region "SHA512 String"
        [TestMethod]
        public void UnixCryptSHA512HashNullString()
        {
            Assert.AreEqual(string.Empty, uc.HashString(null, UnixCryptTypes.SHA2_512));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UnixCryptSHA512HashNullSalt()
        {
            uc.HashString("abc", null, UnixCryptTypes.SHA2_512);
        }

        [TestMethod]
        public void UnixCryptSHA512HashEmptyString()
        {
            Assert.AreEqual("$6$oQVN1aHMEGLMELor$cibFgwuWm/tLb/lthVd03ivqJWkqR4fbcUSCeRk4LLPFP2roT.eaDRZxuHTg6AOXSMXpi5ocgNibbu7rCmcW60", uc.HashString(string.Empty, "oQVN1aHMEGLMELor", UnixCryptTypes.SHA2_512));
        }

        [TestMethod]
        public void UnixCryptSHA512HashString()
        {
            Assert.AreEqual("$6$J7GD5M88GVDA3965$5rvrdrGDxnW3RxkNIbnGiNUbS/63TWEW6z2C25S5ISgbHuCTx0W5JbmpZYxhX8Y2ZJ.8KeOlNRQXoXnVrIXPc/", uc.HashString("Test string", "J7GD5M88GVDA3965", UnixCryptTypes.SHA2_512));
        }
#endregion

        #region MD5 File

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void UnixCryptMD5HashFileNullFile()
        {
            uc.HashFile(null, salt, UnixCryptTypes.MD5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UnixCryptMD5HashFileNullSalt()
        {
            var path = FileUtils.createNewFilePath();
            uc.HashString("abc", null, UnixCryptTypes.MD5);
            FileUtils.deleteFile(path);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void UnixCryptMD5HashFileEmptyPathFile()
        {
            uc.HashFile(String.Empty, UnixCryptTypes.MD5);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void UnixCryptMD5HashFileUnexistentFile()
        {
            var path = "Not a real path";
            uc.HashFile(path, UnixCryptTypes.MD5);
        }

        [TestMethod]
        public void UnixCryptMD5HashFile()
        {
            var path = FileUtils.createNewFilePath();
            var expected = "$1$mySaltSt$lJJ5HziKyl7Mz6OGsFi320";
            Assert.AreEqual(expected, uc.HashFile(path, salt, UnixCryptTypes.MD5));
            FileUtils.deleteFile(path);
        }

        #endregion

        #region SHA256 File

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void UnixCryptSHA256HashFileNullFile()
        {
            uc.HashFile(null, salt, UnixCryptTypes.SHA2_256);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UnixCryptSHA256HashFileNullSalt()
        {
            var path = FileUtils.createNewFilePath();
            uc.HashString("abc", null, UnixCryptTypes.SHA2_256);
            FileUtils.deleteFile(path);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void UnixCryptSHA256HashFileEmptyPathFile()
        {
            uc.HashFile(String.Empty, UnixCryptTypes.SHA2_256);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void UnixCryptSHA256HashFileUnexistentFile()
        {
            var path = "Not a real path";
            uc.HashFile(path, UnixCryptTypes.SHA2_256);
        }

        [TestMethod]
        public void UnixCryptSHA256HashFile()
        {
            var path = FileUtils.createNewFilePath();
            var expected = "$5$mySaltString$94SoOgqkxoHZnEZsAD.GTvL27F3XvkURbaoODmls6W/";
            Assert.AreEqual(expected, uc.HashFile(path, salt, UnixCryptTypes.SHA2_256));
            FileUtils.deleteFile(path);
        }

        #endregion

        #region SHA512 File

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void UnixCryptSHA512HashFileNullFile()
        {
            uc.HashFile(null, salt, UnixCryptTypes.SHA2_512);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UnixCryptSHA512HashFileNullSalt()
        {
            var path = FileUtils.createNewFilePath();
            uc.HashString("abc", null, UnixCryptTypes.SHA2_512);
            FileUtils.deleteFile(path);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void UnixCryptSHA512HashFileEmptyPathFile()
        {
            uc.HashFile(String.Empty, UnixCryptTypes.SHA2_512);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void UnixCryptSHA512HashFileUnexistentFile()
        {
            var path = "Not a real path";
            uc.HashFile(path, UnixCryptTypes.SHA2_512);
        }

        [TestMethod]
        public void UnixCryptSHA512HashFile()
        {
            var path = FileUtils.createNewFilePath();
            var expected = "$6$mySaltString$u.WVM9McVRUp7ZTfSrY9p0JMIhUI/72d2YZalZPfUmiKBaypDDl1tcIGeebxmantza0RHvf/z0BXGblAfbDux0";
            Assert.AreEqual(expected, uc.HashFile(path, salt, UnixCryptTypes.SHA2_512));
            FileUtils.deleteFile(path);
        }

        #endregion
    }
}
