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
    public class CSIReasons : CSIBaseDataObject
    {
        public CSIReasons(CSIContext SrcContext = null) : base(SrcContext)
        {
            IDOName = "SLReasons";
        }

        protected override void InitialPreopertyList()
        {
            base.InitialPreopertyList();
            PreSetPropertyList.Add("ReasonClass");// 'MISC RCPT','MISC ISSUE','INV ADJUST','PO RETURN','CO RETURN'
            PreSetPropertyList.Add("ReasonCode");
            PreSetPropertyList.Add("Description");
        }

        public static bool GetReason(CSIContext SrcContext, ref string ReasonCode, string ReasonClass, ref string ReasonDescription)
        {
            try
            {
                CSIReasons SLReason = new CSIReasons(SrcContext);
                SLReason.UseAsync(false);
                SLReason.AddProperty("ReasonCode");
                SLReason.AddProperty("ReasonClass");
                SLReason.AddProperty("Description");
                SLReason.SetFilter(string.Format("ReasonCode = N'{0}' And ReasonClass = N'{1}'", ReasonCode, ReasonClass));
                SLReason.SetOrderBy("ReasonCode");
                SLReason.SetRecordCap(1);
                SLReason.LoadIDO();
                if (SLReason.CurrentTable.Rows.Count <= 0)
                {
                    return false;
                }
                ReasonCode = SLReason.GetCurrentPropertyValueOfString("ReasonCode");
                ReasonDescription = SLReason.GetCurrentPropertyValueOfString("Description");
                SLReason = null;
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