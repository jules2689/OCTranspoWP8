﻿#pragma checksum "C:\Users\Julian\Documents\GitHub\OCTranspoWP8\OCTranspo\OCTranspo\Views\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7E4322063995915E8C9FA4FDB7AA6D9C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace OCTranspo {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.LongListSelector favouritesList;
        
        internal Microsoft.Phone.Controls.LongListSelector nearbyList;
        
        internal Microsoft.Phone.Maps.Controls.Map currentLocation;
        
        internal Microsoft.Phone.Controls.LongListSelector routesList;
        
        internal System.Windows.Controls.TextBox routesSearch;
        
        internal System.Windows.Controls.ProgressBar searchProgressBar;
        
        internal System.Windows.Controls.TextBlock searchText;
        
        internal System.Windows.Controls.Image searchSmile;
        
        internal System.Windows.Controls.Image searchFrowny;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/OCTranspo;component/Views/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.favouritesList = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("favouritesList")));
            this.nearbyList = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("nearbyList")));
            this.currentLocation = ((Microsoft.Phone.Maps.Controls.Map)(this.FindName("currentLocation")));
            this.routesList = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("routesList")));
            this.routesSearch = ((System.Windows.Controls.TextBox)(this.FindName("routesSearch")));
            this.searchProgressBar = ((System.Windows.Controls.ProgressBar)(this.FindName("searchProgressBar")));
            this.searchText = ((System.Windows.Controls.TextBlock)(this.FindName("searchText")));
            this.searchSmile = ((System.Windows.Controls.Image)(this.FindName("searchSmile")));
            this.searchFrowny = ((System.Windows.Controls.Image)(this.FindName("searchFrowny")));
        }
    }
}

