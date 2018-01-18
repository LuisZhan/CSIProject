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
    public class LogDialogFragment : CSIBaseDialogFragment
    {
        private EditText LogEdit;
        private Button DeleteLogFileButton;

        public LogDialogFragment(CSIBaseActivity activity = null) : base(activity)
        {
            CSISystemContext.ReadConfigurations();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                var view = inflater.Inflate(Resource.Layout.CSILog, container, false);

                LogEdit = view.FindViewById<EditText>(Resource.Id.LogEdit);
                DeleteLogFileButton = view.FindViewById<Button>(Resource.Id.DeleteLogFileButton);

                LogEdit.Text = CSIErrorLog.ReadLog();

                DeleteLogFileButton.Click += DeleteLogFileButton_Click;

                return view;
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return null;
            }
        }

        private void DeleteLogFileButton_Click(object sender, EventArgs args)
        {
            CSIErrorLog.PrintLog(this.BaseActivity);
            FragmentTransaction ft = FragmentManager.BeginTransaction();

            CSIMessageDialog DeleteLogFileDialog = (CSIMessageDialog)FragmentManager.FindFragmentByTag("DeleteLogFileDialog");

            if (DeleteLogFileDialog != null)
            {
                ft.Show(DeleteLogFileDialog);
            }
            else
            {
                DeleteLogFileDialog = new CSIMessageDialog(GetString(Resource.String.app_name), GetString(Resource.String.DeleteLogFile), DialogTypes.OKCancel);
                DeleteLogFileDialog.OkHandler += (o, e) =>
                {
                    CSIErrorLog.DeleteErrorLogFile();
                    LogEdit.Text = CSIErrorLog.ReadLog();
                };
                DeleteLogFileDialog.Show(ft, "DeleteLogFileDialog");
            }
        }
    }
}