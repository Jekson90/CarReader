namespace CarReader.Interfaces
{
    public interface ICarReader<T> where T : ICar
    {
        IEnumerable<T> GetCars();
        T GetCar(string brand);
        void UpdateCars(IEnumerable<T> car);
        void AddCars(IEnumerable<T> car);
        void RemoveCars(IEnumerable<T> car);
    }
}
