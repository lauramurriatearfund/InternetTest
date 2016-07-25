using Subgurim.Controles;
using System.Web;
using MvcMovie.Helpers;
using System;

namespace MvcMovie.Helpers
{
    public class LocationHelper
    {
        private Logger logger = Logger.Get();

        public static Location GetCityLocationFromIP(string ipAddress)
        {
            Location loc = null;
            string databasePath = HttpContext.Current.Server.MapPath("~/App_Data/GeoLite2-City.mmdb");

            if (databasePath != null)
            {
                LookupService service = new LookupService(databasePath);
                if (service != null)
                {
                    loc = service.getLocation(ipAddress);
                }
                else
                {
                    //cannot write to logger from static context so just output to console
                    Console.WriteLine("Mapping database was not opened at " + databasePath, null);
                }
            }
            else
            {
                Console.WriteLine("Mapping database was not found at " + databasePath, null);
            }
            return loc;
        }



        public static Location GetCountryLocationFromIP(string ipAddress)
        {
            Location loc = null;
            string databasePath = HttpContext.Current.Server.MapPath("~/App_Data/GeoLite2-Country.mmdb");

            if (databasePath != null)
            {
                LookupService service = new LookupService(databasePath);
                if (service != null)
                {
                    loc = service.getLocation(ipAddress);
                }
                else
                {
                    //cannot write to logger from static context so just output to console
                    Console.WriteLine("Mapping database was not opened at " + databasePath, null);
                }
            }
            else
            {
                Console.WriteLine("Mapping database was not found " + databasePath, null);
            }
            return loc;
        }


    }
}