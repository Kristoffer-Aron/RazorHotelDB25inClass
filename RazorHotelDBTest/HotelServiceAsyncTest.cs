using RazorHotelDB25inClass.Interfaces;
using RazorHotelDB25inClass.Models;
using RazorHotelDB25inClass.Services;

namespace RazorHotelDBTest
{
    [TestClass]
    public class HotelServiceAsyncTest
    {
        [TestMethod]
        public void TestAddHotel()
        {
            //Arrange
            IHotelServiceAsync hotelService = new HotelService();
            List<Hotel> hotels = hotelService.GetAllHotelAsync().Result;

            //Act
            int numberOfHotelsBefore = hotels.Count;
            Hotel newHotel = new Hotel(3000, "TestTestTest", "TestRoad");
            bool ok = hotelService.CreateHotelAsync(newHotel).Result;
            hotels = hotelService.GetAllHotelAsync().Result;
            int numberOfHotelsAfter = hotels.Count;
            Hotel h = hotelService.DeleteHotelAsync(newHotel.HotelNr).Result;

            //Assert
            Assert.AreEqual(numberOfHotelsBefore + 1, numberOfHotelsAfter);
            Assert.IsTrue(ok);
            Assert.AreEqual(h.HotelNr, newHotel.HotelNr);
        }
    }
}