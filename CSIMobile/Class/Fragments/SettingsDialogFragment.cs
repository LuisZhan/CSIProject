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
    public class SettingsDialogFragment : CSIBaseDialogFragment
    {
        private EditText CSIWebServerEdit;
        private Switch EnableHTTPS;
        private Switch UseRESTForRequestSwitch;
        private Switch LoadPictureSwitch;
        private EditText UserEdit;
        private EditText PasswordEdit;
        private Spinner ConfigurationSpinner;
        private Button SaveButton;
        private Button TestButton;
        private EditText RecordCapEdit;
        private Switch SaveUserSwitch;
        private Switch SavePasswordSwitch;
        private ImageView CloseImage;

        public SettingsDialogFragment(CSIBaseActivity activity = null) : base(activity)
        {
            CSISystemContext.Fragment = "SettingsDialogFragment";
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                Cancelable = false;
                var view = inflater.Inflate(Resource.Layout.CSISettings, container, false);

                CSIWebServerEdit = view.FindViewById<EditText>(Resource.Id.CSIWebServerEdit);
                EnableHTTPS = view.FindViewById<Switch>(Resource.Id.EnableHTTPSEdit);
                UseRESTForRequestSwitch = view.FindViewById<Switch>(Resource.Id.UseRESTForRequestEdit);
                UserEdit = view.FindViewById<EditText>(Resource.Id.UserEdit);
                PasswordEdit = view.FindViewById<EditText>(Resource.Id.PasswordEdit);
                ConfigurationSpinner = view.FindViewById<Spinner>(Resource.Id.ConfigurationEdit);
                LoadPictureSwitch = view.FindViewById<Switch>(Resource.Id.LoadPictureEdit);
                RecordCapEdit = view.FindViewById<EditText>(Resource.Id.RecordCapEdit);
                SaveButton = view.FindViewById<Button>(Resource.Id.SaveButton);
                TestButton = view.FindViewById<Button>(Resource.Id.TestButton);
                SaveUserSwitch = view.FindViewById<Switch>(Resource.Id.SaveUserSwitch);
                SavePasswordSwitch = view.FindViewById<Switch>(Resource.Id.SavePasswordSwitch);
                CloseImage = view.FindViewById<ImageView>(Resource.Id.CloseImage);

                CSIWebServerEdit.Text = CSISystemContext.CSIWebServerName;
                UserEdit.Text = CSISystemContext.SavedUser;
                PasswordEdit.Text = CSISystemContext.SavedPassword;
                EnableHTTPS.Checked = CSISystemContext.EnableHTTPS;
                UseRESTForRequestSwitch.Checked = CSISystemContext.UseRESTForRequest;
                LoadPictureSwitch.Checked = CSISystemContext.LoadPicture;
                RecordCapEdit.Text = CSISystemContext.RecordCap;
                SaveUserSwitch.Checked = CSISystemContext.SaveUser;
                SavePasswordSwitch.Checked = CSISystemContext.SavePassword;

                UserEdit.Enabled = SaveUserSwitch.Checked;
                PasswordEdit.Enabled = SavePasswordSwitch.Checked;

                SetConfigurationSpin();

                CloseImage.Click += (sender, args) =>
                {
                    Dismiss();
                    Dispose();
                };

                SaveUserSwitch.CheckedChange += (o, e) =>
                {
                    UserEdit.Enabled = SaveUserSwitch.Checked;
                    UserEdit.Text = SaveUserSwitch.Checked ? UserEdit.Text : "";
                };

                SavePasswordSwitch.CheckedChange += (o, e) =>
                {
                    PasswordEdit.Enabled = SavePasswordSwitch.Checked;
                    PasswordEdit.Text = SavePasswordSwitch.Checked ? PasswordEdit.Text : "";
                };

                // Set up a handler to dismiss this DialogFragment when this button is clicked.
                SaveButton.Click += (sender, args) =>
                {
                    SaveConfiguration();
                    Dismiss();
                    Dispose();
                };

                TestButton.Click += (sender, args) =>
                {
                    CSISystemContext.ConfigurationList = new List<string>(CSIBaseInvoker.GetConfigurationList(CSISystemContext));

                    //set Configuration.adapter
                    SetConfigurationSpin();
                    SaveConfiguration();
                };

                return view;
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return null;
            }
        }

        private void SetConfigurationSpin()
        {
            int index = 0, i = 0;
            try
            {
                ArrayAdapter adapter = new ArrayAdapter(Context, Android.Resource.Layout.SimpleSpinnerItem);
                foreach (string config in CSISystemContext.ConfigurationList)
                {
                    adapter.Add(config);
                    if (CSISystemContext.Configuration == config)
                    {
                        index = i;
                    }
                    i += 1;
                }
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                ConfigurationSpinner.Adapter = adapter;
                ConfigurationSpinner.SetSelection(index);
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
        }

        private void SaveConfiguration()
        {
            CSISystemContext.CSIWebServerName = CSIWebServerEdit.Text;
            CSISystemContext.Configuration = (string)ConfigurationSpinner.SelectedItem;
            CSISystemContext.SavedUser = UserEdit.Text;
            CSISystemContext.SavedPassword = PasswordEdit.Text;
            CSISystemContext.EnableHTTPS = EnableHTTPS.Checked;
            CSISystemContext.UseRESTForRequest = UseRESTForRequestSwitch.Checked;
            CSISystemContext.LoadPicture = LoadPictureSwitch.Checked;
            CSISystemContext.RecordCap = RecordCapEdit.Text;
            CSISystemContext.SaveUser = SaveUserSwitch.Checked;
            CSISystemContext.SavePassword = SavePasswordSwitch.Checked;

            CSISystemContext.WriteConfigurations();
        }
    }
}