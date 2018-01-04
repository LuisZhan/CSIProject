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
    [Activity(Label = "Activity1")]
    public class CSIBaseActivity : Activity
    {
        CSIContext CSIContext = new CSIContext();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                CSIContext.ParseBundle(Intent.GetBundleExtra("CSIContext"));
                base.OnCreate(savedInstanceState);
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }

            // Create your application here
        }

        protected void WriteErrorLog(Exception Ex)
        {
            if (CSIContext.DisplayWhenError)
            {
                Toast.MakeText(this, Ex.Message, ToastLength.Long).Show();
            }
            CSIErrorLog.WriteErrorLog(Ex);
        }

        protected void WriteLog(string content)
        {
            CSIErrorLog.WriteLog(content);
        }

        protected void WriteLog()
        {
            CSIErrorLog.WriteLog(CSIContext);
        }
    }
}