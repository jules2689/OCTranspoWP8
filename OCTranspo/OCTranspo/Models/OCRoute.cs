using System;

public class OCRoute
{
    public OCRoute(){}

    public static OCRoute newOCRoute(int routeNo, String routeID, int directionId, String direction, String routeHeading)
	{
        OCRoute route = new OCRoute();
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
    public String fiveArrivalTimes { get; set; }
}
