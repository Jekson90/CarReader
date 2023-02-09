namespace CarReader.Interfaces
{
    public interface ICarReader
    {
        List<ICar> GetCars();
        ICar GetCar(string brand);
        void UpdateCar(ICar car);
        void AddCar(ICar car);
        void RemoveCar(ICar car);
    }
}
