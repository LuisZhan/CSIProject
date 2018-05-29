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
    public class CSIDcBase : CSIBaseDataObject
    {
        public CSIDcBase(CSIContext SrcContext = null) : base(SrcContext)
        {
        }

        protected override void InitialPreopertyList()
        {
            base.InitialPreopertyList();
        }

        protected virtual string GetLot()
        {
            return "";
        }

        protected virtual string GetNextLot()
        {
            return "";
        }

        protected virtual string GetSN()
        {
            return "";
        }

        public virtual int NextTransNum(CSIContext SrcContext = null)
        {
            return 1;
        }
    }
}