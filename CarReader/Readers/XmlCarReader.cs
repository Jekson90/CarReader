using CarReader.Interfaces;
using CarReader.Models;
using System.Xml;
using System.Xml.Serialization;

namespace CarReader.Readers
{
    [XmlRoot("Document")]
    public class XmlCarReader : ICarReader
    {
        #region Properties
        [XmlElement("Cars")]
        public List<Car> Cars { get; set; } = new List<Car>();
        [XmlIgnore]
        public string FilePath { get; set; }
        #endregion

        #region Ctor
        public XmlCarReader()
        {
            Random r = new Random();
            var i = r.Next(10000, 99999);
            FilePath = i.ToString() + ".XML";
        }

        public XmlCarReader(string path)
        {
            FilePath = path;
        }
        #endregion

        #region Additional functions
        private void Write()
        {
            var xmlSettings = new XmlWriterSettings();
            xmlSettings.NewLineHandling = NewLineHandling.Entitize;
            xmlSettings.Indent = true;
            var xns = new XmlSerializerNamespaces();
            xns.Add("", "");

            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            using (var xmlWriter = XmlWriter.Create(fs, xmlSettings))
            {
                XmlSerializer sr = new XmlSerializer(typeof(XmlCarReader));
                sr.Serialize(xmlWriter, this, xns);
            }
        }

        private Car GetCarForRemoveOrUpdate(ICar car)
        {
            Cars = GetCars().Select(x => (Car)x).ToList();

            var currentCar = Cars.FirstOrDefault(x => x.Brand == car.Brand);
            if (currentCar == null)
                throw new ArgumentException("Car brand was not found.");

            return currentCar;
        }
        #endregion

        #region CRUD
        public void AddCar(ICar car)
        {
            if (car == null || string.IsNullOrEmpty(car.Brand))
                throw new ArgumentException("Argument or brand might be not null.");

            Cars.Clear();
            Cars.Add((Car)car);
            Write();
        }

        public List<ICar> GetCars()
        {
            var xmlSettings = new XmlReaderSettings();
            var xns = new XmlSerializerNamespaces();
            xns.Add("", "");

            using (FileStream fs = new FileStream(FilePath, FileMode.Open))
            using (var xmlReader = XmlReader.Create(fs, xmlSettings))
            {
                XmlSerializer sr = new XmlSerializer(typeof(XmlCarReader));
                var result = sr.Deserialize(xmlReader) as XmlCarReader;
                return result.Cars.Select(x => (ICar)x).ToList();
            }
        }

        public ICar GetCar(string brand) => GetCars().FirstOrDefault(x => x.Brand == brand);

        public void UpdateCar(ICar car)
        {
            var currentCar = GetCarForRemoveOrUpdate(car);
            currentCar.Price = car.Price;
            currentCar.Date = car.Date;
            Write();
        }

        public void RemoveCar(ICar car)
        {
            var currentCar = GetCarForRemoveOrUpdate(car);
            Cars.Remove(currentCar);
            Write();
        } 
        #endregion
    }
}
