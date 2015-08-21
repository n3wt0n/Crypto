using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTek.Crypto.Test
{
    public static class FileUtils
    {
        public static string createPlainFile()
        {
            string path = System.IO.Path.GetTempFileName();
            System.IO.File.AppendAllText(path, "Test string on file");
            return path;
        }

        public static void deleteFile(string path)
        {
            System.IO.File.Delete(path);
        }

        public static string createNewFilePath()
        {
            string path = System.IO.Path.GetTempFileName();
            return path;
        }
    }
}
