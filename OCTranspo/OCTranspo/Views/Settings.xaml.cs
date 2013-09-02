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
using OCTranspo.Models;
using System.Threading.Tasks;

namespace OCTranspo.Views
{
    public partial class Settings : PhoneApplicationPage
    {
        int distance;
        bool dirtyPage;

        public Settings()
        {
            InitializeComponent();
            this.dirtyPage = false;
        }

        protected override async void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            OCSettings settings = await OCTranspoStopsData.getSettings();
            nearbyDistanceSlider.Value = settings.nearbyDistance;
        }

        protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            Boolean canLeave = false;
            if (this.dirtyPage)
            {
                MessageBoxResult result = MessageBox.Show("Do you want leave without saving?", "", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    canLeave = true;
                }
                else
                {
                    canLeave = false;
                }
            }
            else
            {
                canLeave = true;
            }

            e.Cancel = canLeave == false;
        }

        private void nearbyDistanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider nearbySlider = (Slider)sender;
            distance = (int)nearbySlider.Value;
            this.dirtyPage = true;
        }

        private void ApplicationBarMenuItem_Click_2(object sender, EventArgs e)
        {
            Navigation.NavigateToInfo();
        }

        private async void ApplicationBarMenuItem_Click_1(object sender, EventArgs e)
        {
           await saveSettings(false);
        }

        private async Task<Boolean> saveSettings(Boolean fromExit)
        {
            int result = await OCTranspoStopsData.updateSettings(OCSettings.newOCSettings(distance));
            if (result > 0)
            {
                MessageBox.Show("Settings were successfully saved.");
                dirtyPage = false;
                if (this.NavigationService.CanGoBack && fromExit == false)
                {
                    this.NavigationService.GoBack();
                }
                return true;
            }
            else
            {
                MessageBox.Show("There was an error saving your settings, please try again.");
                return false;
            }
        }
    }
}