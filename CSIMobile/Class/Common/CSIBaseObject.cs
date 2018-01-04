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

    class CSIBaseObject : Object
    {
        private CSIContext CSIContext = new CSIContext();

        public CSIBaseObject()
        {
        }

        public CSIBaseObject(CSIContext MyContext)
        {
            SetCSIContext(MyContext);
        }

        public void SetCSIContext(CSIContext MyContext)
        {
            CSIContext = MyContext;
        }

        protected void WriteErrorLog(Exception Ex)
        {
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