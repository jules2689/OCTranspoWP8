using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using OCTranspo;

namespace OCTranspo.Views
{
    public partial class Settings : PhoneApplicationPage
    {
        string distanceString;
        int distance;

        public Settings()
        {
            InitializeComponent();
        }

        private void nearbyDistanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider nearbySlider = (Slider)sender;
            distance = (int)nearbySlider.Value;
            distanceString = distance.ToString() + "m";
        }

        private void ApplicationBarMenuItem_Click_2(object sender, EventArgs e)
        {
            Navigation.NavigateToInfo();
        }

        private void ApplicationBarMenuItem_Click_1(object sender, EventArgs e)
        {
            
        }
    }
}