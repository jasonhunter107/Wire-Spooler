using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Wire_Spooler
{
    public sealed class AppState
    {
        public static AppState Instance { get; } = new AppState();

        private AppState()
        {
            SpoolSize = 10;
        }

        public int SpoolSize { get; set; }

       // public List<Conductor> Conductors = new List<Conductor>();

        //Add a conductor class
        //Add a List of conductors here
        //Create Adapter class and inherit from the base ArrayAdapter class (refer to bookmarked page)
    }
}