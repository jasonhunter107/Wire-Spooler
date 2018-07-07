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
        private Button gSelectSpool;
        private int gSizeOfSpool;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Client class
            //TabletClient tab = new TabletClient("10.0.2.2", 8081);

            // Create your application here
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_auto);

            gSelectSpool = FindViewById<Button>(Resource.Id.button9);

            //Start dialog when user clicks on select spool size
            gSelectSpool.Click += (object sender, EventArgs args) =>
            {
                //pull up dialog
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                SpoolSizeDialog spoolDialog = new SpoolSizeDialog();
                spoolDialog.Show(transaction, "Dialog Fragment");

                //Button closeBtn = spoolDialog.Fi

                gSizeOfSpool = spoolDialog.SpoolSize;
            };


            var btnGoBack = FindViewById<Button>(Resource.Id.button10);
            btnGoBack.Click += delegate
            {
                this.Finish();
            };


            var spoolSize = FindViewById<EditText>(Resource.Id.editText5); //Spool size edit text
            spoolSize.Text = gSizeOfSpool.ToString();
        }

    }
}