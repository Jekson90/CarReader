using CarReader.Models;
using CarReader.Readers;
using System.Xml;

var car = new Car()
{
    Brand = "MercedesBenz",
    Date = new DateTime(2012, 5, 16),
    Price = 6000000
};

var s = new XmlCarReader<Car>();

string path = Directory.GetCurrentDirectory();
path = Path.Combine(path, "Test");
if (!Directory.Exists(path))
    Directory.CreateDirectory(path);
path = Path.Combine(path, "mers.xml");
var reader = new XmlCarReader<Car>(path);
reader.AddCars(new List<Car> { car });
var r = reader.GetCars();
Console.WriteLine(r.First());