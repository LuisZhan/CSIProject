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
    public class CSIDcpos : CSIDcBase
    {
        public CSIDcpos(CSIContext SrcContext = null) : base(SrcContext)
        {
            IDOName = "SLDcpos";
        }

        protected override void InitialPreopertyList()
        {
            base.InitialPreopertyList();
            PreSetPropertyList.Add("TransNum");
            PreSetPropertyList.Add("TransType");
            PreSetPropertyList.Add("Stat"); 
            PreSetPropertyList.Add("Termid");
            PreSetPropertyList.Add("TransDate");
            PreSetPropertyList.Add("EmpNum");
            PreSetPropertyList.Add("PoNum");
            PreSetPropertyList.Add("PoLine");
            PreSetPropertyList.Add("PoRelease");
            PreSetPropertyList.Add("PoitemStat");
            PreSetPropertyList.Add("Item");
            PreSetPropertyList.Add("UM");
            PreSetPropertyList.Add("ItemDescription"); 
            PreSetPropertyList.Add("ItemLotTracked");
            PreSetPropertyList.Add("ItemSerialTracked");
            PreSetPropertyList.Add("PoitemUM");
            PreSetPropertyList.Add("Whse");
            PreSetPropertyList.Add("Loc");
            PreSetPropertyList.Add("Lot");
            PreSetPropertyList.Add("QtyReceived");
            PreSetPropertyList.Add("QtyRejected");
            PreSetPropertyList.Add("QtyReturned");
            PreSetPropertyList.Add("ReasonCode");
            PreSetPropertyList.Add("ReasonDescription");
            PreSetPropertyList.Add("DocumentNum");
            PreSetPropertyList.Add("ErrorMessage");
            PreSetPropertyList.Add("Override");
            PreSetPropertyList.Add("CanOverride");
            PreSetPropertyList.Add("ItemSerialPrefix"); 
        }
    }
}