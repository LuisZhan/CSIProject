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

namespace CSIMobile.Class.Common
{
    public class CSIBaseGridViewerAdapter : CSIBaseAdapter
    {
        protected Activity Activity;
        protected Android.Support.V4.App.Fragment Fragment;
        protected GridView GridView;

        public List<Object> ActionItems = new List<Object>();

        public CSIBaseGridViewerAdapter(CSIBaseActivity activity, GridView gridView) : base(activity?.GetCSISystemContext())
        {
            GridView = gridView;
            Activity = activity;
        }

        public CSIBaseGridViewerAdapter(Android.Support.V4.App.Fragment fragment, GridView gridView, CSIContext SrcContext = null) : base(SrcContext)
        {
            GridView = gridView;
            Fragment = fragment;
        }

        public override int Count
        {
            get { return ActionItems.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return (Java.Lang.Object)ActionItems[position];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            throw new NotImplementedException();
        }
    }
}