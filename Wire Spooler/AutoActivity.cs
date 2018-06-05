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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_auto);

            gSelectSpool = FindViewById<Button>(Resource.Id.button9);

            gSelectSpool.Click += (object sender, EventArgs args) =>
            {
                //pull up dialog
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                SpoolSizeDialog spoolDialog = new SpoolSizeDialog();
                spoolDialog.Show(transaction, "Dialog Fragment");
            };


            var btnGoBack = FindViewById<Button>(Resource.Id.button10);
            btnGoBack.Click += delegate
            {
                this.Finish();
            };
        }

    }
}