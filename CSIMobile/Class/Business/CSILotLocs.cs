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
    public class CSILotLocs : CSIBaseDataObject
    {
        public CSILotLocs(CSIContext SrcContext = null) : base(SrcContext)
        {
            IDOName = "SLLotLocs";
        }

        protected override void InitialPreopertyList()
        {
            base.InitialPreopertyList();
            PreSetPropertyList.Add("Item");
            PreSetPropertyList.Add("ItemUM");
            PreSetPropertyList.Add("ItemDescription");
            PreSetPropertyList.Add("Loc");
            PreSetPropertyList.Add("LocationDescription");
            PreSetPropertyList.Add("Whse");
            PreSetPropertyList.Add("WhseName");
            PreSetPropertyList.Add("Lot"); 
            PreSetPropertyList.Add("QtyOnHand");
        }

        public static bool GetItemLotLocInfor(CSIContext SrcContext, string Item, string Whse, string Loc, ref string Lot, ref string Qty)
        {
            try
            {
                CSILotLocs SLLotLocs = new CSILotLocs(SrcContext);
                SLLotLocs.UseAsync(false);
                SLLotLocs.AddProperty("Item");
                SLLotLocs.AddProperty("Loc");
                SLLotLocs.AddProperty("Lot");
                SLLotLocs.AddProperty("QtyOnHand");
                if (string.IsNullOrEmpty(Lot))
                {
                    SLLotLocs.SetFilter(string.Format("Item = N'{0}' And Whse = N'{1}' And Loc = N'{2}'", Item, Whse, Loc));
                }
                else
                {
                    SLLotLocs.SetFilter(string.Format("Item = N'{0}' And Whse = N'{1}' And Loc = N'{2}' And Lot = N'{3}'", Item, Whse, Loc, Lot));
                }
                SLLotLocs.SetOrderBy("Lot");
                SLLotLocs.SetRecordCap(1);
                SLLotLocs.LoadIDO();
                if (SLLotLocs.CurrentTable.Rows.Count <= 0)
                {
                    return false;
                }
                Loc = SLLotLocs.GetCurrentPropertyValueOfString("Loc");
                Lot = SLLotLocs.GetCurrentPropertyValueOfString("Lot");
                Qty = string.Format("{0:n}", SLLotLocs.GetCurrentPropertyValueOfDecimal("QtyOnHand"));
                SLLotLocs = null;
                return true;
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return false;
            }
        }
    }
}