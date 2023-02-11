using CarReader.Interfaces;

namespace CarReader.Readers
{
    public class ReaderFactory
    {
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
