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
        private ProgressBar ProgressBar;
        private LinearLayout Layout;

        private CSIUserNames Users;
        private CSIUserLocals UsersLocals;

        //public event CreateSessionTokenCompletedEventHandler CreateSessionTokenCompleted;

        public SignInDialogFragment(CSIBaseActivity activity = null) : base(activity)
        {
            //CreateSessionTokenCompleted += OnCreateSessionTokenCompleted;
            Users = new CSIUserNames(CSISystemContext);
            Users.CreateSessionTokenCompleted += OnCreateSessionTokenCompleted;
            Users.LoadDataSetCompleted += OnLoadDataSetCompleted;
            UsersLocals = new CSIUserLocals(CSISystemContext);
            UsersLocals.LoadDataSetCompleted += OnLoadDataSetCompleted;
        }

        private void OnLoadDataSetCompleted(object sender, LoadDataSetCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Users.ConvertDataSet(e.Result);
                CSISystemContext.UserDesc = Users.CSIDataSet.GetCurrentObjectString("UserDesc");
                ErrorText.Visibility = ViewStates.Gone;
                Dismiss();
                Dispose();
            }
            else
            {
                WriteErrorLog(e.Error);
                ErrorText.Text = CSIBaseInvoker.TranslateError(e.Error);
                ErrorText.Visibility = ViewStates.Visible;
            }
        }

        private void OnCreateSessionTokenCompleted(object sender, CreateSessionTokenCompletedEventArgs e)
        {
            ShowProgressBar(false);
            if (e.Error == null)
            {
                CSISystemContext.Token = e.Result;
                CSISystemContext.SavedUser = UserEdit.Text;
                CSISystemContext.SavedPassword = PasswordEdit.Text;
                CSISystemContext.SaveUser = SaveUserSwitch.Checked;
                CSISystemContext.SavePassword = SavePasswordSwitch.Checked;
                CSISystemContext.Configuration = (string)ConfigurationEdit.SelectedItem;
                CSISystemContext.WriteConfigurations();

                GetUserInfor();
            }
            else
            {
                WriteErrorLog(e.Error);
                ErrorText.Text = CSIBaseInvoker.TranslateError(e.Error);
                ErrorText.Visibility = ViewStates.Visible;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                Cancelable = false;

                var view = inflater.Inflate(Resource.Layout.CSISignIn, container, false);

                Layout = view.FindViewById<LinearLayout>(Resource.Id.LinearLayout);
                UserEdit = view.FindViewById<EditText>(Resource.Id.UserEdit);
                PasswordEdit = view.FindViewById<EditText>(Resource.Id.PasswordEdit);
                SaveUserSwitch = view.FindViewById<Switch>(Resource.Id.SaveUserSwitch);
                SavePasswordSwitch = view.FindViewById<Switch>(Resource.Id.SavePasswordSwitch);
                SignInButton = view.FindViewById<Button>(Resource.Id.SignInButton);
                ErrorText = view.FindViewById<TextView>(Resource.Id.ErrorText);
                ConfigurationEdit = view.FindViewById<Spinner>(Resource.Id.ConfigurationEdit);
                CloseImage = view.FindViewById<ImageView>(Resource.Id.CloseImage);
                ProgressBar = view.FindViewById<ProgressBar>(Resource.Id.ProgressBar);

                UserEdit.Text = CSISystemContext.SavedUser;
                PasswordEdit.Text = CSISystemContext.SavedPassword;
                SaveUserSwitch.Checked = CSISystemContext.SaveUser;
                SavePasswordSwitch.Checked = CSISystemContext.SavePassword;

                ShowProgressBar(false);
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
                    SignIn();
                };
                
                return view;
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return null;
            }
        }

        private void SignIn()
        {
            ShowProgressBar(true);
            CSISystemContext.User = UserEdit.Text;
            CSISystemContext.Password = PasswordEdit.Text;
            CSISystemContext.SaveUser = SaveUserSwitch.Checked;
            CSISystemContext.SavePassword = SavePasswordSwitch.Checked;
            CSISystemContext.Configuration = (string)ConfigurationEdit.SelectedItem;
            CSISystemContext.Token = Users.CreateToken();
            //Dictionary<String, Object> ParmList = new Dictionary<string, object>
            //{
            //    { "User", UserEdit.Text },
            //    { "Password", PasswordEdit.Text },
            //    { "SaveUser", SaveUserSwitch.Checked },
            //    { "SavePassword", SavePasswordSwitch.Checked },
            //    { "Configuration", (string)ConfigurationEdit.SelectedItem },
            //    { "EnableHTTPS", CSISystemContext.EnableHTTPS },
            //    { "UseAsync", true },
            //    { "CreateSessionTokenCompleted", CreateSessionTokenCompleted }
            //};
            //try
            //{
            //    ShowProgressBar(true);
            //    BaseActivity.InvokeCommand("CreateToken", ParmList);

            //}
            //catch (Exception Ex)
            //{
            //    WriteErrorLog(Ex);
            //    ErrorText.Text = GetString(Resource.String.WrongUserOrPassword);
            //    ErrorText.Visibility = ViewStates.Visible;
            //}
            //ParmList.Clear();
        }

        private void GetUserInfor()
        {
            ShowProgressBar(true);
            Users.AddProperty("UserId");
            Users.AddProperty("Username");
            Users.AddProperty("UserDesc");
            Users.SetFilter(string.Format("Username = '{0}'", UserEdit.Text));
            Users.LoadIDO();
        }

        private void GetEmpInfor()
        {
            ShowProgressBar(true);
            Users.AddProperty("UserId");
            Users.AddProperty("Username");
            Users.AddProperty("UserDesc");
            Users.SetFilter(string.Format("Username = '{0}'", UserEdit.Text));
            Users.LoadIDO();
        }

        private void SetConfigurationSpin()
        {
            int index = 0, i = 0;
            ArrayAdapter adapter = new ArrayAdapter(Context, Android.Resource.Layout.SimpleSpinnerItem);
            try
            {
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
                ConfigurationEdit.Adapter = adapter;
                ConfigurationEdit.SetSelection(index);
            }
        }

        private void ShowProgressBar(bool show)
        {
            ProgressBar.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
            CSIBaseObject.DisableEnableControls(!show, Layout);
        }


    }
}