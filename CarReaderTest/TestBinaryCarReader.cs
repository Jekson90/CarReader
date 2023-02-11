using CarReader.Models;
using CarReader.Readers;
using CarReaderTest.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CarReaderTest
{
    public class TestBinaryCarReader
    {
        [Test]
        public void TestReadingHeaderException()
        {
            //get path, without this file
            string path = TestHelper.GetPath("TestBinaryHeader.car");
            var reader = ReaderFactory.GetCarReader<Car>(ReaderType.Car, path);
            //get simple object
            var car1 = CarHelper<Car>.GetMercedes();
            //write object to file
            reader.AddCars(new List<Car> { car1 });
            //remove all cars, except header and number of rows
            reader.RemoveCars(new List<Car> { car1 });
            using (var fStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            {
                byte[] buffer = new byte[2];
                //read header
                fStream.Read(buffer, 0, 2);
                //check header
                Assert.That(buffer[0], Is.EqualTo(37));
                Assert.That(buffer[1], Is.EqualTo(38));
                //change header
                buffer[0]++;
                fStream.Position = 0;
                //write changed header
                fStream.Write(buffer, 0, 2);

                buffer = new byte[4];
                fStream.Position = 2;
                //read number of rows
                int nBytes = fStream.Read(buffer, 0, 4);
                int numberOfRows = BitConverter.ToInt32(buffer, 0);
                //check that readed righr number of bytes
                Assert.That(nBytes, Is.EqualTo(4));
                //check value of number of rows
                Assert.That(numberOfRows, Is.EqualTo(0));
            }
            //check, that exception is throwing in case wrong header
            var a = reader.GetCars();
            Assert.Throws<FileLoadException>(() => reader.GetCars().ToList());
        }

        [Test]
        public void TestRowsCount()
        {
            //get path, without this file
            string path = TestHelper.GetPath("TestBinaryRowsCount.car");
            var reader = ReaderFactory.GetCarReader<Car>(ReaderType.Car, path);
            int count = 10;
            //get list of objects
            var cars = CarHelper<Car>.GetCars(count);
            //write object to file
            reader.AddCars(cars);
            using (var fStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[4];
                fStream.Position = 2;
                //read number of rows
                int nBytes = fStream.Read(buffer, 0, 4);
                int numberOfRows = BitConverter.ToInt32(buffer, 0);
                //check that readed righr number of bytes
                Assert.That(nBytes, Is.EqualTo(4));
                //check value of number of rows
                Assert.That(numberOfRows, Is.EqualTo(count));
            }
        }
    }
}
