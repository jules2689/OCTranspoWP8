﻿
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
            createFavouritesTable();
            createSettingsTable();
        }

        private static async void createFavouritesTable()
        {
            String path = ApplicationData.Current.LocalFolder.Path + "/OCTranspo.sqlite";
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(path);
            var count = await conn.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='OCDirection'");
            if (count == 0)
            {
                await conn.CreateTableAsync<OCDirection>();
            }
        }

        private static async void createSettingsTable()
        {
            String path = ApplicationData.Current.LocalFolder.Path + "/OCTranspo.sqlite";
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(path);
            var count = await conn.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='OCSettings'");
            if (count == 0)
            {
                await conn.CreateTableAsync<OCSettings>();
                OCSettings settings = OCSettings.newOCSettings(500);
                settings.id = 1;
                await conn.InsertAsync(settings);
            }
        }

        public static async Task<int> updateSettings(OCSettings settings)
        {
            settings.id = 1;
            String path = ApplicationData.Current.LocalFolder.Path + "/OCTranspo.sqlite";
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(path);
            int result = await conn.UpdateAsync(settings);
            return result;
        }

        public static async Task<OCSettings> getSettings()
        {
            String path = ApplicationData.Current.LocalFolder.Path + "/OCTranspo.sqlite";
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(path);
            var count = await conn.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='OCSettings'");
            if (count > 0)
            {
                String Query = "SELECT * from OCSettings LIMIT 1;";
                List<OCSettings> settings = await conn.QueryAsync<OCSettings>(Query);
                return settings.First<OCSettings>();
            }
            else
            {
                createSettingsTable();
                return await getSettings();
            }
        }
        
        // Favourites

        public static async Task<int> addFavouriteStop(OCDirection direction)
        {
            int result = 0;
            String path = ApplicationData.Current.LocalFolder.Path + "/OCTranspo.sqlite";
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(path);
            await conn.InsertAsync(direction).ContinueWith((t) =>
           {
               result = t.Result;
           });
            return result;
        }

        public static async Task<int> deleteFavourite(OCDirection direction)
        {
            String path = ApplicationData.Current.LocalFolder.Path + "/OCTranspo.sqlite";
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(path);
            String Query = "DELETE from OCDirection WHERE routeNo=" + direction.RouteNo + " AND FromStopNumber=" + direction.FromStopNumber + ";";
            int result = await conn.DeleteAsync(direction);
            return result;
        }

        public static async Task<ObservableCollection<OCDirection>> getFavourites()
        {
            //TODO Math.
            String path = ApplicationData.Current.LocalFolder.Path + "/OCTranspo.sqlite";
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(path);
            var count = await conn.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='OCDirection'");
            if (count > 0)
            {
                String Query = "SELECT Id, * from OCDirection;";
                List<OCDirection> stops = await conn.QueryAsync<OCDirection>(Query);
                ObservableCollection<OCDirection> directionsCollection = new ObservableCollection<OCDirection>(stops);
                return directionsCollection;
            }
            else
            {
                createFavouritesTable();
                return await getFavourites();
            }
        }

        // Stops

        public static async Task<ObservableCollection<OCStop>> getCloseStops(double latitude, double longitude, double zoomLevel)
        {
            //TODO Math.
            String path = ApplicationData.Current.LocalFolder.Path + "/OCTranspo.sqlite";
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(path);
            var count = await conn.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='OCStop'");
            if (count > 0)
            {
                OCSettings settings = await getSettings();
                OCGeoMath latlong = OCGeoMath.getRange(latitude, longitude, settings.nearbyDistance);
                String Query = "SELECT DISTINCT(stop_id), * from OCStop where ((stop_lat BETWEEN " + latlong.lowerLat + " AND " + latlong.upperLat + ")" +
                " AND (stop_lon BETWEEN " + latlong.lowerLong + " AND " + latlong.upperLong + ")) ORDER BY stop_id;";

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

        //Schedules

        public static async Task<List<OCStop>> getStopIDForCode(String stop)
        {
            String path = ApplicationData.Current.LocalFolder.Path + "/OCTranspo.sqlite";
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(path);
            var count = await conn.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='OCStop'");
            if (count > 0)
            {
                int date = getTodaysDate();
                String Query = "SELECT stop_id from OCStop where stop_code = '" + stop + "'";
                List<OCStop> codes = await conn.QueryAsync<OCStop>(Query);
                return codes;
            }
            else
            {
                OCTranspoStopsData.initDB();
                return await getStopIDForCode(stop);
            }
        }

        public static async Task<List<OCSchedule>> getScheduleForDayAndStop(String day, String stop, int route)
        {
            String path = ApplicationData.Current.LocalFolder.Path + "/OCTranspo.sqlite";
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(path);
            var count = await conn.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='OCStop'");
            if (count > 0)
            {
                int date = getTodaysDate();
                String Query = "SELECT * from OCSchedule where start_date <= " + date + " AND end_date >= " + date + " AND stop_code = '" + stop + "' AND route_short_name='" + route + "' AND " + day.ToLower() + " = '1' ORDER BY arrival_time";
                List<OCSchedule> schedule = await conn.QueryAsync<OCSchedule>(Query);
                return schedule;
            }
            else
            {
                OCTranspoStopsData.initDB();
                return await getScheduleForDayAndStop(day, stop, route);
            }
        }

        private static int getTodaysDate()
        {
            DateTime date = DateTime.Today;
            String month = date.Month.ToString();
            month = month.Length > 1 ? month : "0" + month;

            String day = date.Day.ToString();
            day = day.Length > 1 ? day : "0" + day;

            String dateString = date.Year.ToString() + month + day;
            return int.Parse(dateString);
        }
    }
}

