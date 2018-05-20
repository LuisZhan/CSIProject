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
using CSIMobile.Class.Common;
using CSIMobile.Class.Activities;

namespace CSIMobile.Class.Fragments.Adapter
{
    public class ModuleGridViewerAdapter : CSIBaseGridViewerAdapter
    {

        public ModuleGridViewerAdapter(Android.Support.V4.App.Fragment f, GridView gridView) : base(f, gridView)
        {
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return base.GetItem(position);
        }

        public override long GetItemId(int position)
        {
            return base.GetItemId(position);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            try
            {
                ModuleAction Action = (ModuleAction)ActionItems[position];

                View view = convertView;

                if (view == null) // no view to re-use, create new
                {
                    if (Activity != null)
                    {
                        view = Activity.LayoutInflater.Inflate(Resource.Layout.CSIGridItem, null);
                    }
                    else if (Fragment != null)
                    {
                        view = Fragment.LayoutInflater.Inflate(Resource.Layout.CSIGridItem, null);
                    }else{
                        return null;
                    }
                }

                ImageView imageView = view.FindViewById<ImageView>(Resource.Id.ImageView);
                TextView textView = view.FindViewById<TextView>(Resource.Id.ActionText);

                //imageView.LayoutParameters = new AbsListView.LayoutParams(300, 300);
                imageView.SetScaleType(ImageView.ScaleType.FitCenter);
                imageView.SetPadding(24, 24, 24, 24);
                imageView.SetImageResource(Action.DrawableId);
                textView.Text = Action.ActionName;
                view.Enabled = Action.Enabled;
                view.Visibility = Action.Visible ? ViewStates.Visible : ViewStates.Gone;
                return view;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
    }
}