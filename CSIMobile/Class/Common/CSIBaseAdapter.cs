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
using Java.Lang;

namespace CSIMobile.Class.Common
{
    public class CSIBaseAdapter : BaseAdapter
    {
        protected CSIContext CSISystemContext;

        public CSIBaseAdapter(CSIContext SrcContext = null)
        {
            CSISystemContext = new CSIContext(SrcContext)
            {
                Adapter = "CSIBaseAdapter"
            };
        }

        public override int Count => throw new NotImplementedException();

        public override Java.Lang.Object GetItem(int position)
        {
            throw new NotImplementedException();
        }

        public override long GetItemId(int position)
        {
            throw new NotImplementedException();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            throw new NotImplementedException();
        }
    }
}