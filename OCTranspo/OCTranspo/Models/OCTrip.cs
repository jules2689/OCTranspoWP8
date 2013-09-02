using System;

public class OCTrip
{

    public OCTrip(){}

    public static OCTrip newOCTrip(String route_id, String service_id, String trip_id, String trip_headsign, int direction_id, int block_id)
    {
        OCTrip trip = new OCTrip();
        trip.route_id = route_id;
        trip.service_id = service_id;
        trip.trip_id = trip_id;
        trip.trip_headsign = trip_headsign;
        trip.direction_id = direction_id;
        trip.block_id = block_id;
        return trip;
    }

    public String route_id { get; set; }
    public String service_id { get; set; }
    public String trip_id { get; set; }
    public String trip_headsign { get; set; }
    public int direction_id { get; set; }
    public int block_id { get; set; }
    public double Longitude { get; set; }
    public bool LastTrip { get; set; }

}
