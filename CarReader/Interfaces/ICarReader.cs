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

    //Interface makes easy to use different readers wich implement it
    //and we can develop architecture and use it without thinking about 
    //concrete classes (you can see examples in CRUD Tests, where we don't 
    //know about concrete reader).
    //Generic type is the same, it takes us possible to use not realisations
    //but abstractions.
    //Both of it give us scalability and independence of realisation.
    //But if we whant to change interface of reader it needs a lot of changes.
}
