using System;

public class OCTrip
{

    public OCTrip(){}

    public static OCTrip newOCTrip(String destination, String startTime, int adjustTime,  double adjustAge, String busType, double gpsSpeed = 0, double latitude = 0, double longitude = 0, bool lastTrip = false)
    {
        OCTrip trip = new OCTrip();
        trip.Destination = destination;
        trip.StartTime = startTime;
        trip.AdjustTime = adjustTime;
        trip.LastTrip = lastTrip;
        trip.BusType = busType;
        trip.GPSSpeed = gpsSpeed;
        trip.Latitude = latitude;
        trip.Longitude = longitude;
        trip.AdjustAge = adjustAge;
        return trip;
    }

    public String Destination { get; set; }
    public String StartTime { get; set; }
    public int AdjustTime { get; set; }
    public String BusType { get; set; }
    public double GPSSpeed { get; set; }
    public double AdjustAge { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool LastTrip { get; set; }

}
