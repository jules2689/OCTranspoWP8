using System;

public class OCSchedule
{
    public OCSchedule() { }

    public static OCSchedule newOCSchedule(int route_short_name, String arrival_time, String trip_headsign, String trip_id, String stop_code)
	{
        OCSchedule stopTime = new OCSchedule();
        stopTime.trip_id = trip_id;
        stopTime.route_short_name = route_short_name;
        stopTime.arrival_time = arrival_time;
        stopTime.trip_headsign = trip_headsign;
        stopTime.stop_code = stop_code;
        return stopTime;
	}

    public int route_short_name { get; set; }
    public String stop_code { get; set; }
    public String trip_headsign { get; set; }
    public String trip_id { get; set; }
    public String arrival_time { get; set; }
}
