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

namespace CSIMobile.Class.Fragments
{
    public class AboutDialogFragment : CSIBaseDialogFragment
    {
        private TextView UserText;
        private TextView UserNameText;
        private TextView EmployeeText;
        private TextView EmployeeNameText;
        private ListView ListView;

        public AboutDialogFragment(CSIBaseActivity activity = null) : base(activity)
        {
            CSISystemContext.ReadConfigurations();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                var view = inflater.Inflate(Resource.Layout.CSIAbout, container, false);

                UserText = view.FindViewById<TextView>(Resource.Id.UserText);
                UserNameText = view.FindViewById<TextView>(Resource.Id.UserNameText);
                EmployeeText = view.FindViewById<TextView>(Resource.Id.EmployeeText);
                EmployeeNameText = view.FindViewById<TextView>(Resource.Id.EmployeeNameText);

                ListView = view.FindViewById<ListView>(Resource.Id.ListView);

                UserText.Text = CSISystemContext.User ?? CSISystemContext.UserDesc;
                UserNameText.Text = CSISystemContext.UserDesc;
                EmployeeText.Text = CSISystemContext.EmpNum ?? CSISystemContext.EmpName;
                EmployeeNameText.Text = CSISystemContext.EmpName;

                return view;
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return null;
            }
        }
    }
}