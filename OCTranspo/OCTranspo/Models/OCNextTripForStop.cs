using System;
using System.Collections.Generic;

public class OCNextTripForStop
{
    public OCNextTripForStop(){}

    public static OCNextTripForStop newOCNextTripForStop(int stopNo, String stopLabel, List<OCDirection> directions)
	{
        OCNextTripForStop direction = new OCNextTripForStop();
        direction.StopNo = stopNo;
        direction.StopLabel = stopLabel;
        direction.Directions = directions;
        return direction;
	}

    public int StopNo { get; set;  }
    public String StopLabel { get; set; }
    public List<OCDirection> Directions { get; set; }

}
