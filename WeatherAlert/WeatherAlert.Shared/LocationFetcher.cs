using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using System.Net.Http;
using System.Xml.Linq;
using System.Linq;

namespace WeatherAlert
{
    class LocationFetcher
    {
        static String BaiduApiUrl = "http://api.map.baidu.com/geocoder?location={0},{1}&output=xml";
        Geolocator geo = null;
        public LocationFetcher()
        {
        }

        public async Task<Geoposition> getCurrentLocation()
        {
            if (geo == null)
            {
                geo = new Geolocator();
            }

            Geoposition pos = await geo.GetGeopositionAsync();
            return pos;
        }

        async Task<Location> findLocation(String province, String city)
        {
            var locations = await LocationList.getLocationsAsync();
            var answer = from location in locations
                         where province.Contains(location.Province)
                         && city.Contains(location.City)
                         select location;
            return answer.FirstOrDefault();
        }

        public async Task<Location> getCurrentCity()
        {
            var location = await getCurrentLocation();
            Double latitude = location.Coordinate.Point.Position.Latitude;
            Double longitude = location.Coordinate.Point.Position.Longitude;

            HttpClient client = new HttpClient();
            StringBuilder stringBuilder = new StringBuilder(50);
            stringBuilder.AppendFormat(BaiduApiUrl, latitude, longitude);
            var responseMessage = await client.GetAsync(stringBuilder.ToString());
            String responseXml = await responseMessage.Content.ReadAsStringAsync();

            XDocument document = XDocument.Parse(responseXml);
            XElement root = document.Element("GeocoderSearchResponse");
            XElement status = root.Element("status");
            String statusValue = status.Value;
            if (statusValue.Equals("OK"))
            {
                XElement result = root.Element("result");
                XElement address = result.Element("addressComponent");
                String province = address.Element("province").Value;
                String city = address.Element("city").Value;

                return await findLocation(province, city);
            }
            else
            {
                return null;
            }
        }
    }
}
