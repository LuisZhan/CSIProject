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
    public class CSIPoItems : CSIBaseDataObject
    {
        public CSIPoItems(CSIContext SrcContext = null) : base(SrcContext)
        {
            IDOName = "SLPoitems";
        }

        protected override void InitialPreopertyList()
        {
            base.InitialPreopertyList();
            PreSetPropertyList.Add("PoNum");
            PreSetPropertyList.Add("PoLine");
            PreSetPropertyList.Add("PoRelease");
            PreSetPropertyList.Add("PoType");
            PreSetPropertyList.Add("PoOrderDate");
            PreSetPropertyList.Add("PoStat");
            PreSetPropertyList.Add("Item");
            PreSetPropertyList.Add("DerItem");
            PreSetPropertyList.Add("UM");
            PreSetPropertyList.Add("Whse");
            PreSetPropertyList.Add("QtyOrdered");
            PreSetPropertyList.Add("QtyReceived");
            PreSetPropertyList.Add("QtyRejected");
            PreSetPropertyList.Add("QtyReturned");
            PreSetPropertyList.Add("Stat");
            PreSetPropertyList.Add("Description");
            PreSetPropertyList.Add("ItmLotTracked");
            PreSetPropertyList.Add("ItmSerialTracked");
            PreSetPropertyList.Add("PoVendNum");
        }

        public static bool GetPoNumInfor(CSIContext SrcContext, ref string PoNum, ref string Line, ref string Release, ref string Type, ref string Customer
            , ref string Item, ref string ItemDesc, ref string UM, ref string QtyOrdered, ref string QtyReceived, ref string QtyToBeReceived, ref bool LotTracked, ref bool SNTracked)
        {
            try
            {
                CSIPoItems SLPoItem = new CSIPoItems(SrcContext);
                SLPoItem.UseSync(false);
                SLPoItem.AddProperty("PoNum");
                SLPoItem.AddProperty("PoLine");
                SLPoItem.AddProperty("PoRelease");
                SLPoItem.AddProperty("PoType");
                SLPoItem.AddProperty("QtyOrdered"); 
                SLPoItem.AddProperty("QtyReceived");
                SLPoItem.AddProperty("PoVendNum");
                SLPoItem.AddProperty("Item");
                SLPoItem.AddProperty("Description");
                SLPoItem.AddProperty("UM");
                SLPoItem.AddProperty("ItmLotTracked");
                SLPoItem.AddProperty("ItmSerialTracked");
                SLPoItem.SetFilter(string.Format("PoNum = N'{0}' AND Stat IN (N'O', N'F')", PoNum));
                if (!string.IsNullOrEmpty(Line))
                {
                    SLPoItem.SetFilter(string.Format("PoNum = N'{0}' AND PoLine = {1} AND Stat IN (N'O', N'F') ", PoNum, Line));
                    if (!string.IsNullOrEmpty(Release) | !Release.Equals("0"))
                    {
                        SLPoItem.SetFilter(string.Format("PoNum = N'{0}' AND PoLine = {1} AND PoRelease = {2} AND Stat IN (N'O', N'F')", PoNum, Line, Release));
                    }
                }
                SLPoItem.SetOrderBy("PoNum,PoLine,PoRelease");
                SLPoItem.SetRecordCap(1);
                SLPoItem.LoadIDO();
                if (SLPoItem.CurrentTable.Rows.Count <= 0)
                {
                    return false;
                }
                PoNum = SLPoItem.GetCurrentPropertyValueOfString("PoNum");
                Line = SLPoItem.GetCurrentPropertyValueOfString("PoLine");
                Release = SLPoItem.GetCurrentPropertyValueOfString("PoRelease");
                Customer = SLPoItem.GetCurrentPropertyValueOfString("PoVendNum");
                Item = SLPoItem.GetCurrentPropertyValueOfString("Item");
                ItemDesc = SLPoItem.GetCurrentPropertyValueOfString("Description");
                UM = SLPoItem.GetCurrentPropertyValueOfString("UM");
                QtyOrdered = string.Format("{0:n}", SLPoItem.GetCurrentPropertyValueOfDecimal("QtyOrdered"));
                QtyReceived = string.Format("{0:n}", SLPoItem.GetCurrentPropertyValueOfDecimal("QtyReceived"));
                QtyToBeReceived = string.Format("{0:n}", SLPoItem.GetCurrentPropertyValueOfDecimal("QtyOrdered") - SLPoItem.GetCurrentPropertyValueOfDecimal("QtyReceived"));
                LotTracked = SLPoItem.GetCurrentPropertyValueOfBoolean("ItmLotTracked");
                SNTracked = SLPoItem.GetCurrentPropertyValueOfBoolean("ItmSerialTracked");
                SLPoItem = null;
                return true;
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return false;
            }
        }
    }
}