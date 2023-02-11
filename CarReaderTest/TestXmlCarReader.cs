using CarReader.Models;
using CarReader.Readers;
using CarReaderTest.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace CarReaderTest
{
    public class TestXmlCarReader
    {
        [Test]
        public void TestRootDocument()
        {
            //get path, without this file
            string path = TestHelper.GetPath("TestXmlDocumentRoot.xml");
            var reader = ReaderFactory.GetCarReader<Car>(ReaderType.Xml, path);
            //get simple object
            var car1 = CarHelper<Car>.GetMercedes();
            //write object to file
            reader.AddCars(new List<Car> { car1 });
            //remove all cars, except "Document" root
            reader.RemoveCars(new List<Car> { car1 });
            //get root element
            XElement xRoot = XDocument.Load(path).Element("Document");
            Assert.That(xRoot, Is.Not.Null);
            //get list of car elements from document
            var cars = xRoot.Elements("Car").ToList();
            //check that document is empty (has only root element)
            Assert.That(cars.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestCarsElement()
        {
            //get path, without this file
            string path = TestHelper.GetPath("TestXmlDocumentElement.xml");
            var reader = ReaderFactory.GetCarReader<Car>(ReaderType.Xml, path);
            int count = 10;
            //get list of objects
            var cars = CarHelper<Car>.GetCars(count);
            //write object to file
            reader.AddCars(cars);
            //get root element
            XElement xRoot = XDocument.Load(path).Element("Document");
            Assert.That(xRoot, Is.Not.Null);
            //get list of car elements from document
            var carElements = xRoot.Elements("Car").ToList();
            //check count of "Car" elements
            Assert.That(carElements.Count, Is.EqualTo(count));
        }

        [TestCase("Document")]
        public void TestWrongElements(string elementName)
        {
            //get path, without this file
            string path = TestHelper.GetPath("TestXmlDocumentRoot.xml");
            var reader = ReaderFactory.GetCarReader<Car>(ReaderType.Xml, path);
            //get simple object
            var car1 = CarHelper<Car>.GetMercedes();
            //write object to file
            reader.AddCars(new List<Car> { car1 });
            string str;
            //read document as string
            using (var sReader = new StreamReader(path))
                str = sReader.ReadToEnd();
            //spoil it
            str = str.Replace(elementName, elementName + "1");
            using (var sWriter = new StreamWriter(path))
                sWriter.Write(str);
            //check it no reading
            Assert.Throws<InvalidOperationException>(() => reader.GetCars());
        }
    }
}
