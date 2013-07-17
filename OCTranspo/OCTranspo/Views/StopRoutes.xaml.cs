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
                stopID.Text = msg;
                OCSupport.getRouteSummaryForStop(int.Parse(msg),new UploadStringCompletedEventHandler(processRouteSummaryForStop));
            }

            if (NavigationContext.QueryString.TryGetValue("stopName", out msg))
                stopName.Text = msg;

            routesListInit();
        }

        public void processRouteSummaryForStop(Object sender, UploadStringCompletedEventArgs e)
        {
            string reply = (string)e.Result;
            OCRouteSummaryForStop stop = OCSupport.makeRouteSummary(reply);
            if (stop != null)
            {
                List<OCRoute> routesListObject = stop.Routes;
                routes = new ObservableCollection<OCRoute>(routesListObject);
                routesList.ItemsSource = routes;
            }
        }

        private void routesListInit()
        {
            routesList.ItemsSource = routes;
        }
    }
}