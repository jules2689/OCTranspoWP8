using System;
using System.Collections.Generic;

public class OCDirection
{

	public OCDirection(){}

    public static OCDirection newOCDirection(int routeNo, String routeLabel, String direction, String procTime, List<OCTrip> trips)
    {
        OCDirection dir = new OCDirection();
        dir.RouteNo = routeNo;
        dir.RouteLabel = routeLabel;
        dir.Direction = direction;
        dir.ProcTime = procTime;
        dir.Trips = trips;
        return dir;
    }

    public int RouteNo { get; set; }
    public String RouteLabel { get; set; }
    public String Direction { get; set; }
    public String ProcTime { get; set; }
    public List<OCTrip> Trips { get; set; }
}
