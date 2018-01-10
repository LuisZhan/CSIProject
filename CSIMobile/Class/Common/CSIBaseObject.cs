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

        public CSIBaseObject(CSIContext SrcContext = null)
        {
            CSISystemContext = new CSIContext(SrcContext)
            {
                File = "CSIBaseObject"
            };
        }

        public void SetCSIContext(CSIContext SrcContext)
        {
            if (CSISystemContext == null)
            {
                CSISystemContext = new CSIContext(SrcContext);
            }
            else
            {
                CSIContext.Copy(SrcContext, CSISystemContext);
            }
        }

        public CSIContext GetCSISystemContext()
        {
            return CSISystemContext;
        }

        protected static void WriteErrorLog(Exception Ex)
        {
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
    }
}