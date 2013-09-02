using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCTranspo
{
    class OCGeoMath
    {
        public double lowerLat { get; set; }
        public double upperLat { get; set; }
        public double lowerLong { get; set; }
        public double upperLong { get; set; }

        //mean radius of the Earth (m)
        private static double eRadius = 6371000;

        public static OCGeoMath getRange(double lat, double lon, double dist)
        {
            double[][] result = new double[2][];
            double[] min = new double[2];
            double[] max = new double[2];

            min = newLocation(lat, lon, dist, -45);
            max = newLocation(lat, lon, dist, 135);

            OCGeoMath geo = new OCGeoMath();
            geo.lowerLat = max[0];
            geo.upperLat = min[0];
            geo.lowerLong = min[1];
            geo.upperLong = max[1];
            return geo;
        }

        private static double[] newLocation(double lat, double lon, double dist, double angle)
        {
            double newLon, newLat, R;
            double[] result = new double[2];

            R = dist / eRadius;

            angle = angle * Math.PI / 180;
            lat = lat * Math.PI / 180;
            lon = lon * Math.PI / 180;

            newLat = Math.Asin((Math.Sin(lat) * Math.Cos(R)) + (Math.Cos(lat) * Math.Sin(R) * Math.Cos(angle)));
            newLon = lon + Math.Atan2(Math.Sin(angle) * Math.Sin(R) * Math.Cos(lat), Math.Cos(R) - (Math.Sin(lat) * Math.Sin(newLat)));
            newLon = ((newLon + (3 * Math.PI)) % (2 * Math.PI)) - Math.PI;

            newLat = newLat * 180 / Math.PI;
            newLon = newLon * 180 / Math.PI;

            result[0] = roundTo(newLat, 6);
            result[1] = roundTo(newLon, 6);
            return result;
        }

        private static double roundTo(double x, int places)
        {
            double p = Math.Pow(10, places);

            x *= p;
            x += 0.5;
            x = Math.Truncate(x);
            x /= p;
            return x;
        }


    }
}
