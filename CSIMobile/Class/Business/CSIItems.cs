﻿using System;
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
    public class CSIItems : CSIBaseDataObject
    {
        public CSIItems(CSIContext SrcContext = null) : base(SrcContext)
        {
            IDOName = "SLItems";
        }

        protected override void InitialPreopertyList()
        {
            base.InitialPreopertyList();
            PreSetPropertyList.Add("Item");
            PreSetPropertyList.Add("Description");
            PreSetPropertyList.Add("UM");
            PreSetPropertyList.Add("MatlType");
            PreSetPropertyList.Add("PMTCode");
            PreSetPropertyList.Add("ProductCode");
            PreSetPropertyList.Add("LotTracked");
            PreSetPropertyList.Add("SerialTracked");
            PreSetPropertyList.Add("Picture");
            PreSetPropertyList.Add("DerQtyOnHand");
            PreSetPropertyList.Add("DerQtyAvailable");
            PreSetPropertyList.Add("DerQtyOrdered");
            PreSetPropertyList.Add("DerQtyReorder");
            PreSetPropertyList.Add("DerQtyRsvdCo");
            PreSetPropertyList.Add("DerQtyTrans");
            PreSetPropertyList.Add("DerQtyWip");
            PreSetPropertyList.Add("QtyAllocjob");
            PreSetPropertyList.Add("DerQtyAllocCo");
        }

        public static bool GetItemInfor(CSIContext SrcContext, ref string Item, ref string Desc, ref string UM, ref string Qty, ref bool LotTracked, ref bool SNTracked)
        {
            try
            {
                CSIItems SLItem = new CSIItems(SrcContext);
                SLItem.UseSync(false);
                SLItem.AddProperty("Items");
                SLItem.AddProperty("Description");
                SLItem.AddProperty("UM");
                SLItem.AddProperty("LotTracked");
                SLItem.AddProperty("SerialTracked");
                SLItem.AddProperty("DerQtyOnHand");
                SLItem.SetFilter(string.Format("Item = N'{0}'", Item));
                SLItem.LoadIDO();
                if (SLItem.CurrentTable.Rows.Count <= 0)
                {
                    return false;
                }
                Item = SLItem.GetCurrentPropertyValueOfString("Item");
                Desc = SLItem.GetCurrentPropertyValueOfString("Description");
                UM = SLItem.GetCurrentPropertyValueOfString("UM");
                Qty = string.Format("{0:n}", SLItem.GetCurrentPropertyValueOfDecimal("DerQtyOnHand"));
                LotTracked = SLItem.GetCurrentPropertyValueOfBoolean("LotTracked");
                SNTracked = SLItem.GetCurrentPropertyValueOfBoolean("SerialTracked");
                SLItem = null;
                return true;
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return false;
            }
        }
    }
}