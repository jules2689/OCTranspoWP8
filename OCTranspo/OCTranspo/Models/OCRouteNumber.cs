using System;

public class OCRouteNumber
{
    public OCRouteNumber() { }

    public static OCRouteNumber newOCRoute(int routeNo, String routeID, int routeType)
	{
        OCRouteNumber route = new OCRouteNumber();
        route.RouteNumber = routeNo;
        route.RouteID = routeID;
        route.RouteType = routeType;
        return route;
	}

    [SQLite.Indexed]
    public int RouteNumber { get; set; }
    public int RouteType { get; set; }
    public String RouteID { get; set; }
}
