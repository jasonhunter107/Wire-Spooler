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
    [Activity(Label = "AutoActivity")]
    public class AutoActivity : Activity
    {
        //Cancellation tokens for the read and write tasks
        private CancellationTokenSource readCancellationTokenSource;
        private CancellationTokenSource writeCancellationTokenSource;

        //The read and write asynchronously task
        private Task writeTask;
        private Task readTask;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            // Set our view from the "auto" layout resource
            SetContentView(Resource.Layout.activity_auto);

            //Variables that are going to be needed
            var sizeOfSpool = 0;
            var conductorNum = 0;

            //Spool size text
            var spoolTxt = FindViewById<EditText>(Resource.Id.editText3);
            spoolTxt.Text = AppState.Instance.SpoolSize.ToString();


            //Client class
            //TabletClient tab = new TabletClient();
            //10.0.2.2:8081 || 10.205.61.70:10002 || 192.168.1.12:37847
            await AppState.Instance.TabClient.ConnectAsync("10.205.61.70", 10002);
            //await AppState.Instance.TabClient.ConnectAsync("10.0.2.2", 8081);

            //Creating new Cancellation token for reading data
            readCancellationTokenSource = new CancellationTokenSource();
            readTask = AppState.Instance.TabClient.ReceiveDataAsync(readCancellationTokenSource.Token);


            //Tell PLC that we are in Auto Mode
            writeCancellationTokenSource = new CancellationTokenSource();
            writeTask = AppState.Instance.TabClient.SendCommandAsync(writeCancellationTokenSource.Token, 0);

            //Test out concurrency by writing 3 to PLC
            var copyBtn = FindViewById<Button>(Resource.Id.copyBtn);
            copyBtn.Click += (s, e) =>
            {
                writeCancellationTokenSource = new CancellationTokenSource();
                writeTask = AppState.Instance.TabClient.SendCommandAsync(writeCancellationTokenSource.Token, 0);
            };

            //Update the length feed everytime a value is read
            var lengthFeed = FindViewById<TextView>(Resource.Id.lengthFeed);
            AppState.Instance.TabClient.WireLengthReceived += (s, e) =>
            {
                lengthFeed.Text = string.Format("{0} ft.", e.ToString());
            };

            var statusMsg = FindViewById<TextView>(Resource.Id.statusText);
            AppState.Instance.TabClient.StatusMsgReceived += (s, e) =>
            {
                statusMsg.Text = string.Format("{0}", e.ToString());
            };

            var servoPos = FindViewById<TextView>(Resource.Id.servoPosTxt);
            AppState.Instance.TabClient.ServoPosReceived += (s, e) =>
            {
                servoPos.Text = string.Format("{0}", e.ToString());
            };

            var alarmMsg = FindViewById<TextView>(Resource.Id.alarmMsgTxt);
            AppState.Instance.TabClient.AlarmMsgReceived += (s, e) =>
            {
                alarmMsg.Text = string.Format("{0}", e.ToString());
            };

            //Stuff to add:
            // Save/Load setting for auto mode - probably not happening
            //Jog Left/Right buttons on manual mode - DONE
            //Live feed of the length spooled in auto screen (just like manual mode) - DONE

            //Start dialog when user clicks on select spool size
            var selectSpool = FindViewById<Button>(Resource.Id.spoolSizeBtn);
            SpoolSizeDialog spoolDialog = new SpoolSizeDialog();
            selectSpool.Click += (s, e) =>
            {
                //pull up dialog
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                spoolDialog.Show(transaction, "Dialog Fragment");


                //sizeOfSpool = AppState.Instance.SpoolSize;
                //spoolTxt.Text = AppState.Instance.SpoolSize.ToString();
            };

            //Update the spool size after the dialog closes
            spoolDialog.DialogClosed += (s, e) =>
            {
                spoolTxt.Text = AppState.Instance.SpoolSize.ToString();
            };


            /**********************************************************************
            * Generates the table of the conductors
            *********************************************************************/

            var lstData = FindViewById<ListView>(Resource.Id.conductorList);
            var createTable = FindViewById<Button>(Resource.Id.genBtn);
            var condText = FindViewById<EditText>(Resource.Id.ConductorAmtText);
            //var lstConductor = AppState.Instance.Conductors;

            createTable.Click += delegate
             {
                 //Clear out list first
                 AppState.Instance.Conductors.Clear();

                 writeCancellationTokenSource.Cancel();
                 //error check here
                 if (!string.IsNullOrWhiteSpace(condText.Text))
                 {
                     if (Int32.Parse(condText.Text) != 0)
                     {
                         var tempCondNum = Int32.Parse(condText.Text);
                         conductorNum = tempCondNum;
                         for (int i = 0; i < tempCondNum; i++)
                         {
                             Conductor conductor = new Conductor()
                             {
                                 Quantity = 0,
                                 Gauge = 0,
                                 Length = 0
                             };

                             AppState.Instance.Conductors.Add(conductor);
                         }

                         var adapter = new ListAdapter(this, AppState.Instance.Conductors);
                         lstData.Adapter = adapter;
                     }
                 }
                 //If user did not enter a number of runs
                 else
                 {
                     Toast.MakeText(this, "Insert the amount of runs first", ToastLength.Long).Show();
                 }
             };

            /**********************************************************************
            * Run button
            *********************************************************************/
            var runButton = FindViewById<Button>(Resource.Id.runBtn);
            var tempCondNum2 = conductorNum;
            runButton.Click += async delegate //Inserted async here (may or may not want to remove)
            {
                writeCancellationTokenSource.Cancel();

                if (conductorNum != 0 || !string.IsNullOrWhiteSpace(condText.Text))
                {
                    //Check to see if all table cells are filled
                    if (!IsEmptyCells())
                    {
                        //If there is only 1 run that needs to be done
                        if (tempCondNum2 == 1)
                        {
                            Toast.MakeText(this, "Your job is running", ToastLength.Long).Show();

                            //Create cancellation token for writing data
                            writeCancellationTokenSource = new CancellationTokenSource();

                            //Send data to the PLC
                            //writeTask = AppState.Instance.tabClient.SendSpoolWireCodeAsync(writeCancellationTokenSource.Token,
                            //     sizeOfSpool, AppState.Instance.Conductors[0].Quantity,
                            //    AppState.Instance.Conductors[0].Gauge, AppState.Instance.Conductors[0].Length);

                            //Send data to the PLC
                            await SendSingleRunAsync(0,sizeOfSpool);

                            //Wait for response
                            readTask = AppState.Instance.TabClient.ReceiveDataAsync(readCancellationTokenSource.Token);
                            Toast.MakeText(this, "Your Run is Complete", ToastLength.Long).Show();

                        }
                        //If there are multiple runs for the operation
                        else
                        {
                            var tempListSize = AppState.Instance.Conductors.Count;
                            var totalLength = AppState.Instance.Conductors[0].Length;
                            var tempTwo = AppState.Instance.Conductors[tempListSize + 1].Length;
                            var tempLength = totalLength - tempTwo;

                            await SendMultipleRunAsync(tempLength, sizeOfSpool);

                            //Wait for PLC response
                            readTask = AppState.Instance.TabClient.ReceiveDataAsync(readCancellationTokenSource.Token);
                            Toast.MakeText(this, "Your Run is Complete", ToastLength.Long).Show();

                            tempCondNum2--;

                            AppState.Instance.Conductors.RemoveAt(0);

                            //update list adapater here?

                        }
                    }
                    //If there is an empty cell
                    else
                    {
                        Toast.MakeText(this, "Fill in table first", ToastLength.Long).Show();
                    }
                }
                //If user did not enter the number of runs 
                else
                {
                    Toast.MakeText(this, "Insert the amount of runs first", ToastLength.Long).Show();
                }
            };

            /**********************************************************************
            * Go back to menu button
            *********************************************************************/
            var btnGoBack = FindViewById<Button>(Resource.Id.menuBtn);
            btnGoBack.Click += delegate
            {
                this.Finish();
            };

            /**********************************************************************
            * Stop operation button
            *********************************************************************/
            var stopBtn = FindViewById<Button>(Resource.Id.stopBtn);
            stopBtn.Click += delegate
            {
                writeCancellationTokenSource = new CancellationTokenSource();
                writeTask = AppState.Instance.TabClient.SendCommandAsync(writeCancellationTokenSource.Token, 10);
            };
            /**********************************************************************
            * Each time the spool size changes, modify its value in the AppState
            *********************************************************************/
            spoolTxt.TextChanged += (s, e) =>
            {
                sizeOfSpool = string.IsNullOrWhiteSpace(spoolTxt.Text)
                ? 0
                    : Convert.ToInt32(spoolTxt.Text);
                AppState.Instance.SpoolSize = sizeOfSpool;

            };
        }

        /**********************************************************************
        * This method checks to see if any cell in the table is empty
        *********************************************************************/
        private bool IsEmptyCells()
        {
            var listsize = AppState.Instance.Conductors.Count;

            //iterate through each cell in the table to 
            // check if there is an empty cell
            for (var i = 0; i < listsize; i++)
            {
                if (AppState.Instance.Conductors[i].Gauge == 0 ||
                    AppState.Instance.Conductors[i].Quantity == 0 ||
                    AppState.Instance.Conductors[i].Length == 0 ||
                    AppState.Instance.SpoolSize == 0
                    )
                    return true;
            }

            return false;
        }
        //Only when sending one run or the last run
        private async Task SendSingleRunAsync(int i, int sizeOfSpool)
        {
            writeTask = AppState.Instance.TabClient.SendLengthAsync(writeCancellationTokenSource.Token,
            AppState.Instance.Conductors[i].Length);

            await Task.Delay(200);

            writeTask = AppState.Instance.TabClient.SendSpoolSizeAsync(writeCancellationTokenSource.Token,
                 (float)sizeOfSpool);

            await Task.Delay(200);

            writeTask = AppState.Instance.TabClient.SendQuantityAsync(writeCancellationTokenSource.Token,
                 AppState.Instance.Conductors[i].Quantity);

            await Task.Delay(200);

            writeTask = AppState.Instance.TabClient.SendGaugeCommandAsync(writeCancellationTokenSource.Token,
                  AppState.Instance.Conductors[i].Gauge);

            await Task.Delay(200);


            writeTask = AppState.Instance.TabClient.SendCommandAsync(writeCancellationTokenSource.Token, 11);
        }

        //When sending multiple runs, need to send a temporary length to PLC
        private async Task SendMultipleRunAsync(float length, int sizeOfSpool)
        {
            writeTask = AppState.Instance.TabClient.SendLengthAsync(writeCancellationTokenSource.Token,
                length);

            await Task.Delay(200);

            writeTask = AppState.Instance.TabClient.SendSpoolSizeAsync(writeCancellationTokenSource.Token,
                 (float)sizeOfSpool);

            await Task.Delay(200);

            writeTask = AppState.Instance.TabClient.SendQuantityAsync(writeCancellationTokenSource.Token,
                 AppState.Instance.Conductors[0].Quantity);

            await Task.Delay(200);

            writeTask = AppState.Instance.TabClient.SendGaugeCommandAsync(writeCancellationTokenSource.Token,
                  AppState.Instance.Conductors[0].Gauge);

            await Task.Delay(200);


            writeTask = AppState.Instance.TabClient.SendCommandAsync(writeCancellationTokenSource.Token, 11);
        }

        /**********************************************************************
        * When the activity gets destroyed, cancel all active tokens 
        *********************************************************************/
        protected override void OnDestroy()
        {
            base.OnDestroy();
            readCancellationTokenSource.Cancel();

            AppState.Instance.TabClient.Close();

            if (writeCancellationTokenSource != null)
                writeCancellationTokenSource.Cancel();
        }
    }
}