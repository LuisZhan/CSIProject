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
    public class CSIItemLocs : CSIBaseDataObject
    {
        public CSIItemLocs(CSIContext SrcContext = null) : base(SrcContext)
        {
            IDOName = "SLItemLocs";
        }

        protected override void InitialPreopertyList()
        {
            base.InitialPreopertyList();
            PreSetPropertyList.Add("Item");
            PreSetPropertyList.Add("Loc");
            PreSetPropertyList.Add("LocDescription");
            PreSetPropertyList.Add("QtyOnHand");
            PreSetPropertyList.Add("QtyRsvd");
            PreSetPropertyList.Add("Whse");
            PreSetPropertyList.Add("Wc");
            PreSetPropertyList.Add("Rank");
        }

        public static bool GetItemLocInfor(CSIContext SrcContext, string Item, string Whse, ref string Loc, ref string LocDescription, ref string Qty)
        {
            try
            {
                CSIItemLocs SLItemLoc = new CSIItemLocs(SrcContext);
                SLItemLoc.UseAsync(false);
                SLItemLoc.AddProperty("Items");
                SLItemLoc.AddProperty("Loc");
                SLItemLoc.AddProperty("LocDescription");
                SLItemLoc.AddProperty("QtyOnHand");
                SLItemLoc.AddProperty("Whse");
                SLItemLoc.AddProperty("Rank");
                if (string.IsNullOrEmpty(Loc))
                {
                    SLItemLoc.SetFilter(string.Format("Item = N'{0}' AND Whse =  N'{1}'", Item, Whse));
                }
                else
                {
                    SLItemLoc.SetFilter(string.Format("Item = N'{0}' AND Whse =  N'{1}' And Loc = N'{2}'", Item, Whse, Loc));
                }
                SLItemLoc.SetOrderBy("Rank");
                SLItemLoc.SetRecordCap(1);
                SLItemLoc.LoadIDO();
                if (SLItemLoc.CurrentTable.Rows.Count <= 0)
                {
                    return false;
                }
                Loc = SLItemLoc.GetCurrentPropertyValueOfString("Loc");
                LocDescription = SLItemLoc.GetCurrentPropertyValueOfString("LocDescription");
                Qty = string.Format("{0:n}", SLItemLoc.GetCurrentPropertyValueOfDecimal("QtyOnHand"));
                SLItemLoc = null;
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