using System;

public class OCSchedule
{
    public OCSchedule() { }
  
    public static OCSchedule newOCSchedule(int route_short_name, int start_date, int end_date, String stop_code, String stop_id, String route_id, String arrival_time, String trip_headsign, int monday, int tuesday, int wednesday, int thursday, int friday, int saturday, int sunday)
	{
        OCSchedule schedule = new OCSchedule();
        schedule.route_short_name = route_short_name;
        schedule.stop_code = stop_code;
        schedule.start_date = start_date;
        schedule.end_date = end_date;
        schedule.stop_id = stop_id;
        schedule.route_id = route_id;
        schedule.arrival_time = arrival_time;
        schedule.trip_headsign = trip_headsign;
        schedule.monday = monday;
        schedule.tuesday = tuesday;
        schedule.wednesday = wednesday;
        schedule.thursday = thursday;
        schedule.friday = friday;
        schedule.saturday = saturday;
        schedule.sunday = sunday;
        return schedule;
	}

    public int route_short_name { get; set; }
    public int start_date { get; set; }
    public int end_date { get; set; }
    public String trip_headsign { get; set; }
    public String stop_code { get; set; }
    public String stop_id { get; set; }
    public String route_id { get; set; }
    public String arrival_time { get; set; }
    public int monday { get; set; }
    public int tuesday { get; set; }
    public int wednesday { get; set; }
    public int thursday { get; set; }
    public int friday { get; set; }
    public int saturday { get; set; }
    public int sunday { get; set; }
}
