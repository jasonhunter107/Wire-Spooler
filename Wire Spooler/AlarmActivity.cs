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
    [Activity(Label = "AlarmActivity")]
    public class AlarmActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Client class
            //TabletClient tab = new TabletClient("10.0.2.2", 8081);

            // Create your application here
            SetContentView(Resource.Layout.activity_alarm);
        }
    }
}