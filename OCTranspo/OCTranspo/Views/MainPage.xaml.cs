using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Windows.Devices.Geolocation;

namespace OCTranspo
{
    public partial class MainPage : PhoneApplicationPage
    {
        List<OCStop> routes;
        ObservableCollection<OCDirection> favourites;
        ObservableCollection<OCStop> nearbyStops;
        Boolean searching;
        DispatcherTimer searchBoxTimer;
        DispatcherTimer mapTimer;

        // Constructor
        public MainPage()
        {
            searching = false;
            ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)Resources["DefaultAppBar"];
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
            favourites = new ObservableCollection<OCDirection>();
            this.routesList.ItemsSource = routes;
            this.favouritesList.ItemsSource = favourites;
            OCSupport.getNextTripForStop(3009, 95, new UploadStringCompletedEventHandler(processGetNextTripForStop));
           // OCSupport.getRouteSummaryForStop(3009, new UploadStringCompletedEventHandler(processGetRouteSummaryForStop)); 
        }

        public void processGetRouteSummaryForStop(Object sender, UploadStringCompletedEventArgs e)
        {
            string reply = (string)e.Result;
            OCRouteSummaryForStop stop = OCSupport.makeRouteSummary(reply);
            Console.WriteLine(reply);
        }

        public void processGetNextTripForStop(Object sender, UploadStringCompletedEventArgs e)
        {
            try
            {
                loadingProgressBar.IsVisible = true;
                loadingProgressBar.Text = "Loading favourites data ...";
                string reply = (string)e.Result;
                OCNextTripForStop stop = OCSupport.makeNextTrip(reply);
                if (stop != null)
                {
                    favourites.Clear();
                    foreach (OCDirection direction in stop.Directions)
                    {
                        direction.FromStopName = stop.StopLabel;
                        direction.FromStopNumber = stop.StopNo;
                        direction.DirectionalName = "to " + direction.RouteLabel.ToUpper();
                        favourites.Add(direction);
                    }
                    this.favouritesList.ItemsSource = favourites;
                    setFavouriteErrorMessage(false, favourites.Count > 0);
                    loadingProgressBar.IsVisible = false;
                }
                else
                {
                    loadingProgressBar.IsVisible = false;
                    setFavouriteErrorMessage(true, false);
                }
            }
            catch
            {
                loadingProgressBar.IsVisible = false;
                setFavouriteErrorMessage(true, false);
                MessageBox.Show("There was an issue getting your data! Please check your data connection and try again.");
            }
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
             try
             {
                 Geocoordinate myCoordinate = await GeoLocator.getMyLocation();
                 if (myCoordinate != null)
                 {
                     nearbyStops = await OCTranspoStopsData.getCloseStops(myCoordinate.Latitude, myCoordinate.Longitude, currentLocation.ZoomLevel);
                     this.nearbyList.ItemsSource = nearbyStops;

                     setNearbyErrorMessage(true, nearbyStops.Count > 0);
                 }
                 else
                 {
                     nearbySorry.Visibility = nearbyStops.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                     nearbyFrown.Visibility = nearbyStops.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                     setNearbyErrorMessage(true, false);
                 }
             }
             catch
             {
                 setNearbyErrorMessage(false, false);
             }
        }
        //Sorry, there was an issue retrieving your favourites. Check your data connection and try again.
        private void setNearbyErrorMessage(bool GPSEnabled, bool foundItems)
        {
            if (!GPSEnabled)
            {
                nearbySorry.Text = "Sorry, your GPS is not enabled!";
                nearbySorry.Visibility = Visibility.Visible;
                nearbyFrown.Visibility = Visibility.Visible;
            } else if (GPSEnabled && !foundItems)
            {
                nearbySorry.Text = "Sorry, we couldn't find any stops close to you!";
                nearbySorry.Visibility = Visibility.Visible;
                nearbyFrown.Visibility = Visibility.Visible;
            }
            else if (GPSEnabled && foundItems)
            {
                nearbySorry.Text = "Sorry, we couldn't find any stops close to you!";
                nearbySorry.Visibility = Visibility.Collapsed;
                nearbyFrown.Visibility = Visibility.Collapsed;
            }
            else
            {
                nearbySorry.Visibility = Visibility.Collapsed;
                nearbyFrown.Visibility = Visibility.Collapsed;
            }
        }

        private void setFavouriteErrorMessage(bool error, bool foundItems)
        {
            if (error)
            {
                favouritesSorry.Text = "Sorry, there was an issue retrieving your favourites. Check your data connection and try again.";
                favouritesSorry.Visibility = Visibility.Visible;
                faveFrowny.Visibility = Visibility.Visible;
            }
            else if (!error && !foundItems)
            {
                favouritesSorry.Text = "You have no favourites yet, try adding a route you use often for quick access!";
                favouritesSorry.Visibility = Visibility.Collapsed;
                faveFrowny.Visibility = Visibility.Visible;
            }
            else
            {
                favouritesSorry.Text = "Sorry, there was an issue retrieving your favourites. Check your data connection and try again.";
                favouritesSorry.Visibility = Visibility.Collapsed;
                faveFrowny.Visibility = Visibility.Collapsed;
            }
        }

        //Maps Methods***********************************************************************************************************************************************************//

        private void currentLocation_ZoomLevelChanged(object sender, Microsoft.Phone.Maps.Controls.MapZoomLevelChangedEventArgs e)
        {
            loadingProgressBar.Text = "Loading map data ...";
            loadingProgressBar.IsVisible = true;
            mapTimer.Stop();
            mapTimer.Start();
        }

        private void currentLocation_CenterChanged(object sender, Microsoft.Phone.Maps.Controls.MapCenterChangedEventArgs e)
        {
            loadingProgressBar.Text = "Loading map data ...";
            loadingProgressBar.IsVisible = true;
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
                loadingProgressBar.IsVisible = false;
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
            searchHasItems(routes.Count > 0, query.Length > 0);
            this.routesList.ItemsSource = routes;
            searchBoxTimer.Stop();
            searchProgressBar.Visibility = Visibility.Collapsed;
        }

        private void searchHasItems(bool hasItems, bool hasQuery)
        {
            if (!hasItems && hasQuery)
            {
                searchText.Text = "Sorry, we couldn't find a stop with that name or code, please try another search.";
                searchText.Visibility = Visibility.Visible;
                searchSmile.Visibility = Visibility.Collapsed;
                searchFrowny.Visibility = Visibility.Visible;
            }
            else if (!hasItems && !hasQuery)
            {
                searchText.Text = "Search for a stop by it's number or name simply by tapping above.";
                searchText.Visibility = Visibility.Visible;
                searchFrowny.Visibility = Visibility.Collapsed;
                searchSmile.Visibility = Visibility.Visible;
            }
            else if (hasItems)
            {
                searchText.Visibility = Visibility.Collapsed;
                searchFrowny.Visibility = Visibility.Collapsed;
                searchSmile.Visibility = Visibility.Collapsed;
            }
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
                searchHasItems(false, false);
            }
        }

        //Routes List Methods***********************************************************************************************************************************************************//

        private void routesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ((LongListSelector)sender).SelectedItem;
            if (selectedItem is OCDirection)
            {
                OCDirection stop = (OCDirection)selectedItem;
                Navigation.NavigateToStopRoute(stop.RouteNo.ToString(), stop.RouteLabel);
            }
            else if (selectedItem is OCStop)
            {
                OCStop stop = (OCStop)selectedItem;
                Navigation.NavigateToStopRoute(stop.stop_code.ToString(), stop.stop_name);
            }
            ((LongListSelector)sender).SelectedItem = null;
        }

        private void ApplicationBarMenuItem_Click_1(object sender, EventArgs e)
        {
            Navigation.NavigateToInfo();
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
                loadingProgressBar.Text = "Loading map data ...";
                loadingProgressBar.IsVisible = true;
                mapTimer.Stop();
                searching = true;
                //TODO: Called too much, don't get a chance to draw markers before change.
                GeoLocator.centerMapOnCurrentLocation(currentLocation);
                GeoLocator.drawMapMarkers(currentLocation);
                getNearbyStops();
                searching = false;
                loadingProgressBar.IsVisible = false;
        }

        private void Pivot_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            switch (((Pivot)sender).SelectedIndex)
            {
                case 0:
                    ApplicationBar = ((ApplicationBar)Application.Current.Resources["AppBarFavourites"]);
                    ApplicationBarIconButton newButton1 = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
                    ApplicationBarIconButton pinButton1 = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
                    break;

                case 1:
                    ApplicationBar = ((ApplicationBar)Application.Current.Resources["AppBarNearby"]);
                    ApplicationBarIconButton findButton2 = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
                    findButton2.Click += ApplicationBarIconButton_Click_1;
                    ApplicationBarIconButton pinButton2 = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
                    break;

                case 2:
                    ApplicationBar = ((ApplicationBar)Application.Current.Resources["AppBarMap"]);
                    ApplicationBarIconButton findButton3 = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
                    findButton3.Click += ApplicationBarIconButton_Click_1;
                    ApplicationBarIconButton pinButton3 = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
                    break;

                case 3:
                    ApplicationBar = ((ApplicationBar)Application.Current.Resources["AppBarStops"]);
                    ApplicationBarIconButton pinButton4 = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
                    break;
            }
        }
    }
}