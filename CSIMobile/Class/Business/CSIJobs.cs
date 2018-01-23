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
    public class CSIJobs : CSIBaseDataObject
    {
        public CSIJobs(CSIContext SrcContext = null) : base(SrcContext)
        {
            IDOName = "SLJobs";
        }

        protected override void InitialPreopertyList()
        {
            base.InitialPreopertyList();
            PreSetPropertyList.Add("Job");
            PreSetPropertyList.Add("Suffix");
            PreSetPropertyList.Add("Description");
            PreSetPropertyList.Add("Item");
            PreSetPropertyList.Add("ItemDescription");
            PreSetPropertyList.Add("ItemUM");
            PreSetPropertyList.Add("QtyReleased");
            PreSetPropertyList.Add("QtyComplete");
            PreSetPropertyList.Add("QtyScrapped");
            PreSetPropertyList.Add("Rework");
            PreSetPropertyList.Add("Stat");
            PreSetPropertyList.Add("Type");
            PreSetPropertyList.Add("Whse");
            PreSetPropertyList.Add("CoProductMix");
            PreSetPropertyList.Add("CustNum");
            PreSetPropertyList.Add("EffectDate");
            PreSetPropertyList.Add("JobDate");
            PreSetPropertyList.Add("LstTrxDate");
            PreSetPropertyList.Add("ProdMix");
            PreSetPropertyList.Add("ItemLotTracked");
            PreSetPropertyList.Add("ItemSerialTracked");
        }

        public static bool GetJobInfor(CSIContext SrcContext, ref string Job, ref string Suffix, ref string Desc, ref string Item, ref string ItemDesc, ref string ItemUM
            , ref string QtyReleased, ref string QtyComplete, ref string QtyRequired, ref bool LotTracked, ref bool SNTracked)
        {
            try
            {
                CSIJobs SLJob = new CSIJobs(SrcContext);
                SLJob.UseSync(false);
                SLJob.AddProperty("Job");
                SLJob.AddProperty("Suffix");
                SLJob.AddProperty("Description");
                SLJob.AddProperty("Item");
                SLJob.AddProperty("ItemUM");
                SLJob.AddProperty("ItemDescription");
                SLJob.AddProperty("QtyReleased");
                SLJob.AddProperty("QtyComplete");
                SLJob.AddProperty("ItemLotTracked");
                SLJob.AddProperty("ItemSerialTracked");
                if (string.IsNullOrEmpty(Suffix))
                {
                    Suffix = "0000";
                }
                SLJob.SetFilter(string.Format("Job = N'{0}' And Suffix = N'{1}'", Job, Suffix));
                SLJob.LoadIDO();
                if (SLJob.CurrentTable.Rows.Count <= 0)
                {
                    return false;
                }
                Job = SLJob.GetCurrentPropertyValueOfString("Job");
                Suffix = SLJob.GetCurrentPropertyValueOfInteger("Suffix").ToString("0000");
                Desc = SLJob.GetCurrentPropertyValueOfString("Description");
                Item = SLJob.GetCurrentPropertyValueOfString("Item");
                ItemDesc = SLJob.GetCurrentPropertyValueOfString("ItemDescription");
                ItemUM = SLJob.GetCurrentPropertyValueOfString("ItemUM");
                QtyReleased = SLJob.GetCurrentPropertyValueOfString("QtyReleased");
                QtyComplete = SLJob.GetCurrentPropertyValueOfString("QtyComplete");
                QtyRequired = (SLJob.GetCurrentPropertyValueOfDecimal("QtyReleased") - SLJob.GetCurrentPropertyValueOfDecimal("QtyComplete")).ToString();
                LotTracked = SLJob.GetCurrentPropertyValueOfBoolean("ItemLotTracked");
                SNTracked = SLJob.GetCurrentPropertyValueOfBoolean("ItemSerialTracked");
                SLJob = null;
                return true;
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return false;
            }
        }
    }
}