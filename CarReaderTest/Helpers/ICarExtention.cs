using CarReader.Interfaces;

namespace CarReaderTest.Helpers
{
    public static class ICarExtention
    {
        /// <summary>
        /// Extention method for ICar
        /// </summary>
        /// <param name="car"></param>
        /// <param name="car1"></param>
        /// <returns></returns>
        public static bool EqualTo(this ICar car, ICar car1)
        {
            return car.Brand == car1.Brand &&
                   car.Price == car1.Price &&
                   car.Date == car1.Date;
        }
    }
}
