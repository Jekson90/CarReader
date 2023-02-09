namespace CarReader.Interfaces
{
    public interface ICar
    {
        string Brand { get; set; }
        DateTime Date { get; set; }
        int Price { get; set; }
    }
}
