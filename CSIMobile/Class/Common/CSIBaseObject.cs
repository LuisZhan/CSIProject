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

    public class CSIBaseObject : Object
    {
        protected CSIContext CSISystemContext;
        public static string DrawableType = "Drawable";
        public static string StringType = "String";
        public static string LayoutType = "Layout";
        public static string ColorType = "Color";

        public CSIBaseObject(CSIContext SrcContext = null)
        {
            if (SrcContext == null)
            {
                CSISystemContext = new CSIContext();
            }
            else
            {
                CSISystemContext = SrcContext;
            }
        }

        public void SetCSIContext(CSIContext SrcContext)
        {
            if (SrcContext == null)
            {
                if (CSISystemContext == null)
                {
                    CSISystemContext = new CSIContext();
                }
            }
            else
            {
                CSISystemContext = SrcContext;
            }
        }

        public CSIContext GetCSISystemContext()
        {
            return CSISystemContext;
        }

        protected static void WriteErrorLog(Exception Ex)
        {
            Toast.MakeText(Application.Context, Ex.Message, ToastLength.Short).Show();
            CSIErrorLog.WriteErrorLog(Ex);
        }

        protected static void WriteLog(string content)
        {
            CSIErrorLog.WriteLog(content);
        }

        protected void WriteLog()
        {
            CSIErrorLog.WriteLog(CSISystemContext);
        }

        public static void DisableEnableControls(bool enable, ViewGroup vg)
        {
            for (int i = 0; i < vg.ChildCount; i++)
            {
                View child = vg.GetChildAt(i);
                child.Enabled = enable;
                try
                {
                    DisableEnableControls(enable, (ViewGroup)child);
                }
                catch
                {
                    continue;
                }
            }
        }

        public static string GetResourceStringByName(string type, string name)
        {
            string r_str = string.Empty;
            try
            {
                r_str = Application.Context.Resources.GetString(GetResourceIdByName(type, name));
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                r_str = string.Empty;
            }
            return r_str;
        }

        public static int GetResourceIdByName(string type, string name)
        {
            int r_id = 0;
            var r_Drawable = typeof(Resource.Drawable);
            var r_String = typeof(Resource.String);
            var r_Layout = typeof(Resource.Layout);
            var r_Color = typeof(Resource.Color);

            try
            {
                switch (type)
                {
                    case "Drawable":
                        var field_Drawable = r_Drawable.GetField(name);
                        r_id = (int)field_Drawable.GetValue(field_Drawable.Name);
                        break;
                    case "String":
                        var field_String = r_String.GetField(name);
                        r_id = (int)field_String.GetValue(field_String.Name);
                        break;
                    case "Layout":
                        var field_Layout = r_Layout.GetField(name);
                        r_id = (int)field_Layout.GetValue(field_Layout.Name);
                        break;
                    case "Color":
                        var field_Color = r_Color.GetField(name);
                        r_id = (int)field_Color.GetValue(field_Color.Name);
                        break;
                }
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return 0;
            }

            return r_id;            
        }
    }
}