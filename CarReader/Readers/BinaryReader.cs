using CarReader.Interfaces;
using CarReader.Models;
using System.Text;

namespace CarReader.Readers
{
    public class BinaryReader : ICarReader
    {
        private readonly byte[] _header = { 0x25, 0x26 };

        private string _path;
        #region Property CheckBytes
        private int _bytes;
        private int CheckBytes
        {
            set
            {
                _bytes = value;
                if (_bytes == 0)
                    throw new FileLoadException("Wrong type of file.");
            }
        }
        #endregion

        #region Ctor
        public BinaryReader(string path)
        {
            _path = path;
        }

        public BinaryReader()
        {
            Random r = new Random();
            var i = r.Next(10000, 99999);
            _path = i.ToString() + ".CAR";
        }
        #endregion

        #region Read / Write
        private List<Car> Read()
        {
            using (var fStream = new FileStream(_path, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[2];
                CheckBytes = fStream.Read(buffer, 0, 2);

                buffer = new byte[4];
                CheckBytes = fStream.Read(buffer, 0, 4);
                int rowsCount = BitConverter.ToInt32(buffer, 0);

                var cars = new List<Car>();
                for (int i = 0; i < rowsCount; i++)
                {
                    buffer = new byte[8];
                    CheckBytes = fStream.Read(buffer, 0, 8);
                    long lDate = BitConverter.ToInt64(buffer, 0);
                    DateTime date = DateTime.FromBinary(lDate);

                    buffer = new byte[2];
                    CheckBytes = fStream.Read(buffer, 0, 2);
                    int brandLength = BitConverter.ToInt16(buffer, 0) * 2;

                    buffer = new byte[brandLength];
                    CheckBytes = fStream.Read(buffer, 0, brandLength);
                    Encoding unicode = Encoding.Unicode;
                    string brand = new string(unicode.GetChars(buffer, 0, brandLength));

                    buffer = new byte[4];
                    CheckBytes = fStream.Read(buffer, 0, 4);
                    int price = BitConverter.ToInt32(buffer, 0);

                    cars.Add(new Car()
                    {
                        Brand = brand,
                        Price = price,
                        Date = date
                    });
                }

                return cars;
            }
        }

        private void Write(List<Car> cars)
        {
            if (cars == null)
                throw new ArgumentNullException("Cars might be not null.");

            using (var fStream = new FileStream(_path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                byte[] buffer = _header;
                fStream.Write(buffer);

                buffer = BitConverter.GetBytes(cars.Count);
                fStream.Write(buffer);

                foreach (var car in cars)
                {
                    long date = car.Date.ToBinary();
                    buffer = BitConverter.GetBytes(date);
                    fStream.Write(buffer);

                    buffer = BitConverter.GetBytes(car.Brand.Length);
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

        #region CRUD
        public void AddCar(ICar car)
        {
            var cars = Read();
            if (cars.Any(x => x.Brand == car.Brand))
                throw new ArgumentException("Car with this brand already exists.");

            cars.Add((Car)car);
            Write(cars);
        }

        public ICar GetCar(string brand)
        {
            var cars = Read();
            return cars.FirstOrDefault(x => x.Brand == brand);
        }

        public List<ICar> GetCars() => Read().Select(x => (ICar)x).ToList();

        public void RemoveCar(ICar car)
        {
            var cars = Read();
            var currentCar = cars.FirstOrDefault(x => x.Brand == car.Brand);
            if (currentCar == null)
                throw new ArgumentException("Removable car not found.");

            cars.Remove(currentCar);
            Write(cars);
        }

        public void UpdateCar(ICar car)
        {
            var cars = Read();
            var currentCar = cars.FirstOrDefault(x => x.Brand == car.Brand);
            if (currentCar == null)
                throw new ArgumentException("Removable car not found.");

            currentCar.Price = car.Price;
            currentCar.Date = car.Date;
            Write(cars);
        } 
        #endregion
    }
}
