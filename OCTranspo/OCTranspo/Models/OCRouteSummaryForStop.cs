using System;
using System.Collections.Generic;

public class OCRouteSummaryForStop
{
    public OCRouteSummaryForStop() { }
    public static OCRouteSummaryForStop newOCRouteSummaryForStop(int stopNo, String stopDescription, List<OCApiRoute> routes)
	{
        OCRouteSummaryForStop summary = new OCRouteSummaryForStop();
        summary.StopNumber = stopNo;
        summary.StopName = stopDescription;
        summary.Routes = routes;
        return summary;
	}

    public int StopNumber { get; set; }
    public String StopName { get; set; }
    public List<OCApiRoute> Routes { get; set; }
}
