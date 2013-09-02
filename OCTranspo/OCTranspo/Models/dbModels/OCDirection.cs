using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using OCTranspo.Models;
using System.Threading.Tasks;

public class OCDirection
{

	public OCDirection(){}
    
    public static OCDirection newOCDirection(int routeNo, String routeLabel, String direction, String procTime, int Id)
    {
        OCDirection dir = new OCDirection();
        dir.RouteNo = routeNo;
        dir.RouteLabel = routeLabel;
        dir.Direction = direction;
        dir.ProcTime = procTime;
        dir.Id = Id;
        return dir;
    }

    [SQLite.PrimaryKey, SQLite.AutoIncrement]
    public int Id { get; set; }

    public int RouteNo { get; set; }
    public String RouteLabel { get; set; }
    public String Direction { get; set; }
    public String ProcTime { get; set; }

    //Other Attributes for Views
    public String FromStopName { get; set; }
    public int FromStopNumber { get; set; }
    public String DirectionalName { get; set; }

    //Variable for View
    public String fourArrivalTimes { get; set; }
    public String nextTimes { get; set; }

    //TODO :GENERALIZE/Abstract
    public async Task<OCDirection> fetchTimes(String stopID)
    {
        String times = "";
        String fourTimes = "";

        DateTime now = DateTime.Now;
        String originalDate = "";
        List<OCSchedule> schedules = await OCTranspoStopsData.getScheduleForDayAndStop(now.DayOfWeek.ToString(), stopID, this.RouteNo);
        int idx = 0;
        foreach (OCSchedule schedule in schedules)
        {
            if (schedule.arrival_time.StartsWith("24"))
            {
                originalDate = schedule.arrival_time;
                schedule.arrival_time = DateTime.Now.AddDays(1).ToShortDateString() + " 00:" + schedule.arrival_time.Substring(3);
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
        fourTimes = fourTimes.Length > 0 ? fourTimes.Substring(1) : fourTimes;
        times = times.Length > 0 ? times.Substring(1) : times;
        if (times.Trim().Length == 0) times = "Sorry, there are no more stops today.";
        this.fourArrivalTimes = fourTimes;
        this.nextTimes = times;
        return this;
    }
}
