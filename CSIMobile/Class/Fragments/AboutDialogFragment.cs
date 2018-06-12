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
using CSIMobile.Class.Business;

namespace CSIMobile.Class.Fragments
{
    public class AboutDialogFragment : CSIBaseDialogFragment
    {
        private TextView UserText;
        private TextView UserNameText;
        private TextView EmployeeText;
        private TextView EmployeeNameText;
        private ListView ListView;
        private Dictionary<string, string> Whses = new Dictionary<string, string>();

        public AboutDialogFragment() : base()
        {

        }

        public AboutDialogFragment(CSIBaseActivity activity = null) : base(activity)
        {
            Title = Application.Context.GetString(Resource.String.About);

            CSISystemContext.ReadConfigurations();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                base.OnCreateView(inflater, container, savedInstanceState);

                var view = inflater.Inflate(Resource.Layout.CSIAbout, container, false);

                UserText = view.FindViewById<TextView>(Resource.Id.UserText);
                UserNameText = view.FindViewById<TextView>(Resource.Id.UserNameText);
                EmployeeText = view.FindViewById<TextView>(Resource.Id.EmployeeText);
                EmployeeNameText = view.FindViewById<TextView>(Resource.Id.EmployeeNameText);

                ListView = view.FindViewById<ListView>(Resource.Id.ListView);

                UserText.Text = CSISystemContext.User ?? CSISystemContext.UserDesc;
                UserNameText.Text = CSISystemContext.UserDesc ?? CSISystemContext.User;
                EmployeeText.Text = CSISystemContext.EmpNum ?? CSISystemContext.EmpName;
                EmployeeNameText.Text = CSISystemContext.EmpName;

                SetDetailList();
                return view;
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return null;
            }
        }

        private void SetDetailList()
        {
            ArrayAdapter adapter = new ArrayAdapter(Application.Context, Android.Resource.Layout.SimpleGalleryItem);
            adapter.Add(string.Format("{0}: {1}", Application.Context.GetString(Resource.String.Site), CSISystemContext.Site));
            adapter.Add(string.Format("{0}: {1}", Application.Context.GetString(Resource.String.Warehouse), CSISystemContext.DefaultWarehouse));
            adapter.Add(string.Format("{0}: {1}", Application.Context.GetString(Resource.String.Device), CSISystemContext.GetDeviceId()));
            adapter.Add(string.Format("{0}: {1}", Application.Context.GetString(Resource.String.RegisterLicense), Application.Context.GetString(Resource.String.NoLicensed)));
            adapter.Add(string.Format("{0}: {1}", Application.Context.GetString(Resource.String.ExpirationDate), CSISystemContext.ExpDate));
            ListView.Adapter = adapter;
        }

    }
}