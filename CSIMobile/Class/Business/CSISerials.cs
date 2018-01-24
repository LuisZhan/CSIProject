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
    public class CSISerials : CSIBaseDataObject
    {
        public CSISerials(CSIContext SrcContext = null) : base(SrcContext)
        {
            IDOName = "SLSerials";
        }

        protected override void InitialPreopertyList()
        {
            base.InitialPreopertyList();
            PreSetPropertyList.Add("Whse");
            PreSetPropertyList.Add("Item");
            PreSetPropertyList.Add("ItemDescription");
            PreSetPropertyList.Add("SerNum");
            PreSetPropertyList.Add("Loc");
            PreSetPropertyList.Add("Lot");
            PreSetPropertyList.Add("Logifld");
            PreSetPropertyList.Add("Stat");
            PreSetPropertyList.Add("PurgeDate");
            PreSetPropertyList.Add("ShipDate");
            PreSetPropertyList.Add("ExpDate");
            PreSetPropertyList.Add("ContainerNum");
        }

        public static void SerialLoadSp()
        {

        }
    }
}