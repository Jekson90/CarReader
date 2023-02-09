using CarReader.Interfaces;
using System.Xml.Serialization;

namespace CarReader.Models
{
    public class Car : ICar
    {
        public string Brand { get; set; }
        public int Price { get; set; }

        #region Date
        [XmlIgnore]
        public DateTime Date { get; set; }

        [XmlElement("Date")]
        public string DateString
        {
            get => Date.ToString("dd.MM.yyyy");
            set => Date = DateTime.Parse(value);
        } 
        #endregion

        public override string ToString()
        {
            return $"Brand = {Brand}\nDate = {Date.ToString("dd.MM.yyyy")}\nPrice = {Price}";
        }
    }
}
