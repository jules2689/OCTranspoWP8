using System;
using System.Collections.Generic;

public class OCRouteSummaryForStop
{
    public OCRouteSummaryForStop() { }
    public static OCRouteSummaryForStop newOCRouteSummaryForStop(int stopNo, String stopDescription, List<OCRoute> routes)
	{
        OCRouteSummaryForStop summary = new OCRouteSummaryForStop();
        summary.StopNumber = stopNo;
        summary.StopName = stopDescription;
        summary.Routes = routes;
        return summary;
	}

    public int StopNumber { get; set; }
    public String StopName { get; set; }
    public List<OCRoute> Routes { get; set; }
}
