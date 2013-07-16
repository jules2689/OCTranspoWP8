using System;
using System.Collections.Generic;

public class OCRouteSummaryForStop
{
    public OCRouteSummaryForStop() { }
    public void newOCRouteSummaryForStop(int stopNo, String stopDescription, List<OCRoute> routes)
	{
        this.StopNumber = stopNo;
        this.StopName = stopDescription;
        this.Routes = routes;
	}

    public int StopNumber { get; set; }
    public String StopName { get; set; }
    public List<OCRoute> Routes { get; set; }
}
