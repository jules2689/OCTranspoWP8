using System;

public class OCTrip
{

    public OCTrip(){}

    public void newOCTrip(String destination, DateTime startTime, int adjustTime, String busType, double gpsSpeed = 0, double latitude = 0, double longitude = 0, bool lastTrip = false)
    {
        this.Destination = destination;
        this.StartTime = startTime;
        this.AdjustTime = adjustTime;
        this.LastTrip = lastTrip;
        this.BusType = busType;
        this.GPSSpeed = gpsSpeed;
        this.Latitude = latitude;
        this.Longitude = longitude;
    }

    public String Destination { get; set; }
    public DateTime StartTime { get; set; }
    public int AdjustTime { get; set; }
    public String BusType { get; set; }
    public double GPSSpeed { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool LastTrip { get; set; }

}
