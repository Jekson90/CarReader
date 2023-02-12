using CarReader.Interfaces;
using System.Xml;
using System.Xml.Serialization;

namespace CarReader.Readers
{
    /// <summary>
    /// Type for reading XML files which contains any type of ICar interface.
    /// </summary>
    /// <typeparam name="T">Type of object for read/write</typeparam>
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

            //set settings
            var xmlSettings = new XmlWriterSettings();
            xmlSettings.NewLineHandling = NewLineHandling.Entitize;
            xmlSettings.Indent = true;
            //remove unnecessary information
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
            using (FileStream fs = new FileStream(FilePath, FileMode.Open))
            using (var xmlReader = XmlReader.Create(fs, new XmlReaderSettings()))
            {
                XmlSerializer sr = new XmlSerializer(typeof(XmlCarReader<T>));
                var result = sr.Deserialize(xmlReader) as XmlCarReader<T>;
                return result.Cars;
            }
        }
        #endregion
    }
}
