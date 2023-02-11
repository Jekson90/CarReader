using CarReader.Models;
using CarReader.Readers;
using CarReaderTest.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarReaderTest
{
    public class TestCRUD
    {
        [TestCase(ReaderType.Car)]
        [TestCase(ReaderType.Xml)]
        public void TestSingleAddingAndGettingInBinaryReader(ReaderType type)
        {
            //get path, without this file
            string path = TestHelper.GetPath("TestSingleBinaryWriteRead.car");
            var reader = ReaderFactory.GetCarReader<Car>(type, path);
            //get simple object
            var car1 = CarHelper<Car>.GetMercedes();
            //write simple object to file
            reader.AddCars(new List<Car> { car1 });
            //read object from file by it's name
            var car2 = reader.GetCar(car1.Brand);
            //null-check
            Assert.That(car2, Is.Not.Null);
            //equals-check
            Assert.That(car2.EqualTo(car1), Is.True);
        }

        [TestCase(ReaderType.Car)]
        [TestCase(ReaderType.Xml)]
        public void TestMultipleAddingAndGettingInBinaryReader(ReaderType type)
        {
            //get path, without this file
            string path = TestHelper.GetPath("TestMultipleBinaryWriteRead.car");
            var reader = ReaderFactory.GetCarReader<Car>(type, path);
            //get list of objects
            var cars = CarHelper<Car>.GetCars().ToList();
            //write objects
            reader.AddCars(cars);
            //read objects
            var readedCars = reader.GetCars().ToList();
            //check count of readed list
            int count = cars.Count;
            Assert.That(readedCars.Count, Is.EqualTo(count));
            //check each element and order
            for (int i = 0; i < count; i++)
                Assert.That(readedCars[i].EqualTo(cars[i]), Is.True);
        }

        [TestCase(ReaderType.Car)]
        [TestCase(ReaderType.Xml)]
        public void TestUpdatingInCarReader(ReaderType type)
        {
            //get path, without this file
            string path = TestHelper.GetPath("TestBinaryUpdate.car");
            var reader = ReaderFactory.GetCarReader<Car>(type, path);
            int count = 10;
            //get list of objects
            var cars = CarHelper<Car>.GetCars(count).ToList();
            //write objects
            reader.AddCars(cars);
            //chage 3 cars
            var changedCars = new List<Car> { cars[3], cars[2], cars[1] };
            for (int i = 0; i < changedCars.Count; i++)
                changedCars[i].Price = i;
            //update 3 cars
            reader.UpdateCars(changedCars);
            //check updates
            foreach (var changedCar in changedCars)
            {
                var readedCar = reader.GetCar(changedCar.Brand);
                Assert.That(readedCar.EqualTo(changedCar), Is.True);
            }
        }

        [TestCase(ReaderType.Car)]
        [TestCase(ReaderType.Xml)]
        public void TestRemovingInCarReader(ReaderType type)
        {
            //get path, without this file
            string path = TestHelper.GetPath("TestBinaryRemove.car");
            var reader = ReaderFactory.GetCarReader<Car>(type, path);
            int count = 10;
            //get list of objects
            var cars = CarHelper<Car>.GetCars(count).ToList();
            //write objects
            reader.AddCars(cars);
            var removableCars = new List<Car> { cars[3], cars[2], cars[1] };
            //remove cars
            reader.RemoveCars(removableCars);
            //check
            foreach (var car in removableCars)
                Assert.That(reader.GetCar(car.Brand), Is.Null);
        }

        [TestCase(ReaderType.Car)]
        [TestCase(ReaderType.Xml)]
        public void TestAddExistingItemException(ReaderType type)
        {
            //get path, without this file
            string path = TestHelper.GetPath("TestBinaryRemove.car");
            var reader = ReaderFactory.GetCarReader<Car>(type, path);
            int count = 10;
            //get list of objects
            var cars = CarHelper<Car>.GetCars(count).ToList();
            //write objects
            reader.AddCars(cars);
            //getting Mercedes
            var mercedes = CarHelper<Car>.GetMercedes();
            Assert.Throws<ArgumentException>(() => reader.AddCars(new List<Car>() { mercedes }));
        }
    }
}