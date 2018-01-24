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
    public class CSILots : CSIBaseDataObject
    {
        public CSILots(CSIContext SrcContext = null) : base(SrcContext)
        {
            IDOName = "SLLots";
        }

        protected override void InitialPreopertyList()
        {
            base.InitialPreopertyList();
            PreSetPropertyList.Add("Item");
            PreSetPropertyList.Add("ItemDescription");
            PreSetPropertyList.Add("ItemU_M");
            PreSetPropertyList.Add("ItemShelfLife");
            PreSetPropertyList.Add("Lot");
            PreSetPropertyList.Add("Logifld");
            PreSetPropertyList.Add("RcvdQty");
            PreSetPropertyList.Add("PurgeDate");
            PreSetPropertyList.Add("ManufacturerName");
            PreSetPropertyList.Add("ManufacturerId");
            PreSetPropertyList.Add("ManufacturerItem");
        }
    }
}