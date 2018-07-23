using System;
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
            //await AppState.Instance.tabClient.ConnectAsync("10.205.61.70", 10002);
            await AppState.Instance.tabClient.ConnectAsync("10.0.2.2", 8081);

            //Tell PLC that we are in Manual Mode
            writeCancellationTokenSource = new CancellationTokenSource();
            writeTask = AppState.Instance.tabClient.SendCommandAsync(writeCancellationTokenSource.Token, 1);

            // Create your application here
            SetContentView(Resource.Layout.activity_manual);

            /**********************************************************************
            * User enters speed of VFD and sets the value
            *********************************************************************/
            var speedBtn = FindViewById<Button>(Resource.Id.setSpeedBtn);
            var speedTxt = FindViewById<EditText>(Resource.Id.speedText);
            var speedNum = 0;
            speedBtn.Click += (s, e) =>
            {
                writeCancellationTokenSource = new CancellationTokenSource();
                writeTask = AppState.Instance.tabClient.SendRunMotorCommandAsync(writeCancellationTokenSource.Token,
                    ( (float)speedNum ));
            };
            /**********************************************************************
            * Each time speed is changed; change its value
            *********************************************************************/
            speedTxt.TextChanged += (s, e) =>
            {
                speedNum = string.IsNullOrWhiteSpace(speedTxt.Text)
                ? 0
                    : Convert.ToInt32(speedTxt.Text);

            };

            /**********************************************************************
            * User enters actuator speed and sets the value
            *********************************************************************/
            var actSpeedBtn = FindViewById<Button>(Resource.Id.actSpeedBtn);
            var actSpeedTxt = FindViewById<EditText>(Resource.Id.actSpeedText);
            var actSpeedNum = 0;
            actSpeedBtn.Click += (s, e) =>
            {
                writeCancellationTokenSource = new CancellationTokenSource();
                writeTask = AppState.Instance.tabClient.SendActuatorSpeedAsync(writeCancellationTokenSource.Token, 
                     (float)actSpeedNum );
            };

            /**********************************************************************
            * Each time actuator speed is changed; change its value
            *********************************************************************/
            actSpeedTxt.TextChanged += (s, e) =>
            {
                actSpeedNum = string.IsNullOrWhiteSpace(actSpeedTxt.Text)
                ? 0
                    : Convert.ToInt32(actSpeedTxt.Text);

            };

            /**********************************************************************
            * User enters spool size and sets the value
            *********************************************************************/
            var spoolSizeBtn = FindViewById<Button>(Resource.Id.spoolSizeBtn);
            var spoolSizeTxt = FindViewById<EditText>(Resource.Id.spoolSizeNum);
            var spoolSizeNum = 0;
            spoolSizeBtn.Click += (s, e) =>
            {
                writeCancellationTokenSource = new CancellationTokenSource();
                writeTask = AppState.Instance.tabClient.SendSpoolSizeAsync(writeCancellationTokenSource.Token, 
                    (float)spoolSizeNum);
            };
            /**********************************************************************
            * Each time the spool size is changed; change its value
            *********************************************************************/
            spoolSizeTxt.TextChanged += (s, e) =>
            {
                spoolSizeNum = string.IsNullOrWhiteSpace(spoolSizeTxt.Text)
                ? 0
                    : Convert.ToInt32(spoolSizeTxt.Text);

            };

            /**********************************************************************
            * User presses Jog Forward
            *********************************************************************/
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
                writeTask = AppState.Instance.tabClient.SendCommandAsync(writeCancellationTokenSource.Token, 3);
            };

            /**********************************************************************
            * User presses Jog Reverse
            *********************************************************************/
            var jogReverse = FindViewById<Button>(Resource.Id.jogReverse);
            jogReverse.Click += (s, e) =>
            {
                writeCancellationTokenSource = new CancellationTokenSource();
                writeTask = AppState.Instance.tabClient.SendCommandAsync(writeCancellationTokenSource.Token, 4);
            };

            /**********************************************************************
            * User presses Jog Left
            *********************************************************************/
            var jogLeft = FindViewById<Button>(Resource.Id.jogLeft);
            jogLeft.Click += (s, e) =>
            {
                writeCancellationTokenSource = new CancellationTokenSource();
                writeTask = AppState.Instance.tabClient.SendCommandAsync(writeCancellationTokenSource.Token, 5);
            };

            /**********************************************************************
            * User presses Jog Right
            *********************************************************************/
            var jogRight = FindViewById<Button>(Resource.Id.jogRight);
            jogRight.Click += (s, e) =>
            {
                writeCancellationTokenSource = new CancellationTokenSource();
                writeTask = AppState.Instance.tabClient.SendCommandAsync(writeCancellationTokenSource.Token, 6);
            };

            /**********************************************************************
            * User presses the run button
            *********************************************************************/
            var runBtn = FindViewById<Button>(Resource.Id.runBtn);
            runBtn.Click += (s, e) =>
            {
                //Check to see if user entered all fields
                if (speedNum == 0)
                    Toast.MakeText(this, "Set Speed First", ToastLength.Long).Show();
                else if (actSpeedNum == 0)
                    Toast.MakeText(this, "Set Actuator Speed First", ToastLength.Long).Show();
                else if (spoolSizeNum == 0)
                    Toast.MakeText(this, "Set Spool Size First", ToastLength.Long).Show();
                //If user entered all the fields
                else
                {
                    writeCancellationTokenSource = new CancellationTokenSource();
                    writeTask = AppState.Instance.tabClient.SendCommandAsync(writeCancellationTokenSource.Token, 11);
                }
            };

            /**********************************************************************
            * User presses the stop button
            *********************************************************************/
            var stopBtn = FindViewById<Button>(Resource.Id.stopBtn);
            stopBtn.Click += (s, e) =>
            {
                writeCancellationTokenSource = new CancellationTokenSource();
                writeTask = AppState.Instance.tabClient.SendCommandAsync(writeCancellationTokenSource.Token, 10);
            };

        }

        /**********************************************************************
        * When the activity is destroyed, cancel all tokens and close the client
        *********************************************************************/
        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (readCancellationTokenSource != null)
                readCancellationTokenSource.Cancel();

            AppState.Instance.tabClient.Close();

            if (writeCancellationTokenSource != null)
                writeCancellationTokenSource.Cancel();
        }
    }
}