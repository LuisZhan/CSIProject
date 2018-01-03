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

        protected void Log(string content)
        {
            CSIErrorLog.WriteLog(content);
        }

        protected void Log()
        {
            CSIErrorLog.WriteLog(CSIContext);
        }
    }
}