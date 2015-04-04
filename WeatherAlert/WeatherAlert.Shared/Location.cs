using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherAlert
{
    public class Location
    {
        public String Province
        {
            get;
            set;
        }
        public String City
        {
            get;
            set;
        }
        public String AreaCode
        {
            get;
            set;
        }
        public Location(String province, String city)
        {
            Province = province;
            City = city;
        }
        public Location(String province, String city, String areaCode)
        {
            Province = province;
            City = city;
            AreaCode = areaCode;
        }
    }
}
