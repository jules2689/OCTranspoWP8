using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.Windows;

namespace OCTranspo
{
    class Navigation
    {

        public static void NavigateToStopRoute(String routeNumber, String name)
        {
            String URI = "/Views/StopRoutes.xaml?stopRoute=" + routeNumber.Trim() + "&stopName=" + name.Trim();
            NavigateTo(URI);
        }

        public static void NavigateToStop(String ID, String name)
        {
            String URI = "/Views/stops.xaml?stopID=" + ID.Trim() + "&stopName=" + name.Trim();
            NavigateTo(URI);
        }

        public static void NavigateToInfo()
        {
            String URI = "/Views/info.xaml";
            NavigateTo(URI);
        }

        private static void NavigateTo(String URI)
        {
            Uri pathURI = new Uri(URI, UriKind.Relative);
            PhoneApplicationFrame ns = (Application.Current.RootVisual as PhoneApplicationFrame);
            ns.Navigate(pathURI);
        }
    }
}
