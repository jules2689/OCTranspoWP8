using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class OCSettings
{
    public OCSettings() { }
    public static OCSettings newOCSettings(int nearbyDistance)
    {
        OCSettings settings = new OCSettings();
        settings.nearbyDistance = nearbyDistance;
        return settings;
    }
    public int nearbyDistance { get; set; }
    [SQLite.PrimaryKey]
    public int id { get; set; }
}

