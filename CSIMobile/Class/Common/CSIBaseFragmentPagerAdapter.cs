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
using Android.Support.V4.App;

namespace CSIMobile.Class.Common
{
    class CSIBaseFragmentPagerAdapter : FragmentPagerAdapter
    {

        public CSIBaseFragmentPagerAdapter(Android.Support.V4.App.FragmentManager fm, View ContextView)
            : base(fm)
        {
            return;
        }

        public override int Count
        {
            get { return 0; }
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return null;
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return null;
        }

        protected void WriteErrorLog(Exception Ex)
        {
            CSIErrorLog.WriteErrorLog(Ex);
        }

        protected void WriteLog(string content)
        {
            CSIErrorLog.WriteLog(content);
        }
    }
}