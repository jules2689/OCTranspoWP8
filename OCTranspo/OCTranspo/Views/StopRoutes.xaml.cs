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
        ObservableCollection<OCRoute> routes;

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

            routesListInit();
        }

        public async void processRouteSummaryForStop(Object sender, UploadStringCompletedEventArgs e)
        {
            string reply = (string)e.Result;
            OCRouteSummaryForStop stop = OCSupport.makeRouteSummary(reply);
            if (stop != null)
            {
                List<OCRoute> routesListObject = stop.Routes;
                List<OCSchedule> schedule = await OCTranspoStopsData.getScheduleForDayAndStop("monday", stop.StopNumber.ToString());
                foreach (OCRoute route in routesListObject)
                {
                    String times = "";
                    schedule.FindAll(delegate(OCSchedule s)
                    {
                        if (s.route_short_name == route.RouteNumber)
                        {
                            times = times + ", " + s.arrival_time;
                            return true;
                        }
                        else
                            return false;
                    });
                    route.fiveArrivalTimes = times;
                }
                routes = new ObservableCollection<OCRoute>(routesListObject);
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
            if (selectedItem is OCRoute)
            {
                OCRoute route = (OCRoute)selectedItem;
                Navigation.NavigateToRoute(route.RouteNumber.ToString(), route.RouteHeading, int.Parse(stopID.Text), stopName.Text, route.RouteHeading);
            }

            ((LongListSelector)sender).SelectedItem = null;
        }
    }
}