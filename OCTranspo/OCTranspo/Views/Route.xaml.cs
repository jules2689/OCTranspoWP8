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
        private bool favourite;
        private bool addFavePressed;

        public Stops()
        {
            InitializeComponent();
            addFavePressed = false;
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

            if (NavigationContext.QueryString.TryGetValue("favourite", out msg))
                favourite = msg.Equals("true") ? true : false;


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

        private async void ApplicationBarMenuItem_Click_1(object sender, EventArgs e)
        {
            if (addFavePressed == false)
            {
                addFavePressed = true;
                OCDirection direction = OCDirection.newOCDirection(int.Parse(routeNumber.Text), routeName.Text, "", "", 0);

                direction.FromStopNumber = stopID;
                direction.FromStopName = fromStopName;
                direction.DirectionalName = "TO " + this.direction.ToUpper();
                int result = await OCTranspoStopsData.addFavouriteStop(direction);
                if (result > 0)
                {
                    MessageBox.Show("Your favourite stop was succesfully added.");
                    ApplicationBarIconButton button = (ApplicationBarIconButton)sender;
                    button.IsEnabled = false;
                    favourite = true;
                }
                else
                {
                    MessageBox.Show("There was an error adding your favourite stop, please try again.");
                    addFavePressed = favourite = false;
                }
            }
        }

        private void ApplicationBarMenuItem_Click_2(object sender, EventArgs e)
        {
            Navigation.NavigateToInfo();
        }

        private void ApplicationBarMenuItem_Click_3(object sender, EventArgs e)
        {
            Navigation.NavigateToSettings();
        }
    }
}