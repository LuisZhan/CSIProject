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

        public void SetMyTheme()
        {
            if (!string.IsNullOrEmpty(CSISystemContext.Theme))
            {
                switch (CSISystemContext.Theme)
                {
                    case "Light":
                        Application.SetTheme(Resource.Style.MyTheme_Light_Base);
                        SetTheme(Resource.Style.MyTheme_Light_Base);
                        break;
                    default:
                        Application.SetTheme(Resource.Style.MyTheme_Base);
                        SetTheme(Resource.Style.MyTheme_Base);
                        break;
                }
            }
            else
            {
                Application.SetTheme(Resource.Style.MyTheme_Base);
                SetTheme(Resource.Style.MyTheme_Base);
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            SetMyTheme();
            CSISystemContext.ParseBundle(Intent.GetBundleExtra("CSISystemContext"));
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                if (CSISystemContext == null)
                {
                    CSISystemContext = new CSIContext();
                }
                SetMyTheme();
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