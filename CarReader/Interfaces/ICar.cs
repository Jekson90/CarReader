namespace CarReader.Interfaces
{
    public interface ICar
    {
        string Brand { get; set; }
        DateTime Date { get; set; }
        int Price { get; set; }
    }

    //While we use Interface unstead concrete class
    //we don't think about it realisation
    //We can add a lot of implemented types and all of them will work.
    //But if we whant to change interface it needs a lot of changes.
}
