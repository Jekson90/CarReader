using CarReader.Interfaces;
using CarReader.Readers;
using System;
using System.IO;

namespace CarReaderTest.Helpers
{
    public class TestHelper
    {
        private const string _testDirectory = "Test";

        /// <summary>
        /// Gets path for testing directory. Removes need file if exist.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetPath(string fileName = null)
        {
            string path = Directory.GetCurrentDirectory();
            path = Path.Combine(path, _testDirectory);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (!string.IsNullOrEmpty(fileName))
            {
                path = Path.Combine(path, fileName);

                if (File.Exists(path))
                    File.Delete(path);
            }

            return path;
        }
    }
}
