using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace WeatherAlert
{
    static class LocationList
    {
        static List<Location> locations;
        public static async Task<List<Location>> getLocationsAsync()
        {
            if (locations == null)
            {
                var uri = new System.Uri("ms-appx:///data/location.csv");
                var file = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri);
                var fileStream = await file.OpenStreamForReadAsync();
                StreamReader reader = new StreamReader(fileStream);
                locations = new List<Location>();
                while (!reader.EndOfStream)
                {
                    String line = reader.ReadLine();
                    var tokens = line.Split(',');
                    if (tokens.Length < 3)
                        continue;
                    Location location = new Location(tokens[2], tokens[1], tokens[0]);
                    locations.Add(location);
                }
            }
            return locations;
        }
    }
}
