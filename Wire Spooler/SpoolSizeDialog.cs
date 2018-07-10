﻿using System;
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
   class SpoolSizeDialog : DialogFragment 
    {
        /* Declaring the size of the frame */
        private int spoolSize = 0;


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.dialog_select, container, false);

            var submitBtn = view.FindViewById<Button>(Resource.Id.button13);


            var spoolSizeBox = view.FindViewById<EditText>(Resource.Id.editText2); //Gets string from textbox

            spoolSizeBox.Text = AppState.Instance.SpoolSize.ToString();

           // spoolSize = Int32.Parse(spoolSizeBox.Text);

            // Set up a handler to dismiss this DialogFragment when this button is clicked.
            var closeBtn = view.FindViewById<Button>(Resource.Id.button13); //.Click += (sender, args) => Dismiss();
            closeBtn.Click += delegate
            {
                this.Dismiss();
            };

            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            var submitBtn = View.FindViewById<Button>(Resource.Id.button13);

            //Different types of spools
            List<string> list = new List<string>();
            list.Add("1");
            list.Add("2");
            list.Add("3");
            list.Add("4");
            list.Add("5");

            var adapter = new ArrayAdapter<string>(this.Activity, Android.Resource.Layout.SimpleSpinnerItem, list.ToArray());

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);



        }

        //getter and setter for spool size
        public int SpoolSize
        {
            get
            {
                return spoolSize;
            }
            set
            {
                spoolSize = value;
            }
        }

    }
}