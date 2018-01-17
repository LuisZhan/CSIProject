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
    public class CSILocations : CSIBaseDataObject
    {
        public CSILocations(CSIContext SrcContext = null) : base(SrcContext)
        {
            IDOName = "SLLocations";
        }

        protected override void InitialPreopertyList()
        {
            base.InitialPreopertyList();
            PreSetPropertyList.Add("Loc");
            PreSetPropertyList.Add("LocType");
            PreSetPropertyList.Add("Description");
            PreSetPropertyList.Add("Wc"); 
            PreSetPropertyList.Add("WcDescription");
        }
    }
}