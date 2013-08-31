
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SQLite;
using System.Collections.ObjectModel;
using Windows.Storage;

namespace OCTranspo.Models
{
	class OCTranspoStopsData
	{

        public static void initDB()
        {
           OCZipFile.UnZipOCTranspo();
        }

        public static async Task<ObservableCollection<OCStop>> getCloseStops(double latitude, double longitude, double zoomLevel)
        {
            //TODO Math.
            String path = ApplicationData.Current.LocalFolder.Path + "/OCTranspo.sqlite";
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(path);
            var count = await conn.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='OCStop'");
            if (count > 0)
            {
                double lowerBound = 0.12 / zoomLevel;
                double upperBound = 0.12 / zoomLevel;
                String Query = "SELECT DISTINCT(stop_id), * from OCStop where ((stop_lat BETWEEN " + (latitude - lowerBound) + " AND " + (latitude + upperBound) + ")" +
                " AND (stop_lon BETWEEN " + (longitude - lowerBound) + " AND " + (longitude + upperBound) + ")) ORDER BY stop_id;";
               
                List<OCStop> stops = await conn.QueryAsync<OCStop>(Query);
                ObservableCollection<OCStop> stopsCollection = new ObservableCollection<OCStop>(stops);
                return stopsCollection;
            }
            else
            {
                OCTranspoStopsData.initDB();
                return await getCloseStops(latitude, longitude, zoomLevel);
            }
        }

        public static async Task<List<OCStop>> getStopByNameOrID(String value)
        {
            String path = ApplicationData.Current.LocalFolder.Path + "/OCTranspo.sqlite";
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(path);
            var count = await conn.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='OCStop'");
            if (count > 0)
            {
                if (value.Length > 0)
                {
                    String Query = "SELECT DISTINCT(stop_id), * from OCStop where stop_name LIKE '%" + value.ToUpper() + "%' OR stop_code LIKE '" + value + "%';";
                    List<OCStop> stops = await conn.QueryAsync<OCStop>(Query);
                    return stops;
                }
                else
                {
                    return new List<OCStop>();
                }
            }
            else
            {
                OCTranspoStopsData.initDB();
                return await getStopByNameOrID(value);
            }
        }

	}
}
