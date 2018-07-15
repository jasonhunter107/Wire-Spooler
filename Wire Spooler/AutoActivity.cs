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

                //Button closeBtn = spoolDialog.Fi

                sizeOfSpool = AppState.Instance.SpoolSize;
            };

            /**********************************************************************
            * Generates the table of the conductors
            *********************************************************************/

            var lstData = FindViewById<ListView>(Resource.Id.conductorList);
            var createTable = FindViewById<Button>(Resource.Id.genBtn);
            var condText = FindViewById<EditText>(Resource.Id.ConductorAmtText);
            createTable.Click += delegate
            {
                var conductorNum = Int32.Parse(condText.Text);
                List<Conductor> lstConductor = new List<Conductor>();
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
            * Go back to menu button
            *********************************************************************/
            var btnGoBack = FindViewById<Button>(Resource.Id.menuBtn);
            btnGoBack.Click += delegate
            {
                this.Finish();
            };


            //var spoolSize = FindViewById<EditText>(Resource.Id.editText); //Spool size edit text
            //spoolSize.Text = gSizeOfSpool.ToString();
        }

    }
}