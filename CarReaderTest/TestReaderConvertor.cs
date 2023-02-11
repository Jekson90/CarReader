using CarReader.Convertors;
using CarReader.Models;
using CarReader.Readers;
using CarReaderTest.Helpers;
using NUnit.Framework;
using System;
using System.Linq;

namespace CarReaderTest
{
    public class TestReaderConvertor
    {
        [TestCase(ReaderType.Car, ReaderType.Xml)]
        [TestCase(ReaderType.Xml, ReaderType.Car)]
        public void TestConvertor(ReaderType sourceType, ReaderType destinationType)
        {
            string sourceExtension = BaseCarReader<Car>.GetReaderTypeName(sourceType);
            string destinationExtension = BaseCarReader<Car>.GetReaderTypeName(destinationType);
            //get paths, without this files
            string sourcePath = TestHelper.GetPath("source." + sourceExtension);
            string destinationPath = TestHelper.GetPath("destination." + destinationExtension);
            var sourceReader = ReaderFactory.GetCarReader<Car>(sourceType, sourcePath);
            //get list of objects
            var cars = CarHelper<Car>.GetCars().ToList();
            //write objects
            sourceReader.AddCars(cars);
            //convert data
            var convertor = new FileConvertor(sourcePath, destinationPath);
            convertor.Convert<Car>();
            //read converted data
            var destinationReader = ReaderFactory.GetCarReader<Car>(destinationType, destinationPath);
            var result = destinationReader.GetCars().ToList();
            //compare
            for (int i = 0; i < cars.Count; i++)
                Assert.That(cars[i].EqualTo(result[i]), Is.True);
        }

        [TestCase(ReaderType.Car, ReaderType.Car)]
        [TestCase(ReaderType.Xml, ReaderType.Xml)]
        public void TestConvertorTypesEqualException(ReaderType sourceType, ReaderType destinationType)
        {
            string sourceExtension = BaseCarReader<Car>.GetReaderTypeName(sourceType);
            string destinationExtension = BaseCarReader<Car>.GetReaderTypeName(destinationType);
            //get paths, without this files
            string sourcePath = TestHelper.GetPath("sourceEx." + sourceExtension);
            string destinationPath = TestHelper.GetPath("destinationEx." + destinationExtension);
            //convert data
            var convertor = new FileConvertor(sourcePath, destinationPath);
            Assert.Throws<ArgumentException>(() => convertor.Convert<Car>());
        }
    }
}
