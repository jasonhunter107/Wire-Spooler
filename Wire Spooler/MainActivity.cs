using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;

namespace Wire_Spooler
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            //127.0.0.1
            TabletClient tab = new TabletClient("10.0.2.2", 8081);

            var btnAuto = FindViewById<Button>(Resource.Id.button4);
            var btnManual = FindViewById<Button>(Resource.Id.button5);
            var btnAlarm = FindViewById<Button>(Resource.Id.button6);
            var btnControlRst = FindViewById<Button>(Resource.Id.button7);
            var edtSpeed = FindViewById<EditText>(Resource.Id.editText); //Gets string from textbox

            btnAuto.Click += (s, e) =>
            {
                Intent nextActivity = new Intent(this, typeof(AutoActivity));
                StartActivity(nextActivity);
            };

            btnManual.Click += (s, e) => 
            {
                Intent nextActivity = new Intent(this, typeof(ManualActivity));
                StartActivity(nextActivity);
            };

            btnAlarm.Click += (s, e) =>
            {
                Intent nextActivity = new Intent(this, typeof(AlarmActivity));
                StartActivity(nextActivity);
            };

            btnControlRst.Click += (s, e) =>
            {
                int inches;

                if (edtSpeed.Text != null)
                    inches = Int32.Parse(edtSpeed.Text);
                else
                    inches = 10;

                if (inches < 0)
                    tab.CutWire(10);
                else
                    tab.CutWire(inches);
            };
        }
    }
}

