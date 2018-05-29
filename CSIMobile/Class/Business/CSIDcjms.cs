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
    public class CSIDcjms : CSIDcBase
    {
        public CSIDcjms(CSIContext SrcContext = null) : base(SrcContext)
        {
            IDOName = "SLDcjms";
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
            PreSetPropertyList.Add("Item");
            PreSetPropertyList.Add("ItemLotTracked");
            PreSetPropertyList.Add("ItemSerialTracked");
            PreSetPropertyList.Add("JobmatlUM");
            PreSetPropertyList.Add("UM");
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
            CSIDcjms dcjms = new CSIDcjms(SrcContext);
            dcjms.AddProperty("TransNum");
            dcjms.SetOrderBy("TransNum Desc");
            dcjms.RecordCap = 1;
            dcjms.UseAsync(false);
            dcjms.LoadIDO();
            if (dcjms.CurrentTable.Rows.Count > 0)
            {
                TransNum = dcjms.GetCurrentPropertyValueOfInteger("TransNum") + 1;
            }
            return TransNum;
        }
    }
}