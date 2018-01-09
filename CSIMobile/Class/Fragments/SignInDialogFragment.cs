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

namespace CSIMobile.Class.Fragments
{
    public class SignInDialogFragment : CSIBaseDialogFragment
    {
        private EditText UserEdit;
        private EditText PasswordEdit;
        private Switch SaveUserSwitch;
        private Switch SavePasswordSwitch;
        private Button SignInButton;
        private TextView ErrorText;
        private Spinner ConfigurationEdit;
        private ImageView CloseImage;

        public SignInDialogFragment(CSIBaseActivity activity = null) : base(activity)
        {
            CSISystemContext.Fragment = "SignInDialogFragment";
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                Cancelable = false;

                var view = inflater.Inflate(Resource.Layout.CSISignIn, container, false);
                
                UserEdit = view.FindViewById<EditText>(Resource.Id.UserEdit);
                PasswordEdit = view.FindViewById<EditText>(Resource.Id.PasswordEdit);
                SaveUserSwitch = view.FindViewById<Switch>(Resource.Id.SaveUserSwitch);
                SavePasswordSwitch = view.FindViewById<Switch>(Resource.Id.SavePasswordSwitch);
                SignInButton = view.FindViewById<Button>(Resource.Id.SignInButton);
                ErrorText = view.FindViewById<TextView>(Resource.Id.ErrorText);
                ConfigurationEdit = view.FindViewById<Spinner>(Resource.Id.ConfigurationEdit);
                CloseImage = view.FindViewById<ImageView>(Resource.Id.CloseImage);

                UserEdit.Text = CSISystemContext.SavedUser;
                PasswordEdit.Text = CSISystemContext.SavedPassword;
                SaveUserSwitch.Checked = CSISystemContext.SaveUser;
                SavePasswordSwitch.Checked = CSISystemContext.SavePassword;

                SetConfigurationSpin();

                CloseImage.Click += (sender, args) =>
                {
                    Dismiss();
                    Dispose();
                };

                // Set up a handler to dismiss this DialogFragment when this button is clicked.
                SignInButton.Click += (sender, args) =>
                {
                    if (string.IsNullOrEmpty(UserEdit.Text))
                    {
                        return;
                    }
                    Dictionary<String, Object> ParmList = new Dictionary<string, object>
                    {
                        { "User", UserEdit.Text },
                        { "Password", PasswordEdit.Text },
                        { "SaveUser", SaveUserSwitch.Checked },
                        { "SavePassword", SavePasswordSwitch.Checked },
                        { "Configuration", (string)ConfigurationEdit.SelectedItem }
                    };
                    if (BaseActivity.InvokeCommand("CreateToken", ParmList))
                    {
                        CSISystemContext.SavedUser = UserEdit.Text;
                        CSISystemContext.SavedPassword = PasswordEdit.Text;
                        CSISystemContext.SaveUser = SaveUserSwitch.Checked;
                        CSISystemContext.SavePassword = SavePasswordSwitch.Checked;
                        CSISystemContext.Configuration = (string)ConfigurationEdit.SelectedItem;
                        CSISystemContext.WriteConfigurations();
                        ErrorText.Visibility = ViewStates.Gone;
                        Dismiss();
                        Dispose();
                    }
                    else
                    {
                        ErrorText.Text = GetString(Resource.String.WrongUserOrPassword);
                        ErrorText.Visibility = ViewStates.Visible;
                    }
                    ParmList.Clear();
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
                ConfigurationEdit.Adapter = adapter;
                ConfigurationEdit.SetSelection(index);
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
        }
    }
}