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

            //127.0.0.1 (comp); 10.0.2.2:8081 (Tablet Emu); 172.18.15.250:10002 (PLC); "10.205.60.169"
            TabletClient tab = new TabletClient("10.0.2.2", 8081);

            //Button variables
            var btnAuto = FindViewById<Button>(Resource.Id.autoMode);
            var btnManual = FindViewById<Button>(Resource.Id.manualMode);
            var btnAlarm = FindViewById<Button>(Resource.Id.alarmScreen);
            var btnControlRst = FindViewById<Button>(Resource.Id.ctrlReset);
            var btnMasterFault = FindViewById<Button>(Resource.Id.mstrFaultReset);

            AppState.Instance.SpoolSize = 0;

            //Start auto mode actitivty when user clicks auto mode
            btnAuto.Click += (s, e) =>
            {
                Intent nextActivity = new Intent(this, typeof(AutoActivity));
                StartActivity(nextActivity);
            };

            //Start manual mode actitivty when user clicks manual mode
            btnManual.Click += (s, e) => 
            {
                Intent nextActivity = new Intent(this, typeof(ManualActivity));
                StartActivity(nextActivity);
            };

            //Start Alarm activity when user clicks on alarm history
            btnAlarm.Click += (s, e) =>
            {
                Intent nextActivity = new Intent(this, typeof(AlarmActivity));
                StartActivity(nextActivity);
            };

            //Activate control Reset
            btnControlRst.Click += (s, e) =>
            {
               // tab.SendCommand(7);
            };

            //Activate Fault Reset
            btnMasterFault.Click += (s, e) =>
            {
               // tab.SendCommand(6);
            };

        }
    }
}

