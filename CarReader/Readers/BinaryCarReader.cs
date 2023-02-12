using CarReader.Interfaces;
using System.Text;

namespace CarReader.Readers
{
    /// <summary>
    /// Type for reading binary files with extension ".car" which consist from concrete structure
    /// and contains any type of ICar interface.
    /// </summary>
    /// <typeparam name="T">Object type for reading and writing</typeparam>
    public sealed class BinaryCarReader<T> : BaseCarReader<T> where T : ICar, new()
    {
        private readonly byte[] _header = { 0x25, 0x26 };

        protected override ReaderType ReaderType { get => ReaderType.Car; }

        #region Property CheckBytes
        /// <summary>
        /// Checks number of bytes, if 0 throws FileLoadException
        /// </summary>
        private int CheckBytes
        {
            set
            {
                if (value == 0)
                    throw new FileLoadException("Wrong type of file.");
            }
        }
        #endregion

        #region Ctor
        public BinaryCarReader() : base()
        {
            SetDefaultFilePath();
        }

        public BinaryCarReader(string path) : base(path) { }
        #endregion

        #region Read / Write
        protected override IEnumerable<T> Read()
        {            
            using (var fStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                //read and check header of file
                byte[] buffer = new byte[2];
                CheckBytes = fStream.Read(buffer, 0, 2);
                if (buffer[0] != _header[0] || buffer[1] != _header[1])
                    CheckBytes = 0;

                //read and check rows count
                buffer = new byte[4];
                CheckBytes = fStream.Read(buffer, 0, 4);
                int rowsCount = BitConverter.ToInt32(buffer, 0);

                //read objects
                for (int i = 0; i < rowsCount; i++)
                {
                    //read Date
                    buffer = new byte[8];
                    CheckBytes = fStream.Read(buffer, 0, 8);
                    long lDate = BitConverter.ToInt64(buffer, 0);
                    DateTime date = DateTime.FromBinary(lDate);

                    //read length of Brand's string
                    buffer = new byte[2];
                    CheckBytes = fStream.Read(buffer, 0, 2);
                    int brandLength = BitConverter.ToInt16(buffer, 0) * 2;

                    //read Brand
                    buffer = new byte[brandLength];
                    CheckBytes = fStream.Read(buffer, 0, brandLength);
                    Encoding unicode = Encoding.Unicode;
                    string brand = new string(unicode.GetChars(buffer, 0, brandLength));

                    //read Price
                    buffer = new byte[4];
                    CheckBytes = fStream.Read(buffer, 0, 4);
                    int price = BitConverter.ToInt32(buffer, 0);

                    yield return new T()
                    {
                        Brand = brand,
                        Price = price,
                        Date = date
                    };
                }
            }
        }

        protected override void Write(IEnumerable<T> cars)
        {
            if (cars == null)
                throw new ArgumentNullException("Cars might be not null.");

            using (var fStream = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                //write header of file
                byte[] buffer = _header;
                fStream.Write(buffer);

                //write objects count
                buffer = BitConverter.GetBytes(cars.Count());
                fStream.Write(buffer);

                //write objects
                foreach (var car in cars)
                {
                    long date = car.Date.ToBinary();
                    buffer = BitConverter.GetBytes(date);
                    fStream.Write(buffer);

                    short brandLength = (short)car.Brand.Length;
                    buffer = BitConverter.GetBytes(brandLength);
                    fStream.Write(buffer);

                    Encoding unicode = Encoding.Unicode;
                    buffer = unicode.GetBytes(car.Brand);
                    fStream.Write(buffer);

                    buffer = BitConverter.GetBytes(car.Price);
                    fStream.Write(buffer);
                }
            }
        }
        #endregion
    }
}
