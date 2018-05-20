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
    public class CSIJobmatls : CSIBaseDataObject
    {
        public CSIJobmatls(CSIContext SrcContext = null) : base(SrcContext)
        {
            IDOName = "SLJobmatls";
        }

        protected override void InitialPreopertyList()
        {
            base.InitialPreopertyList();
            PreSetPropertyList.Add("Job");
            PreSetPropertyList.Add("Suffix");
            PreSetPropertyList.Add("OperNum");
            PreSetPropertyList.Add("Sequence");
            PreSetPropertyList.Add("Item");
            PreSetPropertyList.Add("Description");
            PreSetPropertyList.Add("QtyIssued");
            PreSetPropertyList.Add("DerQtyConv");
            PreSetPropertyList.Add("UM");
            PreSetPropertyList.Add("DerLoc");
            PreSetPropertyList.Add("DerLot");
            PreSetPropertyList.Add("DerItemLotTracked");
            PreSetPropertyList.Add("DerItemSerialTracked");
        }

        public static bool GetItemInfor(CSIContext SrcContext, string Job, string Suffix, string OperNum, ref string Item, ref string Desc, ref string UM, ref string ReqQty, ref bool LotTracked, ref bool SNTracked)
        {
            try
            {
                CSIJobmatls SLJobmatl = new CSIJobmatls(SrcContext);
                SLJobmatl.UseAsync(false);
                SLJobmatl.AddProperty("Job");
                SLJobmatl.AddProperty("Suffix");
                SLJobmatl.AddProperty("OperNum");
                SLJobmatl.AddProperty("Sequence");
                SLJobmatl.AddProperty("Item");
                SLJobmatl.AddProperty("Description");
                SLJobmatl.AddProperty("DerQtyConv");
                SLJobmatl.AddProperty("UM");
                SLJobmatl.AddProperty("DerLoc");
                SLJobmatl.AddProperty("DerLot");
                SLJobmatl.AddProperty("DerItemLotTracked");
                SLJobmatl.AddProperty("DerItemSerialTracked");
                SLJobmatl.SetFilter(string.Format("Job = N'{0}' And Suffix = N'{1}' And OperNum = N'{2}' And Item = N'{3}'", Job, Suffix, OperNum, Item));
                SLJobmatl.SetOrderBy("Job,Suffix,OperNum,Sequence");
                SLJobmatl.LoadIDO();
                if (SLJobmatl.CurrentTable.Rows.Count <= 0)
                {
                    return false;
                }
                Item = SLJobmatl.GetCurrentPropertyValueOfString("Item");
                Desc = SLJobmatl.GetCurrentPropertyValueOfString("Description");
                UM = SLJobmatl.GetCurrentPropertyValueOfString("UM");
                ReqQty = string.Format("{0:n}", SLJobmatl.GetCurrentPropertyValueOfString("QtyReceived"));
                LotTracked = SLJobmatl.GetCurrentPropertyValueOfBoolean("DerItemLotTracked");
                SNTracked = SLJobmatl.GetCurrentPropertyValueOfBoolean("DerItemSerialTracked");
                SLJobmatl = null;
                return true;
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return false;
            }
        }
    }
}