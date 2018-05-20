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
using CSIMobile.Class.Common;

namespace CSIMobile.Class.Business 
{
    public class CSIWhses : CSIBaseDataObject
    {
        public CSIWhses(CSIContext SrcContext = null) : base(SrcContext)
        {
            IDOName = "SLWhses";
        }

        protected override void InitialPreopertyList()
        {
            //base.InitialPreopertyList();

            PreSetPropertyList.Add("RowPointer");
            PreSetPropertyList.Add("RecordDate");
            PreSetPropertyList.Add("Whse");
            PreSetPropertyList.Add("Name");
        }
    }
}