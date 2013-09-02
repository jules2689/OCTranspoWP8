using System;

public class OCStopTime
{
    public OCStopTime() { }

    public static OCStopTime newOCStopTime(String trip_id, String stop_id, int stop_sequence, int pickup_type, int drop_off_type, String arrival_time, String departure_time)
	{
        OCStopTime stopTime = new OCStopTime();
        stopTime.trip_id = trip_id;
        stopTime.stop_id = stop_id;
        stopTime.stop_sequence = stop_sequence;
        stopTime.pickup_type = pickup_type;
        stopTime.drop_off_type = drop_off_type;
        stopTime.arrival_time = arrival_time;
        stopTime.departure_time = departure_time;
        return stopTime;
	}

    [SQLite.Indexed]
    public String trip_id { get; set; }
    public String stop_id { get; set; }
    public int stop_sequence { get; set; }
    public int pickup_type { get; set; }
    public int drop_off_type { get; set; }
    public String arrival_time { get; set; }
    public String departure_time { get; set; }
}
