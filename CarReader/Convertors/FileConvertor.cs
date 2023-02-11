using CarReader.Interfaces;
using CarReader.Readers;

namespace CarReader.Convertors
{
    public class FileConvertor
    {
        private string _sourcePath;
        private string _destinationPath;

        #region Ctor
        public FileConvertor()
        {

        }

        public FileConvertor(string sourcePath, string destinationPath)
        {
            _sourcePath = sourcePath;
            _destinationPath = destinationPath;
        } 
        #endregion

        public void Convert<T>() where T : ICar, new()
        {
            var sourceType = GetReaderTypeFromExtension<T>(_sourcePath);
            var destinationType = GetReaderTypeFromExtension<T>(_destinationPath);

            if (sourceType == destinationType)
                throw new ArgumentException("Convert types are equals.");

            var sourceReader = ReaderFactory.GetCarReader<T>(sourceType, _sourcePath);
            var destinationReader = ReaderFactory.GetCarReader<T>(destinationType, _destinationPath);

            var sourceCars = sourceReader.GetCars();
            var destinationCars = destinationReader.GetCars();

            if (destinationCars.Any())
                destinationReader.RemoveCars(destinationCars);

            destinationReader.AddCars(sourceCars);
        }

        private ReaderType GetReaderTypeFromExtension<T>(string filePath) where T : ICar, new()
        {
            var types = Enum.GetNames(typeof(ReaderType)).Select(x => x.ToLower()).ToList();
            string extesion = Path.GetExtension(filePath).ToLower().Replace(".","");
            int index = types.IndexOf(extesion);
            return (ReaderType)index;
        }
    }
}
