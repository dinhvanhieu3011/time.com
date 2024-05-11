using System.Collections.Generic;

namespace BMBSOFT.GIS.CORE
{
    public class GeoCodingResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public List<GeoCoding> result { get; set; }
    }
    public class GeoCoding
    {
        //public List<AddressComponent> address_components { get; set; }
        public List<AddressComponent> addressComponents { get; set; }
        public List<string> types { get; set; }
        //public Geometry geometry { get; set; }
        public Coordinate location { get; set; }
        //public string formatted_address { get; set; }
        public string address { get; set; }
        public string name { get; set; }
    }

    public class AddressComponent
    {
        //public string long_name { get; set; }
        //public string short_name { get; set; }
        public string name { get; set; }
        public List<string> types { get; set; }
    }

    public class Geometry { 
        public Coordinate location { get; set; }
    }

    public class Coordinate
    {
        public string lng { get; set; }
        public string lat { get; set; }
    }
}
