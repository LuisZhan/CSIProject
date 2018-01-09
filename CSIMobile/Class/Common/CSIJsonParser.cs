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
    public class CSIJsonParser : CSIBaseObject
    {
        public CSIJsonParser(CSIContext SrcContext = null) : base(SrcContext)
        {
            CSISystemContext.File = "CSIJsonParser";

        }
    }
}