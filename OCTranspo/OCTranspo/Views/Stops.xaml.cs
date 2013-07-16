using System;
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
    public partial class Stops : PhoneApplicationPage
    {
        public Stops()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string msg = "";

            if (NavigationContext.QueryString.TryGetValue("stopID", out msg))
                stopID.Text = msg;

            if (NavigationContext.QueryString.TryGetValue("stopName", out msg))
                stopName.Text = msg;

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

    }
}