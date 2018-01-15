using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace CSIMobile.Class.Common
{
    public class CSIBaseDialogFragment : DialogFragment
    {
        protected CSIBaseActivity BaseActivity;
        protected CSIContext CSISystemContext;


        public CSIBaseDialogFragment(CSIBaseActivity activity = null)
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

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public virtual void SetBaseActivity(CSIBaseActivity activity)
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

        protected virtual void WriteErrorLog(Exception Ex)
        {
            if (CSISystemContext.DisplayWhenError)
            {
                Toast.MakeText(Application.Context, Ex.Message, ToastLength.Long).Show();
            }
            CSIErrorLog.WriteErrorLog(Ex);
        }

        protected virtual void WriteLog(string content)
        {
            CSIErrorLog.WriteLog(content);
        }

        protected virtual void WriteLog()
        {
            CSIErrorLog.WriteLog(CSISystemContext);
        }
    }
}