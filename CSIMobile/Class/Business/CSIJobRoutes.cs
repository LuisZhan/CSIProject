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
    public class CSIJobRoutes : CSIBaseDataObject
    {
        public CSIJobRoutes(CSIContext SrcContext = null) : base(SrcContext)
        {
            IDOName = "SLJobRoutes";
        }

        protected override void InitialPreopertyList()
        {
            base.InitialPreopertyList();
            PreSetPropertyList.Add("Job");
            PreSetPropertyList.Add("Suffix");
            PreSetPropertyList.Add("OperNum");
            PreSetPropertyList.Add("Wc");
            PreSetPropertyList.Add("QtyReceived");
            PreSetPropertyList.Add("QtyMoved");
            PreSetPropertyList.Add("QtyComplete");
            PreSetPropertyList.Add("QtyScrapped");
            PreSetPropertyList.Add("Complete");
        }

        public static bool GetOperationInfor(CSIContext SrcContext, string Job, string Suffix, ref string OperNum, ref string Wc, ref string QtyReceived)
        {
            try
            {
                CSIJobRoutes SLJobRoute = new CSIJobRoutes(SrcContext);
                SLJobRoute.UseSync(false);
                SLJobRoute.AddProperty("Job");
                SLJobRoute.AddProperty("Suffix");
                SLJobRoute.AddProperty("OperNum");
                SLJobRoute.AddProperty("Wc");
                SLJobRoute.AddProperty("QtyReceived");
                SLJobRoute.SetFilter(string.Format("Job = N'{0}' And Suffix = N'{1}'", Job, Suffix));
                SLJobRoute.LoadIDO();
                if (SLJobRoute.CurrentTable.Rows.Count <= 0)
                {
                    return false;
                }
                OperNum = SLJobRoute.GetCurrentPropertyValueOfString("OperNum");
                Wc = SLJobRoute.GetCurrentPropertyValueOfString("Wc");
                QtyReceived = SLJobRoute.GetCurrentPropertyValueOfString("QtyReceived");
                SLJobRoute = null;
                return true;
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return false;
            }
        }
    }
}