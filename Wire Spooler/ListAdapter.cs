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
using Java.Lang;

namespace Wire_Spooler
{ 
    public class ViewHolder : Java.Lang.Object
    {
        public TextView txtQuantity { get; set; }

        public EditText txtGauge { get; set; }

        public EditText txtLength { get; set; }
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
            return conductors[position].Quantity;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.list_view, parent, false);

            var txtQuantity = view.FindViewById<TextView>(Resource.Id.quantityText);
            var txtGauge = view.FindViewById<EditText>(Resource.Id.gaugeText);
            var txtLength = view.FindViewById<EditText>(Resource.Id.lengthText);

            return view;
        }
    }
}