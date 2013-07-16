using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCTranspo.Models
{
    public class OCStop
    {
        public int StopID { get; set; }
        public String StopCode { get; set; }
        public String StopDesc { get; set; }
        public Double StopLat { get; set; }
        public Double StopLong { get; set; }

        public OCStop() { }

        public static OCStop newOCStop(String StopCode, int StopID, String StopDesc, Double StopLat, Double StopLong)
        {
            OCStop stop = new OCStop();
            stop.StopID = StopID;
            stop.StopCode = StopCode;
            stop.StopDesc = StopDesc;
            stop.StopLat = StopLat;
            stop.StopLong = StopLong;
            return stop;
        }

    }
}
