﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        private CancellationTokenSource readCancellationTokenSource;
        private CancellationTokenSource writeCancellationTokenSource;
        private Task writeTask;
        private Task readTask;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Client class
            //TabletClient tab = new TabletClient();
            //10.0.2.2:8081 || 10.205.61.70:10002
            await AppState.Instance.tabClient.ConnectAsync("10.205.61.70", 10002);

            //Tell PLC that we are in Manual Mode
            writeCancellationTokenSource = new CancellationTokenSource();
            writeTask = AppState.Instance.tabClient.SendCommandAsync(writeCancellationTokenSource.Token, 1);

            // Create your application here
            SetContentView(Resource.Layout.activity_manual);

            var runBtn = FindViewById<Button>(Resource.Id.runBtn);
            var speed = FindViewById<EditText>(Resource.Id.speedText);
            runBtn.Click += (s, e) =>
            {
                writeCancellationTokenSource = new CancellationTokenSource();
                writeTask = AppState.Instance.tabClient.SendRunMotorCommandAsync(writeCancellationTokenSource.Token,
                    ( (float)Int32.Parse(speed.Text) ));
            };

            var actSpeedBtn = FindViewById<Button>(Resource.Id.actSpeedBtn);
            var actSpeed = FindViewById<EditText>(Resource.Id.actSpeedText);
            actSpeedBtn.Click += (s, e) =>
            {
                writeCancellationTokenSource = new CancellationTokenSource();
                writeTask = AppState.Instance.tabClient.SendActuatorSpeedAsync(writeCancellationTokenSource.Token, 
                    ((float)Int32.Parse(actSpeed.Text)));
            };

            var spoolSizeBtn = FindViewById<Button>(Resource.Id.spoolSizeBtn);
            var spoolSize = FindViewById<EditText>(Resource.Id.spoolSizeNum);
            spoolSizeBtn.Click += (s, e) =>
            {
                writeCancellationTokenSource = new CancellationTokenSource();
                writeTask = AppState.Instance.tabClient.SendSpoolSizeAsync(writeCancellationTokenSource.Token, 
                    ((float)Int32.Parse(spoolSize.Text)));
            };

            var jogForward = FindViewById<Button>(Resource.Id.jogForward);
            jogForward.Click += (s, e) =>
            {
                //if (e.Event.Action == MotionEventActions.ButtonRelease)
                //{
                //    Toast.MakeText(this, "Button Released", ToastLength.Short).Show();
                //}
                //else if(e.Event.Action == MotionEventActions.ButtonPress)
                //{
                //    Toast.MakeText(this, "Button Held", ToastLength.Short).Show();
                //}
                //else
                //{
                //    //Do nothing
                //}
                writeCancellationTokenSource = new CancellationTokenSource();
                writeTask = AppState.Instance.tabClient.SendCommandAsync(writeCancellationTokenSource.Token, 9);
            };

            var jogReverse = FindViewById<Button>(Resource.Id.jogReverse);
            jogForward.Click += (s, e) =>
            {
                writeCancellationTokenSource = new CancellationTokenSource();
                writeTask = AppState.Instance.tabClient.SendCommandAsync(writeCancellationTokenSource.Token, 10);
            };

            var jogLeft = FindViewById<Button>(Resource.Id.jogLeft);
            jogForward.Click += (s, e) =>
            {
                writeCancellationTokenSource = new CancellationTokenSource();
                writeTask = AppState.Instance.tabClient.SendCommandAsync(writeCancellationTokenSource.Token, 11);
            };

            var jogRight = FindViewById<Button>(Resource.Id.jogRight);
            jogForward.Click += (s, e) =>
            {
                writeCancellationTokenSource = new CancellationTokenSource();
                writeTask = AppState.Instance.tabClient.SendCommandAsync(writeCancellationTokenSource.Token, 12);
            };

        }
    }
}