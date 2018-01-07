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

        public SignInDialogFragment() : base()
        {
            CSISystemContext.Fragment = GetType().ToString();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                var view = inflater.Inflate(Resource.Layout.CSISignIn, container, false);
                
                UserEdit = view.FindViewById<EditText>(Resource.Id.UserEdit);
                PasswordEdit = view.FindViewById<EditText>(Resource.Id.PasswordEdit);
                SaveUserSwitch = view.FindViewById<Switch>(Resource.Id.SaveUserSwitch);
                SavePasswordSwitch = view.FindViewById<Switch>(Resource.Id.SavePasswordSwitch);
                SignInButton = view.FindViewById<Button>(Resource.Id.SignInButton);
                ErrorText = view.FindViewById<TextView>(Resource.Id.ErrorText);

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
                        { "SavePassword", SavePasswordSwitch.Checked }
                    };
                    if (BaseActivity.InvokeCommand("GetToken", ParmList))
                    {
                        ErrorText.Visibility = ViewStates.Gone;
                        Dismiss();
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
    }
}