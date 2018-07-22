using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace Wire_Spooler
{ 
    public class ViewHolder : Java.Lang.Object
    {
        public int Quantity { get; set; }

        public int Gauge { get; set; }

        public int Length { get; set; }
    }


    public class ListAdapter : BaseAdapter
    {
        private Activity activity;
        private  List<Conductor> conductors;

        public ListAdapter(Activity activity, List<Conductor> conductors)
        {
            this.activity = activity;
            this.conductors = conductors;
        }


        public override int Count
        {
            get
            {
                return conductors.Count;
            }
        }


        public override Java.Lang.Object GetItem(int position)
        {
            throw new NotImplementedException();
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public float GetGauge(int position)
        {
            return conductors[position].Gauge;
        }

        public float GetLength(int position)
        {
            return conductors[position].Length;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.list_view, parent, false);

            var txtQuantity = view.FindViewById<EditText>(Resource.Id.quantityText);

            txtQuantity.TextChanged += (s, e) =>
            {
                conductors[position].Quantity = string.IsNullOrWhiteSpace(txtQuantity.Text) 
                    ? 0 
                    : Convert.ToInt32(txtQuantity.Text);
            };

            var txtGauge = view.FindViewById<EditText>(Resource.Id.gaugeText);

            txtGauge.TextChanged += (s, e) =>
            {
                conductors[position].Gauge = string.IsNullOrWhiteSpace(txtGauge.Text)
                ? 0
                    : Convert.ToInt32(txtGauge.Text);
            };

            var txtLength = view.FindViewById<EditText>(Resource.Id.lengthText);

            txtLength.TextChanged += (s, e) =>
            {
                conductors[position].Length = string.IsNullOrWhiteSpace(txtLength.Text)
                    ? 0
                    : Convert.ToInt32(txtLength.Text);
            };

            return view;
        }
    }
}