using System;
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
    public partial class Stops : PhoneApplicationPage
    {
        private int stopID;
        private String fromStopName;
        private String direction;

        public Stops()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string msg = "";

            if (NavigationContext.QueryString.TryGetValue("routeNumber", out msg))
                routeNumber.Text = msg;    

            if (NavigationContext.QueryString.TryGetValue("routeName", out msg))
                 routeName.Text = msg;

            if (NavigationContext.QueryString.TryGetValue("stopID", out msg))
                stopID = int.Parse(msg);

            if (NavigationContext.QueryString.TryGetValue("stopName", out msg))
                fromStopName = msg;

            if (NavigationContext.QueryString.TryGetValue("direction", out msg))
                direction = msg;


            stopsListInit();
        }

        private void stopsListInit()
        {
            List<String> s = new List<string>();
            s.Add(".");
            s.Add(".");
            s.Add(".");
            StopsList.ItemsSource = s;
        }

        private void ApplicationBarMenuItem_Click_1(object sender, EventArgs e)
        {
           OCDirection direction = OCDirection.newOCDirection(int.Parse(routeNumber.Text), routeName.Text, "", "");
           direction.FromStopNumber = stopID;
           direction.FromStopName = fromStopName;
           direction.DirectionalName = "TO " + this.direction.ToUpper();
           OCTranspoStopsData.addFavouriteStop(direction);
        }

        private void ApplicationBarMenuItem_Click_2(object sender, EventArgs e)
        {
            Navigation.NavigateToInfo();
        }
    }
}