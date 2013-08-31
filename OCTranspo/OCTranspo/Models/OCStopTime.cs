using System;

public class OCStopTime
{
    public OCStopTime() { }

    public static OCStopTime newOCStopTime(String tripId, String stopId, int stopSequence, int pickupType, int dropOffType, String arrivalTime, String departTime)
	{
        OCStopTime stopTime = new OCStopTime();
        stopTime.tripId = tripId;
        stopTime.stopId = stopId;
        stopTime.stopSequence = stopSequence;
        stopTime.pickupType = pickupType;
        stopTime.dropOffType = dropOffType;
        stopTime.arrivalTime = arrivalTime;
        stopTime.departTime = departTime;
        return stopTime;
	}

    public String tripId { get; set; }
    public String stopId { get; set; }
    public int stopSequence { get; set; }
    public int pickupType { get; set; }
    public int dropOffType { get; set; }
    public String arrivalTime { get; set; }
    public String departTime { get; set; }
}
