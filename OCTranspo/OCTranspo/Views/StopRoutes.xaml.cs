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
using System.ComponentModel;
using System.Collections.Specialized;

namespace OCTranspo.Views
{
    public struct StopAndRoute
    {
        public OCApiRoute route;
        public OCRouteSummaryForStop stop;
        public int index;
    }

    public partial class StopRoutes : PhoneApplicationPage
    {
        public ObservableCollection<OCApiRoute> routes = new ObservableCollection<OCApiRoute>();
        private bool addFavePressed;

        public StopRoutes()
        {
            InitializeComponent();
            addFavePressed = false;
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
               
                for(int index = 0; index < routesListObject.Count; index++)
                {
                    OCApiRoute route = routesListObject[index];
                    StopAndRoute stopAndRoute = new StopAndRoute();
                    stopAndRoute.stop = stop;
                    stopAndRoute.route = route;
                    stopAndRoute.index = index;

                    BackgroundWorker bw = new BackgroundWorker();
                    bw.WorkerSupportsCancellation = true;
                    bw.WorkerReportsProgress = false;
                    bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
                    bw.RunWorkerAsync(stopAndRoute);

                    route.nextTimes = "Loading...";
                }
                routes = new ObservableCollection<OCApiRoute>(routesListObject);
                setIsLoading(false);
            }
        }

        private async void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            StopAndRoute stopAndRoute = (StopAndRoute)e.Argument;
            OCApiRoute route = stopAndRoute.route;

            OCApiRoute route1 = await route.fetchTimes(stopAndRoute.stop.StopNumber.ToString());

            Dispatcher.BeginInvoke(() =>
           {
               Console.WriteLine("FUCKING SHIT FUCK: " + route1.nextTimes + " " + route1.fourArrivalTimes);
               routes.RemoveAt(stopAndRoute.index);
               routes.Insert(stopAndRoute.index, route1);
               routesList.ItemsSource = routes;
           });
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                routesList.UpdateLayout();
            });
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

        private async void addFavourite_Click(object sender, RoutedEventArgs e)
        {
            var menItem = (MenuItem)sender;
            OCApiRoute apiRoute = (OCApiRoute)menItem.DataContext;
            OCDirection direction = OCDirection.newOCDirection(apiRoute.RouteNumber, apiRoute.RouteHeading, apiRoute.Direction, "", 0);
            direction.FromStopName = stopName.Text;
            direction.FromStopNumber = int.Parse(stopID.Text);
            direction.DirectionalName = "TO " + apiRoute.RouteHeading.ToUpper();
            int result = await OCTranspoStopsData.addFavouriteStop(direction);
            if (result > 0)
            {
                MessageBox.Show("Your favourite stop was succesfully added.");
            }
            else
            {
                MessageBox.Show("There was an error adding your favourite stop, please try again.");
            }
        }
    }
}