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
    class CSIBaseDataObject : CSIBaseObject
    {
        private string IDOName = "";

        public CSIBaseDataObject() : base()
        {

        }

        public CSIBaseDataObject(CSIContext MyContext) : base(MyContext)
        {

        }
    }
}