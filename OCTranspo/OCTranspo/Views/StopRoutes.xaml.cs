using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using OCTranspo.Models;

namespace OCTranspo.Views
{
    public partial class StopRoutes : PhoneApplicationPage
    {
        ObservableCollection<OCApiRoute> routes;

        public StopRoutes()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string msg = "";

            if (NavigationContext.QueryString.TryGetValue("stopRoute", out msg))
            {
                if (msg.Length == 0)
                {
                    msg = "0";
                }
                stopID.Text = msg;
                OCSupport.getRouteSummaryForStop(int.Parse(msg),new UploadStringCompletedEventHandler(processRouteSummaryForStop));
            }

            if (NavigationContext.QueryString.TryGetValue("stopName", out msg))
                stopName.Text = msg;

            setIsLoading(true);
            routesListInit();
        
        }

        public void setIsLoading(Boolean loading)
        {
            loadingProgress.Visibility = loading ? Visibility.Visible : Visibility.Collapsed;
            loadingText.Visibility = loading ? Visibility.Visible : Visibility.Collapsed;
        }

        public async void processRouteSummaryForStop(Object sender, UploadStringCompletedEventArgs e)
        {
            string reply = (string)e.Result;
            OCRouteSummaryForStop stop = OCSupport.makeRouteSummary(reply);
            if (stop != null)
            {
                List<OCApiRoute> routesListObject = stop.Routes;
               
                foreach (OCApiRoute route in routesListObject)
                {
                    OCApiRoute route1 = await route.fetchTimes(stop.StopNumber.ToString());
                    route.fourArrivalTimes = route1.fourArrivalTimes;
                    route.nextTimes = route1.nextTimes;
                }
                routes = new ObservableCollection<OCApiRoute>(routesListObject);
                setIsLoading(false);
                routesList.ItemsSource = routes;
            }
        }

        private void routesListInit()
        {
            routesList.ItemsSource = routes;
        }

        private void routesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ((LongListSelector)sender).SelectedItem;
            if (selectedItem is OCApiRoute)
            {
                OCApiRoute route = (OCApiRoute)selectedItem;
                Navigation.NavigateToRoute(route.RouteNumber.ToString(), route.RouteHeading, int.Parse(stopID.Text), stopName.Text, route.RouteHeading);
            }

            ((LongListSelector)sender).SelectedItem = null;
        }
    }
}