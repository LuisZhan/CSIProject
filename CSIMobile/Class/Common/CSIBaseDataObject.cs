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
    public class CSIBaseDataObject : CSIBaseObject
    {
        protected string IDOName = "";

        public CSIBaseDataObject() : base()
        {
            CSISystemContext.File = GetType().ToString();
            CSISystemContext.IDO = IDOName;
        }

        public CSIBaseDataObject(CSIContext MyContext) : base(MyContext)
        {

        }
    }
}