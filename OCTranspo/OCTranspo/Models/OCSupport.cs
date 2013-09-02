using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Windows;

public class OCSupport
{
    private static String apiKey = "4515ce0602a00692b763179dfd6ca589";
    private static String appID = "08f6faa1";
    private static String urlBase = "https://api.octranspo1.com/v1.1/";
    private static String urlRouteSum = "GetRouteSummaryForStop";
    private static String urlNextTrip = "GetNextTripsForStop";
    private static String argsBase = "appID=" + appID + "&apiKey=" + apiKey;

   

    // API Calls - Posts Methods (Public Access Methods)

    public static async void getRouteSummaryForStop(int stop, UploadStringCompletedEventHandler handler)
    {
        try
        {
            await Post(new Dictionary<String, String>(), argsBase, stop, -1, handler);
        }
        catch
        {
            MessageBox.Show("There was an issue getting data, is your data connection working?");
        }
    }
    public static async void getNextTripForStop(int stop, int route, UploadStringCompletedEventHandler handler)
    {
        await Post(new Dictionary<String, String>(), argsBase, stop, route, handler);
    }

    private static async Task<XDocument> Post(Dictionary<string, string> headers, string data, int stop, int route, UploadStringCompletedEventHandler handler)
    {
        var webClient = new WebClient();
        String uriString = urlBase + (route == -1 ? urlRouteSum : urlNextTrip);
        data = data + "&stopNo=" + stop;
        if (route != -1)
        {
            data = data + "&routeNo=" + route;
        }

        var uri = new Uri(uriString);

        if (headers != null)
        {
            foreach (var key in headers.Keys)
            {
                webClient.Headers[key] = headers[key];
            }
        }

        return await Post(webClient, uri, data, handler);
    }

    private static Task<XDocument> Post(WebClient webClient, Uri uri, string data, UploadStringCompletedEventHandler handler)
    {

        var taskCompletionSource = new TaskCompletionSource<XDocument>();

        webClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";

        webClient.UploadStringCompleted += handler;

        webClient.UploadStringAsync(uri, data);
        return taskCompletionSource.Task;
    }

    //Document Parsers (Public Access Methods)

    public static OCRouteSummaryForStop makeRouteSummary(String xml)
    {
        try
        {
            XElement root = XDocument.Parse(removeSOAPWrapper(xml)).Root;
            int stopNo;
            String stopLabel;
            XElement route;

            stopNo = int.Parse(root.Element("StopNo").Value);
            stopLabel = root.Element("StopDescription").Value;
            route = root.Element("Routes");

            return OCRouteSummaryForStop.newOCRouteSummaryForStop(stopNo, stopLabel, makeRoute(route));
        }
        catch
        {
           MessageBox.Show("There was an issue getting your data, please try again.");
            return null;
        }
    }


    public static OCNextTripForStop makeNextTrip(String xml)
    {
        try
        {
            XElement root = XDocument.Parse(removeSOAPWrapper(xml)).Root;
            int stopNo;
            String stopLabel;
            XElement dir;

            stopNo = int.Parse(root.Element("StopNo").Value);
            stopLabel = root.Element("StopLabel").Value;
            dir = root.Element("Route");

            return OCNextTripForStop.newOCNextTripForStop(stopNo, stopLabel, makeDirection(dir));
        }
        catch
        {
            MessageBox.Show("There was an issue getting your data, please try again.");
            return null;
        }
    }


    //Element Parsers

    private static List<OCApiRoute> makeRoute(XElement routes)
    {
        List<OCApiRoute> rts = new List<OCApiRoute>();
        int routeNo, dirId;
        String dir, heading, routeID;

        foreach (XElement route in routes.Elements())
        {
            routeNo = int.Parse(route.Element("RouteNo").Value);
            routeID = "";
            dirId = int.Parse(route.Element("DirectionID").Value);
            dir = route.Element("Direction").Value;
            heading = route.Element("RouteHeading").Value;
            OCApiRoute routeObject = OCApiRoute.newOCApiRoute(routeNo, routeID, dirId, dir, heading);
            routeObject.BusName = heading.ToUpper();
            rts.Add(routeObject);
        }

        return rts;
    }

    private static List<OCDirection> makeDirection(XElement routeDir) 
    {
        List<OCDirection> dirs = new List<OCDirection>();
        int routeNo;
        String routeLabel, direction, procTime;
        XElement trips;

        foreach (XElement dir in routeDir.Elements())
        {
            routeNo = int.Parse(dir.Element("RouteNo").Value);
            routeLabel = dir.Element("RouteLabel").Value;
            direction = dir.Element("Direction").Value;
            procTime = dir.Element("RequestProcessingTime").Value;
            trips = dir.Element("Trips");
            dirs.Add(OCDirection.newOCDirection(routeNo, routeLabel, direction, procTime,0));
        }

        return dirs;
    }

    private static List<OCAPITrip> makeTrips(XElement trip)
    {
        List<OCAPITrip> trips = new List<OCAPITrip>();
        String destination, startTime, busType, tmpGPS, tmpLat, tmpLon;
        int adjustTime;
        bool last;
        double adjustAge, gpsSpeed, lat, lon;

        foreach (XElement tr in trip.Elements())
        {
            destination = tr.Element("TripDestination").Value;
            startTime = tr.Element("TripStartTime").Value;
            adjustTime = int.Parse(tr.Element("AdjustedScheduleTime").Value);
            adjustAge = double.Parse(tr.Element("AdjustmentAge").Value);
            last = tr.Element("LastTripOfSchedule").Value == "true";
            busType = tr.Element("BusType").Value;
            tmpGPS = tr.Element("GPSSpeed").Value;
            tmpLat = tr.Element("Latitude").Value;
            tmpLon = tr.Element("Longitude").Value;

            gpsSpeed = tmpGPS == "" ? 0 : double.Parse(tmpGPS);
            lat = tmpLat == "" ? 0 : double.Parse(tmpLat);
            lon = tmpLon == "" ? 0 : double.Parse(tmpLon);

            trips.Add(OCAPITrip.newOCAPITrip(destination, startTime, adjustTime, adjustAge, busType, gpsSpeed, lat, lon, last));
        }

        return trips;
    }

    // XML Helpers

    private static String removeSOAPWrapper(String xml)
    {
        List<String> build = new List<String>();
        String[] XML;
        String results = "";
        int i = 0, len = xml.Length;
        bool body = false;
        
        xml = Regex.Replace(xml, "><", ">XMLBREAKER<");
        XML = Regex.Split(xml, "XMLBREAKER");

        for (int j = 0; j < XML.Length; j++)
        {
            XML[j] = Regex.Replace(XML[j], " xmlns=.*?[^/]>", ">");
            XML[j] = Regex.Replace(XML[j], " xmlns=..*?/>", "/>");
        }

        while (i < len)
        {
            if (Regex.IsMatch(XML[i], "</Get..*Response.*>")) break;
            if (body) build.Add(XML[i]);

            if (!body && Regex.IsMatch(XML[i], "Get..*Response..*")) body = true;

            i++;
        }

        foreach (String elem in build)
        {
            results += elem;
        }

        return results;
    }
}
