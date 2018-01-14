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
using System.Threading.Tasks;
using static CSIMobile.Class.Common.CSIMessageDialog;

namespace CSIMobile.Class.Fragments
{
    public class DCQuantityMoveFragment : CSIBaseFullScreenDialogFragment
    {
        ImageButton ItemScanButton;
        EditText ItemEdit;

        public DCQuantityMoveFragment(CSIBaseActivity activity = null) : base(activity)
        {
            CSISystemContext.ReadConfigurations();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                var view = inflater.Inflate(Resource.Layout.CSIQuantityMove, container, false);

                ItemScanButton = view.FindViewById<ImageButton>(Resource.Id.ItemScanButton);
                ItemEdit = view.FindViewById<EditText>(Resource.Id.ItemEdit);

                ItemScanButton.Click += ItemScanButton_Click;
                return view;
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return null;
            }
        }

        private async void ItemScanButton_Click(object sender, EventArgs e)
        {
            ItemEdit.Text = await CSISanner.ScanAsync();
        }
    }
}