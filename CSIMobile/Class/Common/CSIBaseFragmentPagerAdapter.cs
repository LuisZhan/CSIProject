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
    public class CSIBaseFragmentPagerAdapter : FragmentPagerAdapter
    {
        protected CSIBaseActivity BaseActivity;
        protected CSIContext CSISystemContext;

        public CSIBaseFragmentPagerAdapter(Android.Support.V4.App.FragmentManager fm, CSIBaseActivity activity = null)
            : base(fm)
        {
            if (activity == null)
            {
                CSISystemContext = new CSIContext();
            }
            else
            {
                BaseActivity = activity;
                if (BaseActivity.GetCSISystemContext() == null)
                {
                    CSISystemContext = new CSIContext();
                }
                else
                {
                    CSISystemContext = BaseActivity.GetCSISystemContext();
                }
            }
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