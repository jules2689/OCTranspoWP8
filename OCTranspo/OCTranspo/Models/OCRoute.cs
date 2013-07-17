using System;

public class OCRoute
{
    public OCRoute(){}

    public static OCRoute newOCRoute(int routeNo, int directionId, String direction, String routeHeading)
	{
        OCRoute route = new OCRoute();
        route.RouteNumber = routeNo;
        route.DirectionID = directionId;
        route.Direction = direction;
        route.RouteHeading = routeHeading;
        return route;
	}

    public int RouteNumber { get; set; }
    public int DirectionID { get; set; }
    public String Direction { get; set; }
    public String RouteHeading { get; set; }
}
