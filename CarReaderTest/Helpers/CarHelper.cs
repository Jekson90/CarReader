using CarReader.Interfaces;
using System;
using System.Collections.Generic;

namespace CarReaderTest.Helpers
{
    public class CarHelper<T> where T : ICar, new()
    {
        /// <summary>
        /// Gives simple object not empty
        /// </summary>
        /// <returns></returns>
        public static T GetMercedes()
        {
            return new T()
            {
                Brand = "Mercedes-Benz",
                Date = DateTime.Now.Date,
                Price = 5000000
            };
        }

        /// <summary>
        /// Gives a list of ICar objects witch aren't empty
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<T> GetCars(int count = 10)
        {
            for (int i = 0; i < count; i++)
            {
                //in center of list adds Mercedes
                if (i == (count - 1) / 2)
                    yield return GetMercedes();
                else
                    yield return new T()
                    {
                        Brand = "Brand" + i,
                        Date = DateTime.Now.Date,
                        Price = 1000000 * (i + 1)
                    };
            }
        }
    }
}
