﻿using System;
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

        public float SpoolSize { get; set; }

        public TabletClient TabClient = new TabletClient();

        public List<Conductor> Conductors = new List<Conductor>();
    }
}