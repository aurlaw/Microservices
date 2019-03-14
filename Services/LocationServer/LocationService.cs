using System;
using System.Collections.Generic;
using System.IO;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using LocationServer.Models;

namespace LocationServer
{
    public class LocationService
    {

        public const string DefaultLocationFile = "loc_data.json";


        public IList<LocationProperty> GetLocations(string fileName) => JsonConvert.DeserializeObject<List<LocationProperty>>(File.ReadAllText(fileName), Converter.Settings);
        
    }
    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }    

}