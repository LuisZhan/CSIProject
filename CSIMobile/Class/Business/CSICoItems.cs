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
    public class CSICoItems : CSIBaseDataObject
    {
        public CSICoItems(CSIContext SrcContext = null) : base(SrcContext)
        {
            IDOName = "SLCoitems";
        }

        protected override void InitialPreopertyList()
        {
            base.InitialPreopertyList();
            PreSetPropertyList.Add("CoNum");
            PreSetPropertyList.Add("CoLine");
            PreSetPropertyList.Add("CoRelease");
            PreSetPropertyList.Add("CoType");
            PreSetPropertyList.Add("CoOrderDate");
            PreSetPropertyList.Add("CoStat");
            PreSetPropertyList.Add("CoWhse");
            PreSetPropertyList.Add("DerItem");
            PreSetPropertyList.Add("UM");
            PreSetPropertyList.Add("Whse");
            PreSetPropertyList.Add("QtyOrdered");
            PreSetPropertyList.Add("QtyShipped");
            PreSetPropertyList.Add("QtyReturned");
            PreSetPropertyList.Add("Stat");
            PreSetPropertyList.Add("Description");
            PreSetPropertyList.Add("ItLotTracked");
            PreSetPropertyList.Add("ItSerialTracked");
            PreSetPropertyList.Add("ItStocked");
            PreSetPropertyList.Add("ItUM");
            PreSetPropertyList.Add("DerCustNum");
        }

        public static bool GetCoNumInfor(CSIContext SrcContext, ref string CoNum, ref string Line, ref string Release, ref string Type, ref string Customer
            , ref string Item, ref string ItemDesc, ref string UM, ref string QtyOrdered, ref bool LotTracked, ref bool SNTracked)
        {
            try
            {
                CSICoItems SLCoItem = new CSICoItems(SrcContext);
                SLCoItem.UseSync(false);
                SLCoItem.AddProperty("CoNum");
                SLCoItem.AddProperty("CoLine");
                SLCoItem.AddProperty("CoRelease");
                SLCoItem.AddProperty("CoType");
                SLCoItem.AddProperty("QtyOrdered");
                SLCoItem.AddProperty("DerCustNum");
                SLCoItem.AddProperty("DerItem");
                SLCoItem.AddProperty("Description");
                SLCoItem.AddProperty("UM");
                SLCoItem.AddProperty("ItLotTracked");
                SLCoItem.AddProperty("ItSerialTracked");
                SLCoItem.SetFilter(string.Format("CoNum = N'{0}' AND Stat IN (N'O', N'F')", CoNum));
                if (!string.IsNullOrEmpty(Line))
                {
                    SLCoItem.SetFilter(string.Format("CoNum = N'{0}' AND CoLine = {1} AND Stat IN (N'O', N'F') ", CoNum, Line));
                    if (!string.IsNullOrEmpty(Release) | !Release.Equals("0"))
                    {
                        SLCoItem.SetFilter(string.Format("CoNum = N'{0}' AND CoLine = {1} AND CoRelease = {2} AND Stat IN (N'O', N'F')", CoNum, Line, Release));
                    }
                }
                SLCoItem.SetOrderBy("CoNum,CoLine,CoRelease");
                SLCoItem.SetRecordCap(1);
                SLCoItem.LoadIDO();
                if (SLCoItem.CurrentTable.Rows.Count <= 0)
                {
                    return false;
                }
                CoNum = SLCoItem.GetCurrentPropertyValueOfString("CoNum");
                Line = SLCoItem.GetCurrentPropertyValueOfString("CoLine");
                Release = SLCoItem.GetCurrentPropertyValueOfString("CoRelease");
                Customer = SLCoItem.GetCurrentPropertyValueOfString("DerCustNum");
                Item = SLCoItem.GetCurrentPropertyValueOfString("DerItem");
                ItemDesc = SLCoItem.GetCurrentPropertyValueOfString("Description");
                UM = SLCoItem.GetCurrentPropertyValueOfString("UM");
                QtyOrdered = string.Format("{0:n}", SLCoItem.GetCurrentPropertyValueOfDecimal("QtyOrdered"));
                LotTracked = SLCoItem.GetCurrentPropertyValueOfBoolean("ItLotTracked");
                SNTracked = SLCoItem.GetCurrentPropertyValueOfBoolean("ItSerialTracked");
                SLCoItem = null;
                return true;
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return false;
            }
        }
    }
}