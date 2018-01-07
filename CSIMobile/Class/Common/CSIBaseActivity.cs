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
    [Activity(Theme = "@style/MyTheme", Label = "Activity1")]
    public class CSIBaseActivity : AppCompatActivity
    {
        protected CSIContext CSISystemContext = new CSIContext();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                CSISystemContext.ParseBundle(Intent.GetBundleExtra("CSIContext"));
                CSISystemContext.Activity = GetType().ToString();
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

        protected void WriteErrorLog(Exception Ex)
        {
            //if (CSISystemContext.DisplayWhenError)
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