﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using OCTranspo.Resources;
using OCTranspo.Models;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace OCTranspo
{
    public partial class MainPage : PhoneApplicationPage
    {
        List<OCStop> routes;
        List<OCDirection> favourites;
        List<OCStop> nearbyStops;
        Boolean searching;
        DispatcherTimer searchBoxTimer;
        DispatcherTimer mapTimer;

        // Constructor
        public MainPage()
        {
            searching = false;
            OCTranspoStopsData.initDB();
            InitializeComponent();
            setupLists();
            setupSearchBox();
            setupMap();
        }

        private void setupLists()
        {
            //Set List Data Sources
            getNearbyStops();
            routes = new List<OCStop>();
            favourites = new List<OCDirection>();
            favourites.Add(OCDirection.newOCDirection(18, "QUILL / GLYNN", ".", ".", new List<OCTrip>()));
            favourites.Add(OCDirection.newOCDirection(19, "QUILL / GLYNN", ".", ".", new List<OCTrip>()));
            this.routesList.ItemsSource = routes;
            this.favouritesList.ItemsSource = favourites;
        }

        private void setupSearchBox()
        {
            // Timer for Search Box
            searchBoxTimer = new DispatcherTimer();
            searchBoxTimer.Interval = TimeSpan.FromMilliseconds(500);
            searchBoxTimer.Tick += timer_Tick;
        }

        private void setupMap()
        {
            // Map Initialization
            GeoLocator.centerMapOnCurrentLocation(currentLocation);
            GeoLocator.drawMapMarkers(currentLocation);
            
            // Timer for Map
            mapTimer = new DispatcherTimer();
            mapTimer.Interval = TimeSpan.FromMilliseconds(800);
            mapTimer.Tick += mapTimer_Tick;
        }

        //Nearby List Methods***********************************************************************************************************************************************************//

        private async void getNearbyStops()
        {
            nearbyStops = await OCTranspoStopsData.getCloseStops(currentLocation.Center.Latitude, currentLocation.Center.Longitude, currentLocation.ZoomLevel);
            this.nearbyList.ItemsSource = nearbyStops;
        }


        //Maps Methods***********************************************************************************************************************************************************//

        private void currentLocation_CenterChanged(object sender, Microsoft.Phone.Maps.Controls.MapCenterChangedEventArgs e)
        {
            mapTimer.Stop();
            mapTimer.Start();
        }

        private void mapTimer_Tick(Object sender, EventArgs args)
        {
            if (searching == false)
            {
                mapTimer.Stop();
                searching = true;
                //TODO: Called too much, don't get a chance to draw markers before change.
                GeoLocator.drawMapMarkers(currentLocation);
                searching = false;
            }
        }


        //Search Box Methods***********************************************************************************************************************************************************//

        private void searchBoxText_Changed(object sender, TextChangedEventArgs e)
        {
            searchBoxTimer.Stop();
            searchBoxTimer.Start();
        }

        async void timer_Tick(Object sender, EventArgs args)
        {
            searchBoxTimer.Stop(); 
            searchProgressBar.Visibility = Visibility.Visible;
            String query = routesSearch.Text;
            routes = await OCTranspoStopsData.getStopByNameOrID(query);
            searchHasItems(routes.Count > 0);
            this.routesList.ItemsSource = routes;
            searchBoxTimer.Stop();
            searchProgressBar.Visibility = Visibility.Collapsed;
        }

        private void searchHasItems(Boolean hasItems)
        {
            searchSmile.Visibility = hasItems ? Visibility.Collapsed : Visibility.Visible;
            searchText.Visibility = hasItems ? Visibility.Collapsed : Visibility.Visible;
        }

        private void OnSearchBoxGotFocus(object sender, RoutedEventArgs e)
        {
            if (routesSearch.Text.Equals("Stop Number / Stop Name", StringComparison.OrdinalIgnoreCase))
            {
                routesSearch.Text = string.Empty;
            }
        }

        private void OnSearchBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(routesSearch.Text))
            {
                routesSearch.Text = "Stop Number / Stop Name";
                searchHasItems(false);
            }
        }

        //Routes List Methods***********************************************************************************************************************************************************//

        private void routesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ((LongListSelector)sender).SelectedItem;
            if (selectedItem is OCDirection)
            {
                OCDirection stop = (OCDirection)selectedItem;
                Navigation.NavigateToStop(stop.RouteNo.ToString(), stop.RouteLabel);
            }
            else if (selectedItem is OCStop)
            {
                OCStop stop = (OCStop)selectedItem;
                Navigation.NavigateToStop(stop.StopID.ToString(), stop.StopDesc);
            }
            ((LongListSelector)sender).SelectedItem = null;
        }

    }
}