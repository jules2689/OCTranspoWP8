using System;
using OCTranspo.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class OCApiRoute
{
    public OCApiRoute() { }

    public static OCApiRoute newOCApiRoute(int routeNo, String routeID, int directionId, String direction, String routeHeading)
	{
        OCApiRoute route = new OCApiRoute();
        route.RouteNumber = routeNo;
        route.RouteID = routeID;
        route.DirectionID = directionId;
        route.Direction = direction;
        route.RouteHeading = routeHeading;
        return route;
	}

    public int RouteNumber { get; set; }
    public String RouteID { get; set; }
    public int DirectionID { get; set; }
    public String Direction { get; set; }
    public String RouteHeading { get; set; }
    public String BusName { get; set; }

    //Variable for View
    public String fourArrivalTimes { get; set; }
    public String nextTimes { get; set; }

    public async Task<OCApiRoute> fetchTimes(String stopID)
    {
        String times = "";
        String fourTimes = "";

        DateTime now = DateTime.Now;
        String originalDate = "";
        List<OCSchedule> schedules = await OCTranspoStopsData.getScheduleForDayAndStop(now.DayOfWeek.ToString(), stopID, this.RouteNumber);
        int idx = 0;
        foreach (OCSchedule schedule in schedules)
        {
            try
            {
                int hour = int.Parse(schedule.arrival_time.Substring(0,2));
                if (hour >= 24)
                {
                    hour = hour - 24;
                    originalDate = " 0" + hour + ":" + schedule.arrival_time.Substring(3);
                    schedule.arrival_time = DateTime.Now.AddDays(1).ToShortDateString() + " 0" + hour + ":" + schedule.arrival_time.Substring(3);
                }
                DateTime date = DateTime.Parse(schedule.arrival_time);
                if (date.CompareTo(now) > 0)
                {
                    if (originalDate.Length > 0) schedule.arrival_time = originalDate;
                    if (idx < 3)
                    {
                        times = times + " " + schedule.arrival_time.Substring(0, schedule.arrival_time.Length - 3);
                    }
                    else
                    {
                        fourTimes = fourTimes + " " + schedule.arrival_time.Substring(0, schedule.arrival_time.Length - 3);
                    }
                    idx++;
                    if (idx > 6)
                        break;
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
        }
        fourTimes = fourTimes.Length > 0 ? fourTimes.Substring(1) : fourTimes;
        times = times.Length > 0 ? times.Substring(1) : times;
        if (times.Length == 0) times = "There are no more stops today.";
        this.fourArrivalTimes = fourTimes;
        this.nextTimes = times;
        return this;
    }
}
