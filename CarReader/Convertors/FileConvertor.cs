using CarReader.Interfaces;
using CarReader.Readers;

namespace CarReader.Convertors
{
    public class FileConvertor
    {
        /// <summary>
        /// Converts data of T object from source file to destination file.
        /// </summary>
        /// <typeparam name="T">Object type wich needs to convert.</typeparam>
        /// <param name="sourcePath">Path of source file.</param>
        /// <param name="destinationPath">Path of destination file.</param>
        /// <exception cref="ArgumentException"></exception>
        public static void Convert<T>(string sourcePath, string destinationPath) where T : ICar, new()
        {
            //using Generics and Interfaces makes easy using of type and classes possible
            //while converting two different types
            var sourceType = GetReaderTypeFromExtension<T>(sourcePath);
            var destinationType = GetReaderTypeFromExtension<T>(destinationPath);

            if (sourceType == null || destinationType == null)
                throw new ArgumentException("Unsupported type.");

            if (sourceType == destinationType)
                throw new ArgumentException("Convert types are equals.");

            var sourceReader = ReaderFactory.GetCarReader<T>(sourceType, sourcePath);
            var destinationReader = ReaderFactory.GetCarReader<T>(destinationType, destinationPath);

            var sourceCars = sourceReader.GetCars();
            var destinationCars = destinationReader.GetCars();

            if (destinationCars.Any())
                destinationReader.RemoveCars(destinationCars);

            destinationReader.AddCars(sourceCars);
        }

        private static ReaderType GetReaderTypeFromExtension<T>(string filePath) where T : ICar, new()
        {
            var types = Enum.GetNames(typeof(ReaderType)).Select(x => x.ToLower()).ToList();
            string extesion = Path.GetExtension(filePath).ToLower().Replace(".","");
            int index = types.IndexOf(extesion);
            return (ReaderType)index;
        }
    }
}
