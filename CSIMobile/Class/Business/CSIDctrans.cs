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
    public class CSIDctrans : CSIDcBase
    {
        public CSIDctrans(CSIContext SrcContext = null) : base(SrcContext)
        {
            IDOName = "SLDctrans";
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
            PreSetPropertyList.Add("TrnNum");
            PreSetPropertyList.Add("TrnLine");
            PreSetPropertyList.Add("Loc");
            PreSetPropertyList.Add("Lot");
            PreSetPropertyList.Add("TrnLot");
            PreSetPropertyList.Add("UseExistingSerials");
            PreSetPropertyList.Add("Whse");
            PreSetPropertyList.Add("Qty");
            PreSetPropertyList.Add("UM");
            PreSetPropertyList.Add("ReasonCode");
            PreSetPropertyList.Add("DocumentNum");
            PreSetPropertyList.Add("ErrorMessage");
            PreSetPropertyList.Add("Override");
            PreSetPropertyList.Add("CanOverride");
            PreSetPropertyList.Add("ItemSerialPrefix"); 
        }
    }
}