using CarReader.Interfaces;

namespace CarReader.Readers
{
    public class ReaderFactory
    {
        /// <summary>
        /// Gives reader that corresponds needed type and create it for current file.
        /// </summary>
        /// <typeparam name="T">Type witch in the file. Type implements ICar.</typeparam>
        /// <param name="type">Type of reader.</param>
        /// <param name="path">File location.</param>
        /// <returns>Car reader.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static ICarReader<T> GetCarReader<T>(ReaderType type, string path) where T : ICar, new()
        {
            bool noPath = string.IsNullOrEmpty(path);
            return type switch
            {
                ReaderType.Car => noPath ? new BinaryCarReader<T>() : new BinaryCarReader<T>(path),
                ReaderType.Xml => noPath ? new XmlCarReader<T>() : new XmlCarReader<T>(path),
                _ => throw new ArgumentException("Unknow type.")
            };
        }
    }
}
