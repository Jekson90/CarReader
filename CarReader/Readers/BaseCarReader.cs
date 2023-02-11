using CarReader.Interfaces;

namespace CarReader.Readers
{
    public abstract class BaseCarReader<T> : ICarReader<T> where T : ICar
    {
        protected abstract ReaderType ReaderType { get; }
        protected string FilePath { get; set; }

        #region Ctor
        public BaseCarReader() { }

        public BaseCarReader(string path)
        {
            FilePath = path;
        }
        #endregion

        protected string GetRandomNumber() => new Random().Next(10000, 99999).ToString();
        public static string GetReaderTypeName(ReaderType type) => Enum.GetNames(typeof(ReaderType))[(int)type].ToLower();
        protected void SetDefaultFilePath() => FilePath = GetRandomNumber() + "." + GetReaderTypeName(ReaderType);
        protected abstract IEnumerable<T> Read();
        protected abstract void Write(IEnumerable<T> cars);

        protected void CheckFile()
        {
            if (!CheckFileAndCreate())
                AddCars(new List<T>());
        }

        protected bool CheckFileAndCreate()
        {
            if (!File.Exists(FilePath))
            {
                File.Create(FilePath).Close();
                return false;
            }
            else
                return true;
        }

        #region CRUD
        public void AddCars(IEnumerable<T> cars)
        {
            CheckFileAndCreate();

            List<T> oldCars;
            try
            {
                //if file empty (no header or no xRoot), reader throws exception
                oldCars = Read().ToList();
            }
            catch
            {
                oldCars = new List<T>();
            }

            if (oldCars.Any(x => cars.Select(x => x.Brand).Contains(x.Brand)))
                throw new ArgumentException("Cars with similar brands " +
                    "in both of old and new lists were found.");

            oldCars.AddRange(cars);
            Write(oldCars);
        }

        public T GetCar(string brand)
        {
            CheckFile();
            return Read().FirstOrDefault(x => x.Brand == brand);
        }

        public IEnumerable<T> GetCars()
        {
            CheckFile();
            return Read();
        }

        public void RemoveCars(IEnumerable<T> cars)
        {
            var oldCars = Read().ToList();
            List<T> removableCars = oldCars.Where(x => cars.Select(y => y.Brand).
                                                            Contains(x.Brand)).ToList();
            foreach (var car in removableCars)
                oldCars.Remove(car);

            Write(oldCars);
        }

        public void UpdateCars(IEnumerable<T> cars)
        {
            //get list for closing reading file (there using yield return)
            var oldCars = Read().ToList();
            List<T> updatableCars = oldCars.Where(x => cars.Select(y => y.Brand).
                                                            Contains(x.Brand)).ToList();
            foreach (var car in cars)
            {
                var updatableCar = oldCars.FirstOrDefault(x => x.Brand == car.Brand);
                if (updatableCar != null)
                {
                    updatableCar.Price = car.Price;
                    updatableCar.Date = car.Date;
                }
            }

            Write(oldCars);
        }
        #endregion
    }
}
