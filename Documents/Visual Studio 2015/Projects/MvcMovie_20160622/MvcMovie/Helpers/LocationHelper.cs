using Subgurim.Controles;
using System.Web;

namespace MvcMovie.Helpers
{
    public class LocationHelper
    {

        public static Location GetCityLocationFromIP(string ipAddress)
        {
            string databasePath = HttpContext.Current.Server.MapPath("~/app_data/GeoLite2-City.mmdb");
            LookupService service = new LookupService(databasePath);
            Location loc = service.getLocation(ipAddress);

            return loc;
        }
        public static Location GetCountryLocationFromIP(string ipAddress)
        {
            string databasePath = HttpContext.Current.Server.MapPath("~/app_data/GeoLite2-Country.mmdb");
            LookupService service = new LookupService(databasePath);
            Location loc = service.getLocation(ipAddress);

            return loc;
        }


    }
}