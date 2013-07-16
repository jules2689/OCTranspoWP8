using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using OCTranspo.Models;
using Microsoft.Phone.Maps.Toolkit;

namespace OCTranspo
{
    class GeoLocator
    {
        //TODO: Bounding Locations 

        public static async void centerMapOnCurrentLocation(Map map)
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;

            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10)
                    );

                map.Center = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);
            }
            catch (Exception ex) {}
        }

        public static async void drawMapMarkers(Map map)
        {
            if (map.ZoomLevel > 12)
            {
                map.Layers.Clear();
                MapLayer mapLayer = new MapLayer();
                List<OCStop> stops = await OCTranspoStopsData.getCloseStops(map.Center.Latitude, map.Center.Longitude, map.ZoomLevel);
                foreach (OCStop stop in stops)
                {
                    drawMapMarker(stop, Color.FromArgb(255,210,30,0), mapLayer, true);

                }
                layMyLocation(map, mapLayer);
                map.Layers.Add(mapLayer);
            }
        }

        private static void layMyLocation(Map map, MapLayer mapLayer)
        {
            Ellipse e = new Ellipse();
            Color blue = Color.FromArgb(255, 0, 119, 210);
            e.StrokeThickness = 5;
            e.Stroke = new SolidColorBrush(blue);
            e.Fill = new SolidColorBrush(blue);
            e.Fill.Opacity = 0.9;
            e.Height = e.Width = 20;

            layMarker(e, map.Center.Latitude, map.Center.Longitude, mapLayer);
        }
        private static void drawMapMarker(OCStop stop, Color color, MapLayer mapLayer, Boolean action)
        {
            // Create a map marker
            Pushpin pin = new Pushpin();
            pin.Background = new SolidColorBrush(color);
            pin.GeoCoordinate = new GeoCoordinate(stop.StopLat, stop.StopLong);
            pin.Name = stop.StopID + " - " + stop.StopDesc;
            if (action)
            {
                pin.Tap += mapPinTapped;
            }
            layMarker(pin, stop.StopLat, stop.StopLong, mapLayer);
            
        }

        private static void layMarker(Object pin, double latitude, double longitude, MapLayer mapLayer)
        {
            // Create a MapOverlay and add marker
            MapOverlay overlay = new MapOverlay();
            overlay.Content = pin;
            overlay.GeoCoordinate = new GeoCoordinate(latitude, longitude);
            overlay.PositionOrigin = new Point(0.0, 1.0);
            mapLayer.Add(overlay);
        }

        private static void mapPinTapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Pushpin pin = (Pushpin)sender;
            String[] pinAttr = pin.Name.Split('-');
            Navigation.NavigateToStop(pinAttr[0], pinAttr[1]);
        }

    }
}
