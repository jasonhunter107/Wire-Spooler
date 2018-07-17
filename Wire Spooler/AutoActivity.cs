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
    [Activity(Label = "AutoActivity")]
    public class AutoActivity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            // Set our view from the "auto" layout resource
            SetContentView(Resource.Layout.activity_auto);

            var sizeOfSpool = 0;
               
            //Spool size text
            var spoolTxt = FindViewById<EditText>(Resource.Id.editText3);
            spoolTxt.Text = AppState.Instance.SpoolSize.ToString();


            //Client class
            //TabletClient tab = new TabletClient("10.0.2.2", 8081);

            //Stuff to add:
            // Save/Load setting for auto mode
            //Jog Left/Right buttons on manual mode
            //Live feed of the length spooled in auto screen (just like manual mode)

            //Start dialog when user clicks on select spool size
            var selectSpool = FindViewById<Button>(Resource.Id.spoolSizeBtn);
            selectSpool.Click += (s, e) =>
            {
                //pull up dialog
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                SpoolSizeDialog spoolDialog = new SpoolSizeDialog();
                spoolDialog.Show(transaction, "Dialog Fragment");


                sizeOfSpool = AppState.Instance.SpoolSize;
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
                var conductorNum = Int32.Parse(condText.Text);
                for (int i = 0; i < conductorNum; i++)
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
            };

            /**********************************************************************
            * Run button
            *********************************************************************/
            var runButton = FindViewById<Button>(Resource.Id.runBtn);
            runButton.Click += delegate
            {
                //var txtQuantity = lstData.FindViewById<EditText>(Resource.Id.quantityText);
                //var txtGauge = lstData.FindViewById<EditText>(Resource.Id.gaugeText);
                //var txtLength = lstData.FindViewById<EditText>(Resource.Id.lengthText);

               // var t = s.GetView(1, null, null);
               // var a = t.FindViewById<EditText>(Resource.Id.quantityText);

                //foreach (var i in lstConductor)
                //{
                //    i.Quantity = Int32.Parse(txtQuantity.Text);
                //    i.Gauge = Int32.Parse(txtGauge.Text);
                //    i.Length = Int32.Parse(txtLength.Text);

                //    AppState.Instance.conductors.Add(i);
                //}

                spoolTxt.Text = String.Format(" {0} {1} {2} {3} {4} {5}",
                    AppState.Instance.Conductors[0].Quantity.ToString(),
                    AppState.Instance.Conductors[0].Gauge.ToString(),
                    AppState.Instance.Conductors[0].Length.ToString(),
                    AppState.Instance.Conductors[1].Quantity.ToString(),
                    AppState.Instance.Conductors[1].Gauge.ToString(),
                    AppState.Instance.Conductors[1].Length.ToString()
                    );
            };

            /**********************************************************************
            * Go back to menu button
            *********************************************************************/
            var btnGoBack = FindViewById<Button>(Resource.Id.menuBtn);
            btnGoBack.Click += delegate
            {
                this.Finish();
            };
        }

    }
}