using System;

public class OCRoute
{
    public OCRoute(){}

    public static OCRoute newOCRoute(int route_short_name, String route_id, String route_long_name, int route_type)
	{
        OCRoute route = new OCRoute();
        route.route_short_name = route_short_name;
        route.route_id = route_id;
        route.route_type = route_type;
        route.route_long_name = route_long_name;
        return route;
	}

    public int route_short_name { get; set; }
    public String route_id { get; set; }
    public String route_long_name { get; set; }
    public int route_type { get; set; }

    //Variable for View
    public String fiveArrivalTimes { get; set; }
}
