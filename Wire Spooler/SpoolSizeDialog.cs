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
   class SpoolSizeDialog : DialogFragment 
    {
        public event EventHandler DialogClosed;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.dialog_select, container, false);

            var submitBtn = view.FindViewById<Button>(Resource.Id.button13);

            var spoolSizeBox = view.FindViewById<EditText>(Resource.Id.editText2); //Gets string from textbox


            spoolSizeBox.Text = AppState.Instance.SpoolSize.ToString();

            var firstOption = view.FindViewById<Button>(Resource.Id.firstSize);
            firstOption.Click += delegate
            {
                AppState.Instance.SpoolSize = 5;
                this.Dismiss();
            };

            var secondOption = view.FindViewById<Button>(Resource.Id.secondSize);
            secondOption.Click += delegate
            {
                AppState.Instance.SpoolSize = 10;
                this.Dismiss();
            };


            var thirdOption = view.FindViewById<Button>(Resource.Id.thirdSize);
            thirdOption.Click += delegate
            {
                AppState.Instance.SpoolSize = 15;
                this.Dismiss();
            };


            var fourthOption = view.FindViewById<Button>(Resource.Id.fourthSize);
            fourthOption.Click += delegate
            {
                AppState.Instance.SpoolSize = 20;
                this.Dismiss();
            };

            // Set up a handler to dismiss this DialogFragment when this button is clicked.
            var closeBtn = view.FindViewById<Button>(Resource.Id.button13); //.Click += (sender, args) => Dismiss();
            var customSize = view.FindViewById<EditText>(Resource.Id.editText2);
            closeBtn.Click += delegate
            {
                AppState.Instance.SpoolSize = string.IsNullOrWhiteSpace(customSize.Text)
                 ? 30
                : Convert.ToInt32(customSize.Text);

                this.Dismiss();
            };

            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            var submitBtn = View.FindViewById<Button>(Resource.Id.button13);

            //var firstOption = View.FindViewById<Button>(Resource.Id.firstSize);
            //var secondOption = View.FindViewById<Button>(Resource.Id.secondSize);
            //var thirdOption = View.FindViewById<Button>(Resource.Id.thirdSize);
            //var fourthOption = View.FindViewById<Button>(Resource.Id.fourthSize);
        }

        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);

            if(DialogClosed != null)
            {
                DialogClosed(this, null);
            }
        }

    }
}