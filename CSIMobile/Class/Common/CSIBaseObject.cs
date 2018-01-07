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
        protected CSIContext CSISystemContext = new CSIContext();

        public CSIBaseObject()
        {
            CSISystemContext.File = GetType().ToString();
        }

        public CSIBaseObject(CSIContext MyContext)
        {
            SetCSIContext(MyContext);
        }

        public void SetCSIContext(CSIContext MyContext)
        {
            CSISystemContext = MyContext;
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
    }
}