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

        public static void NavigateToStop(String ID, String name)
        {
            String URI = "/Views/stops.xaml?stopID=" + ID.Trim() + "&stopName=" + name.Trim();
            Uri pathURI = new Uri(URI, UriKind.Relative);
            PhoneApplicationFrame ns = (Application.Current.RootVisual as PhoneApplicationFrame);
            ns.Navigate(pathURI);
        }
    }
}
