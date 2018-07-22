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
            //10.0.2.2:8081 || 10.205.61.70:10002
            await AppState.Instance.tabClient.ConnectAsync("10.0.2.2", 8081);

            //Creating new Cancellation token for reading data
            readCancellationTokenSource = new CancellationTokenSource();
            readTask = AppState.Instance.tabClient.ReceiveDataAsync(readCancellationTokenSource.Token);

            //Test out concurrency by writing 3 to PLC
            var copyBtn = FindViewById<Button>(Resource.Id.copyBtn);
            copyBtn.Click += (s, e) =>
            {
                writeCancellationTokenSource = new CancellationTokenSource();
                writeTask = AppState.Instance.tabClient.SendCommandAsync(writeCancellationTokenSource.Token, 3);
            };

            //Update the length feed everytime a value is read
            var lengthFeed = FindViewById<TextView>(Resource.Id.lengthFeed);
            AppState.Instance.tabClient.StatusMsgReceived += (s, e) =>
            {
                lengthFeed.Text = string.Format("{0} ft.",e.ToString());
            };

            //Stuff to add:
            // Save/Load setting for auto mode
            //Jog Left/Right buttons on manual mode
            //Live feed of the length spooled in auto screen (just like manual mode)

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
            var lstConductor = AppState.Instance.Conductors;

            createTable.Click += delegate
             {
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

                             lstConductor.Add(conductor);
                         }

                         var adapter = new ListAdapter(this, lstConductor);
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
            runButton.Click += delegate
            {
                if (conductorNum != 0 || !string.IsNullOrWhiteSpace(condText.Text))
                {
                    //If there is only 1 run that needs to be done
                    if (conductorNum == 1)
                    {
                        Toast.MakeText(this, "Your job is running", ToastLength.Long).Show();

                        //Create cancellation token for writing data
                        writeCancellationTokenSource = new CancellationTokenSource();

                        //Send data to the PLC
                        writeTask = AppState.Instance.tabClient.SendSpoolWireCodeAsync(writeCancellationTokenSource.Token,
                            sizeOfSpool, AppState.Instance.Conductors[0].Quantity,
                            AppState.Instance.Conductors[0].Gauge, AppState.Instance.Conductors[0].Length);

                        //Pop up a alert dialog indicating that the job is complete
                        AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
                        alertDialog.SetTitle("Operation Status");
                        alertDialog.SetMessage("Your job has been complete!");
                        alertDialog.SetNeutralButton("OK", delegate
                        {
                            alertDialog.Dispose();
                        });
                        alertDialog.Show();
                    }
                    //If there are multiple runs for the operation
                    else
                    {
                        var totalLength = AppState.Instance.Conductors[0].Length;
                        var listSize = AppState.Instance.Conductors.Count;
                        var tempTotal = totalLength;
                        writeCancellationTokenSource = new CancellationTokenSource();

                        //Iterate through the number of runs
                        for (var i = 0; i < (listSize - 1); i++)
                        {
                            //Calculate new total amount of length that needs to be spooled for each run
                            var temp = tempTotal - AppState.Instance.Conductors[i + 1].Length;

                            //Send each run to the PLC
                            writeTask = AppState.Instance.tabClient.SendSpoolWireCodeAsync(writeCancellationTokenSource.Token,
                                AppState.Instance.SpoolSize, AppState.Instance.Conductors[i].Quantity,
                                AppState.Instance.Conductors[i].Gauge, temp);

                            //Assign new total Length and use that for next run
                            tempTotal = AppState.Instance.Conductors[i + 1].Length;
                        }

                        //Finish Last Run
                        writeTask = AppState.Instance.tabClient.SendSpoolWireCodeAsync(writeCancellationTokenSource.Token,
                            AppState.Instance.SpoolSize, AppState.Instance.Conductors[listSize - 1].Quantity,
                                 AppState.Instance.Conductors[listSize - 1].Gauge, tempTotal);

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
            * Each time the spool size changes, modify its value in the AppState
            *********************************************************************/
            spoolTxt.TextChanged += (s, e) =>
            {
                sizeOfSpool = string.IsNullOrWhiteSpace(spoolTxt.Text)
                ? 0
                    : Convert.ToInt32(spoolTxt.Text);
            };
        }

        /**********************************************************************
        * When the activity gets destroyed, cancel all active tokens 
        *********************************************************************/
        protected override void OnDestroy()
        {
            base.OnDestroy();
            readCancellationTokenSource.Cancel();

            if(writeCancellationTokenSource != null)
            writeCancellationTokenSource.Cancel();
        }
    }
}