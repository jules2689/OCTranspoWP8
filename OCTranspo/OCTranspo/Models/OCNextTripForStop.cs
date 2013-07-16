using System;
using System.Collections.Generic;

public class OCNextTripForStop
{
    public OCNextTripForStop(){}

    public void newOCNextTripForStop(int stopNo, String stopLabel, List<OCDirection> directions)
	{
        this.StopNo = stopNo;
        this.StopLabel = stopLabel;
        this.Directions = directions;
	}

    public int StopNo { get; set;  }
    public String StopLabel { get; set; }
    public List<OCDirection> Directions { get; set; }

}
