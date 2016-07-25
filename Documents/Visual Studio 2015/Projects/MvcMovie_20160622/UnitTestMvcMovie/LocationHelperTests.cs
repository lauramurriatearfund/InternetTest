using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcMovie;
using MvcMovie.Helpers;
using Subgurim.Controles;

namespace UnitTestMvcMovie
{
    [TestClass]
    public class LocationHelperTests
    {
        [TestMethod]
        public void TestGetCountryLocationFromIP()
        {
            //arrange
            string ip = "163.99.8.26";
            string expected = "FR";
            string actual;
            //act
            Location actualLoc = LocationHelper.GetCountryLocationFromIP(ip);
            if (actualLoc != null)
            {
                actual = actualLoc.countryCode;
            }
            else
            {
                actual = null;
            }

            //assert equal, ignoring case
            Assert.AreEqual(expected, actual, true);
        }

        [TestMethod]
        public void TestGetCityLocationFromIP()
        {
            //arrange
            string ip = "163.99.8.26";
            string expected = "Nanterre";
            string actual;
            
            //act
            Location actualLoc = LocationHelper.GetCityLocationFromIP(ip);
            if (actualLoc != null) {
                actual = actualLoc.city;
            } else
            {
                actual = null;
            }
               

            //assert equal, ignoring case
            Assert.AreEqual(expected, actual, true);
        }
    }
}
