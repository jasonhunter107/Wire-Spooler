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
    [Activity(Label = "ManualActivity")]
    public class ManualActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Client class
           // TabletClient tab = new TabletClient("10.0.2.2", 8081);

            // Create your application here
            SetContentView(Resource.Layout.activity_manual);

            var runBtn = FindViewById<Button>(Resource.Id.runBtn);
            var speed = FindViewById<EditText>(Resource.Id.speedText);
            runBtn.Click += (s, e) =>
            {
                //Do something
                //if( tab.RunMotor( Int32.Parse(speed.Text)) )
                //{

                //}
            };

            var jogForward = FindViewById<Button>(Resource.Id.jogForward);
            jogForward.Click += (s, e) =>
            {
                //Do something
                //tab.SendCommand(9);
            };

            var jogReverse = FindViewById<Button>(Resource.Id.jogReverse);
            jogForward.Click += (s, e) =>
            {
                //Do something
                //tab.SendCommand(10);
            };

            var jogLeft = FindViewById<Button>(Resource.Id.jogLeft);
            jogForward.Click += (s, e) =>
            {
                //Do something
                //tab.SendCommand(11);
            };

            var jogRight = FindViewById<Button>(Resource.Id.jogRight);
            jogForward.Click += (s, e) =>
            {
                //Do something
                //tab.SendCommand(12);
            };

        }
    }
}