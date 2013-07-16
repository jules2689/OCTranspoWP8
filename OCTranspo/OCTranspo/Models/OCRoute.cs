using System;

public class OCRoute
{
    public OCRoute(){}

    public void newOCRoute(int routeNo, int directionId, String direction, String routeHeading)
	{
        this.RouteNumber = routeNo;
        this.DirectionID = directionId;
        this.Direction = direction;
        this.RouteHeading = routeHeading;
	}

    public int RouteNumber { get; set; }
    public int DirectionID { get; set; }
    public String Direction { get; set; }
    public String RouteHeading { get; set; }
}
