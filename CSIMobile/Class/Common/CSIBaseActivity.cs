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
using Android.Support.V7.App;

namespace CSIMobile.Class.Common
{
    [Activity(Label = "@string/app_name")]
    public class CSIBaseActivity : AppCompatActivity
    {
        protected CSIContext CSISystemContext;

        public CSIBaseActivity()
        {

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                if (CSISystemContext == null)
                {
                    CSISystemContext = new CSIContext();
                }
                base.OnCreate(savedInstanceState);
                CSISystemContext.ParseBundle(Intent.GetBundleExtra("CSISystemContext"));
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }

            // Create your application here
        }

        public virtual bool InvokeCommand(string Command, Dictionary<String, Object> ParmList = null)
        {
            return true;
        }

        public CSIContext GetCSISystemContext()
        {
            return CSISystemContext;
        }

        protected void WriteErrorLog(Exception Ex)
        {
            if (CSISystemContext.DisplayWhenError)
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
            CSIErrorLog.WriteLog(CSISystemContext);
        }

        public string GetToken()
        {
            return CSISystemContext.Token;
        }
    }
}