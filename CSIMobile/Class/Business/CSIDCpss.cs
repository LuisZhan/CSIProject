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
    public class CSIDcpss : CSIDcBase
    {
        public CSIDcpss(CSIContext SrcContext = null) : base(SrcContext)
        {
            IDOName = "SLDcpss";
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
            PreSetPropertyList.Add("Job");
            PreSetPropertyList.Add("Suffix");
            PreSetPropertyList.Add("JobItem");
            PreSetPropertyList.Add("CoProductMix");
            PreSetPropertyList.Add("DerCoItem");
            PreSetPropertyList.Add("OperNum");
            PreSetPropertyList.Add("JobrtWc");
            PreSetPropertyList.Add("JobItemLotTracked");
            PreSetPropertyList.Add("JobItemSerialTracked");
            PreSetPropertyList.Add("ItemSerialPrefix");
            PreSetPropertyList.Add("Whse");
            PreSetPropertyList.Add("Loc");
            PreSetPropertyList.Add("Lot");
            PreSetPropertyList.Add("Qty");
            PreSetPropertyList.Add("DocumentNum");
            PreSetPropertyList.Add("ErrorMessage");
            PreSetPropertyList.Add("Override");
            PreSetPropertyList.Add("CanOverride");
        }

        public override int NextTransNum(CSIContext SrcContext = null)
        {
            if (SrcContext is null)
            {
                SrcContext = this.CSISystemContext;
            }
            int TransNum = base.NextTransNum();
            CSIDcpss dcpss = new CSIDcpss(SrcContext);
            dcpss.AddProperty("TransNum");
            dcpss.SetOrderBy("TransNum Desc");
            dcpss.RecordCap = 1;
            dcpss.UseAsync(false);
            dcpss.LoadIDO();
            if (dcpss.CurrentTable.Rows.Count > 0)
            {
                TransNum = dcpss.GetCurrentPropertyValueOfInteger("TransNum") + 1;
            }
            return TransNum;
        }
    }
}