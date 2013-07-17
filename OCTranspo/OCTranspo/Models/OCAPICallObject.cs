using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace OCTranspo.Models
{
    class OCAPICallObject
    {

        public OCAPICallObject(HttpWebRequest request, int stopNumber, int routeNumber)
        {
            this.WebRequest = request;
            this.StopNumber = stopNumber;
            this.RouteNumber = routeNumber;
        }

        public HttpWebRequest WebRequest { get; set; }
        public int StopNumber { get; set; }
        public int RouteNumber { get; set; }
    }
}
