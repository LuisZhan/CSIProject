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
    public class CSITrnitems : CSIBaseDataObject
    {
        public CSITrnitems(CSIContext SrcContext = null) : base(SrcContext)
        {
            IDOName = "SLTrnitems";
        }

        protected override void InitialPreopertyList()
        {
            base.InitialPreopertyList();
            PreSetPropertyList.Add("TrnNum");
            PreSetPropertyList.Add("TrnLine");
            PreSetPropertyList.Add("TrnLoc");
            PreSetPropertyList.Add("TraStat");
            PreSetPropertyList.Add("TraFromSite");
            PreSetPropertyList.Add("TraFromWhse");
            PreSetPropertyList.Add("FromItLotTracked");
            PreSetPropertyList.Add("FromItSerialTracked");
            PreSetPropertyList.Add("TraToSite");
            PreSetPropertyList.Add("TraToWhse");
            PreSetPropertyList.Add("TraFobSite");
            PreSetPropertyList.Add("ToItLotTracked");
            PreSetPropertyList.Add("ToItSerialTracked");
            PreSetPropertyList.Add("Stat");
            PreSetPropertyList.Add("QtyReq");
            PreSetPropertyList.Add("QtyShipped");
            PreSetPropertyList.Add("QtyReceived");
            PreSetPropertyList.Add("QtyLoss");
            PreSetPropertyList.Add("Item");
            PreSetPropertyList.Add("ItDescription");
            PreSetPropertyList.Add("UM"); 
            PreSetPropertyList.Add("InvUseExistingSerials");
        }

        public static bool GetTransferLineInfor(CSIContext SrcContext, ref string TrnNum, ref string TrnLine, ref string Item, ref string ItemDesc, ref string UM, ref string TrnLoc, ref bool InvUseExistingSerials
            , ref string QtyReq, ref string QtyShipped, ref string QtyReceived, ref string QtyRequired, ref bool FromLotTracked, ref bool FromSNTracked, ref bool ToLotTracked, ref bool ToSNTracked, ref bool FobFromSite)
        {
            string FromSite, ToSite;
            try
            {
                CSITrnitems SLTrnitem = new CSITrnitems(SrcContext);
                SLTrnitem.UseAsync(false);
                SLTrnitem.AddProperty("TrnNum");
                SLTrnitem.AddProperty("TrnLine"); 
                SLTrnitem.AddProperty("TrnLoc");
                SLTrnitem.AddProperty("Item");
                SLTrnitem.AddProperty("ItDescription");
                SLTrnitem.AddProperty("UM");
                SLTrnitem.AddProperty("QtyReq");
                SLTrnitem.AddProperty("QtyShipped");
                SLTrnitem.AddProperty("QtyReceived");
                SLTrnitem.AddProperty("ToItLotTracked");
                SLTrnitem.AddProperty("ToItSerialTracked"); 
                SLTrnitem.AddProperty("InvUseExistingSerials");
                SLTrnitem.AddProperty("TraFromSite");
                SLTrnitem.AddProperty("TraToSite");
                SLTrnitem.AddProperty("ToFobSite");
                if (string.IsNullOrEmpty(TrnLine))
                {
                    TrnLine = "1";
                }
                SLTrnitem.SetFilter(string.Format("TrnNum = N'{0}' And TrnLine = {1}", TrnNum, TrnLine));
                SLTrnitem.LoadIDO();
                if (SLTrnitem.CurrentTable.Rows.Count <= 0)
                {
                    return false;
                }
                TrnNum = SLTrnitem.GetCurrentPropertyValueOfString("TrnNum");
                TrnLine = SLTrnitem.GetCurrentPropertyValueOfString("TrnLine");
                TrnLoc = SLTrnitem.GetCurrentPropertyValueOfString("TrnLoc");
                Item = SLTrnitem.GetCurrentPropertyValueOfString("Item");
                ItemDesc = SLTrnitem.GetCurrentPropertyValueOfString("ItDescription");
                UM = SLTrnitem.GetCurrentPropertyValueOfString("UM");
                QtyReq = string.Format("{0:n}", SLTrnitem.GetCurrentPropertyValueOfString("QtyReq"));
                QtyShipped = string.Format("{0:n}", SLTrnitem.GetCurrentPropertyValueOfString("QtyShipped"));
                QtyReceived = string.Format("{0:n}", SLTrnitem.GetCurrentPropertyValueOfString("QtyReceived"));
                FromSite = SLTrnitem.GetCurrentPropertyValueOfString("TraFromSite");
                ToSite = SLTrnitem.GetCurrentPropertyValueOfString("TraToSite");
                if (SrcContext.Site == FromSite)
                {
                    //ship from site
                    QtyRequired = string.Format("{0:n}", (SLTrnitem.GetCurrentPropertyValueOfDecimal("QtyReq") - SLTrnitem.GetCurrentPropertyValueOfDecimal("QtyShipped")).ToString());
                }
                if (SrcContext.Site == FromSite)
                {
                    //receive to site
                    QtyRequired = string.Format("{0:n}", (SLTrnitem.GetCurrentPropertyValueOfDecimal("QtyShipped") - SLTrnitem.GetCurrentPropertyValueOfDecimal("QtyReceived")).ToString());
                }
                FromLotTracked = SLTrnitem.GetCurrentPropertyValueOfBoolean("FromItLotTracked");
                FromSNTracked = SLTrnitem.GetCurrentPropertyValueOfBoolean("FromItSerialTracked");
                ToLotTracked = SLTrnitem.GetCurrentPropertyValueOfBoolean("ToItLotTracked");
                ToSNTracked = SLTrnitem.GetCurrentPropertyValueOfBoolean("ToItSerialTracked");
                InvUseExistingSerials = SLTrnitem.GetCurrentPropertyValueOfBoolean("InvUseExistingSerials");
                FobFromSite = SLTrnitem.GetCurrentPropertyValueOfString("TraFromSite") == SLTrnitem.GetCurrentPropertyValueOfString("TraFobSite");
                SLTrnitem = null;
                return true;
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return false;
            }
        }
    }
}