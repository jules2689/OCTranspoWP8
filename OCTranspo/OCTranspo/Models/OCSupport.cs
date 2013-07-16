using System;

public class OCSupport
{
    private static String apiKey = "4515ce0602a00692b763179dfd6ca589";
    private static String appID = "08f6faa1";
    private static String urlBase = "https://api.octranspo1.com/v1.1/";
    private static String urlRouteSum = "GetRouteSummaryForStop";
    private static String urlNextTrip = "GetNextTripsForStop";
    private String argsBase = "appID=" + appID + "&apiKey=" + apiKey;

	public OCSupport()
	{
	}

    public void OCAPICall() { }
    public OCRouteSummaryForStop makeRouteSummary(String xml) { return null; }
    public OCNextTripForStop makeNextTrip(String xml) { return null; }
}
