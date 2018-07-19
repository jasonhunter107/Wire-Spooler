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
        private CancellationTokenSource readCancellationTokenSource;
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
            TabletClient tab = new TabletClient();
            await tab.ConnectAsync("10.0.2.2", 8081);

            readCancellationTokenSource = new CancellationTokenSource();
            readTask = tab.ReceiveDataAsync(readCancellationTokenSource.Token);

            //Test out concurrency
            var copyBtn = FindViewById<Button>(Resource.Id.copyBtn);
            copyBtn.Click += async (s, e) =>
            {
                using (var cts = new CancellationTokenSource())
                    await tab.ReceiveDataAsync(cts.Token);
            };

            //Update the length feed everytime a value is read
            var lengthFeed = FindViewById<TextView>(Resource.Id.lengthFeed);
            tab.StatusMsgReceived += (s, e) =>
            {
                lengthFeed.Text = e.ToString();
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
                //SpoolSizeDialog spoolDialog = new SpoolSizeDialog();
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
             };

            /**********************************************************************
            * Run button
            *********************************************************************/
            var runButton = FindViewById<Button>(Resource.Id.runBtn);
            runButton.Click += delegate
            {
                if (conductorNum != 0)
                {
                    //If there is only 1 run that needs to be done
                    if (conductorNum == 1)
                    {
                        Toast.MakeText(this, "Your job is about to start", ToastLength.Long).Show();

                        if (tab.SpoolWire(sizeOfSpool, AppState.Instance.Conductors[0].Quantity,
                            AppState.Instance.Conductors[0].Gauge, AppState.Instance.Conductors[0].Length))
                        {
                            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
                            alertDialog.SetTitle("Operation Status");
                            alertDialog.SetMessage("Your job has been complete!");
                            alertDialog.SetNeutralButton("OK", delegate
                            {
                                alertDialog.Dispose();
                            });
                            alertDialog.Show();
                        }
                    }
                    //
                    else
                    {
                        var totalLength = AppState.Instance.Conductors[0].Length;
                        var listSize = AppState.Instance.Conductors.Count;
                        var tempTotal = totalLength;

                        //Iterate through the number of runs
                        for (var i = 0; i < (listSize - 1); i++)
                        {
                            //Calculate new total amount of length that needs to be spooled for each run
                            var temp = tempTotal - AppState.Instance.Conductors[i + 1].Length;

                            //Send each run to the PLC
                            tab.SpoolWire(AppState.Instance.SpoolSize, AppState.Instance.Conductors[i].Quantity,
                                AppState.Instance.Conductors[i].Gauge, temp);

                            //Assign new total Length and use that for next run
                            tempTotal = AppState.Instance.Conductors[i + 1].Length;
                        }

                        //Finish Last Run
                        tab.SpoolWire(AppState.Instance.SpoolSize, AppState.Instance.Conductors[listSize - 1].Quantity,
                                AppState.Instance.Conductors[listSize - 1].Gauge, tempTotal);

                    }
                }

                //spoolTxt.Text = String.Format(" {0} {1} {2} {3} {4} {5}",
                //AppState.Instance.Conductors[0].Quantity.ToString(),
                //AppState.Instance.Conductors[0].Gauge.ToString(),
                //AppState.Instance.Conductors[0].Length.ToString(),
                //AppState.Instance.Conductors[1].Quantity.ToString(),
                //AppState.Instance.Conductors[1].Gauge.ToString(),
                //AppState.Instance.Conductors[1].Length.ToString()
                //);
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

    }
}