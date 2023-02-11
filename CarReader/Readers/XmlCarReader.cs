using CarReader.Interfaces;
using System.Xml;
using System.Xml.Serialization;

namespace CarReader.Readers
{
    [XmlRoot("Document")]
    public sealed class XmlCarReader<T> : BaseCarReader<T> where T : ICar
    {
        protected override ReaderType ReaderType { get => ReaderType.Xml; }

        #region Properties
        [XmlElement("Car")]
        public List<T> Cars { get; set; } = new List<T>();
        #endregion

        #region Ctor
        public XmlCarReader()
        {
            SetDefaultFilePath();
        }

        public XmlCarReader(string path) : base(path) { }
        #endregion

        #region Read / Write
        protected override void Write(IEnumerable<T> cars)
        {
            Cars.Clear();
            Cars.AddRange(cars);

            var xmlSettings = new XmlWriterSettings();
            xmlSettings.NewLineHandling = NewLineHandling.Entitize;
            xmlSettings.Indent = true;
            var xns = new XmlSerializerNamespaces();
            xns.Add("", "");

            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            using (var xmlWriter = XmlWriter.Create(fs, xmlSettings))
            {
                XmlSerializer sr = new XmlSerializer(typeof(XmlCarReader<T>));
                sr.Serialize(xmlWriter, this, xns);
            }
        }

        protected override IEnumerable<T> Read()
        {
            var xmlSettings = new XmlReaderSettings();
            var xns = new XmlSerializerNamespaces();
            xns.Add("", "");

            using (FileStream fs = new FileStream(FilePath, FileMode.Open))
            using (var xmlReader = XmlReader.Create(fs, xmlSettings))
            {
                XmlSerializer sr = new XmlSerializer(typeof(XmlCarReader<T>));
                var result = sr.Deserialize(xmlReader) as XmlCarReader<T>;
                return result.Cars;
            }
        }
        #endregion
    }
}
