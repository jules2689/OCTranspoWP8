using System;
using System.Collections.Generic;

public class OCCalendar
{
	public OCCalendar(){}

    public static OCCalendar newOCCalendar(String service_id, int monday, int tuesday, int wednesday, int thursday, int friday, int saturday, int sunday, int start_date, int end_date)
    {
        OCCalendar cal = new OCCalendar();
        cal.service_id = service_id;
        cal.monday = monday;
        cal.tuesday = tuesday;
        cal.wednesday = wednesday;
        cal.thursday = thursday;
        cal.friday = friday;
        cal.saturday = saturday;
        cal.sunday = sunday;
        cal.start_date = start_date;
        cal.end_date = end_date;
        return cal;
    }

    public String service_id { get; set; }
    public int monday { get; set; }
    public int tuesday { get; set; }
    public int wednesday { get; set; }
    public int thursday { get; set; }
    public int friday { get; set; }
    public int saturday { get; set; }
    public int sunday { get; set; }
    public int start_date { get; set; }
    public int end_date { get; set; }
}
