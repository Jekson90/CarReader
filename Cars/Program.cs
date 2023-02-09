using CarReader.Models;
using CarReader.Readers;
using System.Xml;

var car = new Car()
{
    Brand = "MercedesBenz",
    Date = new DateTime(2012, 5, 16),
    Price = 6000000
};
string path = "mers.xml";
var reader = new XmlCarReader();
reader.AddCar(car);
var r = reader.GetCars();
Console.WriteLine(r.First());