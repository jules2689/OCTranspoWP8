using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCTranspo.Models
{
    public class OCStop
    {
        public int stop_id { get; set; }
        public String stop_code { get; set; }
        public String stop_name { get; set; }
        public Double stop_lat { get; set; }
        public Double stop_lon { get; set; }

        public OCStop() { }

        public static OCStop newOCStop(String stop_code, int stop_id, String stop_name, Double stop_lat, Double stop_lon)
        {
            OCStop stop = new OCStop();
            stop.stop_id = stop_id;
            stop.stop_code = stop_code;
            stop.stop_name = stop_name;
            stop.stop_lat = stop_lat;
            stop.stop_lon = stop_lon;
            return stop;
        }

    }
}
