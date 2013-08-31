using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCTranspo.Models
{
    class OCCSVParserHelper
    {
        //CSV 
        public List<OCStopTime> getOCStopTimeFromCSV(string location)
        {
            List<OCStopTime> list = new List<OCStopTime>();
            Boolean first = true;
            using (var sr = new System.IO.StreamReader(location))
            {
                while (sr.Peek() != -1)
                {
                    var line = sr.ReadLine();
                    if (first == false)
                    {
                        var fields = line.Split(',');
                        //trip_id,arrival_time,departure_time,stop_id,stop_sequence,pickup_type,drop_off_type
                        OCStopTime time = OCStopTime.newOCStopTime(fields[0], fields[3], int.Parse(fields[4]), int.Parse(fields[5]), int.Parse(fields[6]), fields[1], fields[2]);
                        list.Add(time);
                    }
                    else
                    {
                        first = false;
                    }
                }
                sr.Close();
            }
            return list;
        }

        public List<OCStop> getOCStopsFromCSV(string location)
        {
            List<OCStop> list = new List<OCStop>();
            Boolean first = true;
            using (var sr = new System.IO.StreamReader(location))
            {
                while (sr.Peek() != -1)
                {
                    var line = sr.ReadLine();
                    if (first == false)
                    {
                        var fields = line.Split(',');
                        //trip_id,arrival_time,departure_time,stop_id,stop_sequence,pickup_type,drop_off_type
                        OCStop stop = OCStop.newOCStop(fields[1], int.Parse(fields[0]), fields[3], double.Parse(fields[4]), double.Parse(fields[5]));
                        list.Add(stop);
                    }
                    else
                    {
                        first = false;
                    }
                }
                sr.Close();
            }
            return list;
        }

        public List<OCRoute> getOCRouteFromCSV(string location)
        {
            List<OCRoute> list = new List<OCRoute>();
            Boolean first = true;
            using (var sr = new System.IO.StreamReader(location))
            {
                while (sr.Peek() != -1)
                {
                    var line = sr.ReadLine();
                    if (first == false)
                    {
                        var fields = line.Split(',');
                        //route_id,service_id,trip_id,trip_headsign,direction_id,block_id
                        OCRoute route = OCRoute.newOCRoute(-1, fields[0], int.Parse(fields[4]), fields[3], fields[5]);
                        list.Add(route);
                    }
                    else
                    {
                        first = false;
                    }
                }
                sr.Close();
            }
            return list;
        }

        public List<OCRouteNumber> getOCRouteNumberFromCSV(string location)
        {
            List<OCRouteNumber> list = new List<OCRouteNumber>();
            Boolean first = true;
            using (var sr = new System.IO.StreamReader(location))
            {
                while (sr.Peek() != -1)
                {
                    var line = sr.ReadLine();
                    if (first == false)
                    {
                        var fields = line.Split(',');
                        //route_id,route_short_name,route_long_name,route_type
                        OCRouteNumber route = OCRouteNumber.newOCRoute(int.Parse(fields[1]),fields[0],int.Parse(fields[3]));
                        list.Add(route);
                    }
                    else
                    {
                        first = false;
                    }
                }
                sr.Close();
            }
            return list;
        }

    }
}
