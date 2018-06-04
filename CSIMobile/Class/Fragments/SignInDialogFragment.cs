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
        private CSIUserLocals UsersLocal;
        private CSIEmployees Employee;
        private CSIParms Parms;

        //public event CreateSessionTokenCompletedEventHandler CreateSessionTokenCompleted;

        public SignInDialogFragment(CSIBaseActivity activity = null) : base(activity)
        {
            Title = Application.Context.GetString(Resource.String.Login);

            try
            {
                //CreateSessionTokenCompleted += OnCreateSessionTokenCompleted;
                Users = new CSIUserNames(CSISystemContext);
                Users.CreateSessionTokenCompleted += OnCreateSessionTokenCompleted;
                Users.LoadDataSetCompleted += OnLoadDataSetCompleted;
                UsersLocal = new CSIUserLocals(CSISystemContext);
                UsersLocal.LoadDataSetCompleted += OnLoadDataSetCompleted;
                Employee = new CSIEmployees(CSISystemContext);
                Employee.LoadDataSetCompleted += OnLoadDataSetCompleted;
                Parms = new CSIParms(CSISystemContext);
                Parms.LoadDataSetCompleted += Parmse_LoadDataSetCompleted; ;
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
        }

        private void Parmse_LoadDataSetCompleted(object sender, LoadDataSetCompletedEventArgs e)
        {
            try
            {
                CSISystemContext.Site = Parms.GetCurrentPropertyValueOfString("Site");
                GetUserInfor();
                ShowProgressBar(false);
            }
            catch (Exception Ex)
            {
                ShowProgressBar(false);
                WriteErrorLog(Ex);
                WriteErrorLog(e.Error);
                CSISystemContext.Token = "";
                ErrorText.Text = CSIBaseInvoker.TranslateError(e.Error);
                ErrorText.Visibility = ViewStates.Visible;
            }
        }

        private void OnLoadDataSetCompleted(object sender, LoadDataSetCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (sender.GetType() == Users.GetType())
                    {
                        CSISystemContext.User = Users.GetCurrentPropertyValueOfString("Username");
                        CSISystemContext.UserDesc = Users.GetCurrentPropertyValueOfString("UserDesc");
                        GetUserLocalInfor();
                    }
                    if (sender.GetType() == UsersLocal.GetType())
                    {
                        CSISystemContext.DefaultWarehouse = UsersLocal.GetCurrentPropertyValueOfString("Whse");
                        if (string.IsNullOrEmpty(CSISystemContext.DefaultWarehouse)) CSISystemContext.DefaultWarehouse = "MAIN";
                        GetEmpInfor();
                    }
                    if (sender.GetType() == Employee.GetType())
                    {
                        CSISystemContext.EmpNum = Employee.GetCurrentPropertyValueOfString("EmpNum");
                        CSISystemContext.EmpName = Employee.GetCurrentPropertyValueOfString("Name");
                    }

                    ShowProgressBar(false);

                    if (ProgressBar.Visibility == ViewStates.Invisible)
                    {
                        ErrorText.Visibility = ViewStates.Invisible;
                        Dismiss();
                        Dispose();
                    }
                }
                else
                {
                    ShowProgressBar(false);
                    WriteErrorLog(e.Error);
                    CSISystemContext.Token = "";
                    ErrorText.Text = CSIBaseInvoker.TranslateError(e.Error);
                    ErrorText.Visibility = ViewStates.Visible;
                }
            }catch (Exception Ex)
            {
                ShowProgressBar(false);
                WriteErrorLog(Ex);
                WriteErrorLog(e.Error);
                CSISystemContext.Token = "";
                ErrorText.Text = CSIBaseInvoker.TranslateError(e.Error);
                ErrorText.Visibility = ViewStates.Visible;
            }

        }

        private void OnCreateSessionTokenCompleted(object sender, CreateSessionTokenCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                CSISystemContext.Token = e.Result;
                CSISystemContext.SavedUser = UserEdit.Text;
                CSISystemContext.SavedPassword = PasswordEdit.Text;
                CSISystemContext.SaveUser = SaveUserSwitch.Checked;
                CSISystemContext.SavePassword = SavePasswordSwitch.Checked;
                CSISystemContext.Configuration = (string)ConfigurationEdit.SelectedItem;
                CSISystemContext.WriteConfigurations();

                if (!string.IsNullOrEmpty(CSISystemContext.Token))
                {
                    GetParmInfor();
                }
            }
            else
            {
                WriteErrorLog(e.Error);
                ErrorText.Text = CSIBaseInvoker.TranslateError(e.Error);
                ErrorText.Visibility = ViewStates.Visible;
            }
            ShowProgressBar(false);
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                base.OnCreateView(inflater, container,savedInstanceState);

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
        }

        private void GetParmInfor()
        {
            ShowProgressBar(true);
            Parms.AddProperty("Site");
            Parms.LoadIDO();
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

        private void GetUserLocalInfor()
        {
            ShowProgressBar(true);
            UsersLocal.AddProperty("UserId");
            UsersLocal.AddProperty("Username");
            UsersLocal.AddProperty("UserDesc");
            UsersLocal.SetFilter(string.Format("Username = '{0}'", UserEdit.Text));
            UsersLocal.LoadIDO();
        }
        
        private void GetEmpInfor()
        {
            ShowProgressBar(true);
            Employee.AddProperty("EmpNum");
            Employee.AddProperty("Name");
            Employee.AddProperty("Username");
            Employee.SetFilter(string.Format("Username = '{0}'", UserEdit.Text));
            Employee.LoadIDO();
        }

        private void SetConfigurationSpin()
        {
            int index = 0, i = 0;
            ArrayAdapter adapter = new ArrayAdapter(Application.Context, Android.Resource.Layout.SimpleSpinnerItem);
            try
            {
                if ((CSISystemContext.ConfigurationList == null) || (CSISystemContext.ConfigurationList.Count <= 0))
                {
                    return;
                }
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
            if (show)
            {
                ProcessCount += 1;
                ProgressBar.Visibility = ViewStates.Visible;
                CSIBaseObject.DisableEnableControls(false, Layout);
            }
            else
            {
                ProcessCount -= ProcessCount == 0 ? 0 : 1;
                if(ProcessCount == 0)
                {
                    ProgressBar.Visibility = ViewStates.Invisible;
                    CSIBaseObject.DisableEnableControls(true, Layout);
                }
            }
            CloseImage.Visibility = HasTitle ? ViewStates.Gone : ViewStates.Visible;
        }
    }
}