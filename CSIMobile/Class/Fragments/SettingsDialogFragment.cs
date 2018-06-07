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
    public class SettingsDialogFragment : CSIBaseDialogFragment
    {
        private EditText CSIWebServerEdit;
        private Switch EnableHTTPSSwitch;
        private Switch UseRESTForRequestSwitch;
        private Switch LoadPictureSwitch;
        private Switch ForceAutoPostSwitch;
        private Switch ShowSuccessMessageSwitch;
        private EditText UserEdit;
        private EditText PasswordEdit;
        private Spinner ConfigurationSpinner;
        private Spinner ThemeSpinner;
        private Button SaveButton;
        private Button TestButton;
        private EditText RecordCapEdit;
        private Switch SaveUserSwitch;
        private Switch SavePasswordSwitch;
        private ImageView CloseImage;
        private ProgressBar ProgressBar;
        private LinearLayout Layout;
        private string CSIWebServerName;
        private string Theme;
        private string Configuration;
        private string SavedUser;
        private string SavedPassword;
        private bool EnableHTTPS;
        private bool UseRESTForRequest;
        private bool LoadPicture;
        private bool ForceAutoPost;
        private bool ShowSuccessMessage;
        private string RecordCap;
        private bool SaveUser;
        private bool SavePassword;

        public SettingsDialogFragment() : base()
        {
        }

        public SettingsDialogFragment(CSIBaseActivity activity = null) : base(activity)
        {
            CSISystemContext.ReadConfigurations();
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Title = Application.Context.GetString(Resource.String.Settings);

            try
            {
                base.OnCreateView(inflater, container, savedInstanceState);
                var view = inflater.Inflate(Resource.Layout.CSISettings, container, false);

                Layout = view.FindViewById<LinearLayout>(Resource.Id.LinearLayout);
                CSIWebServerEdit = view.FindViewById<EditText>(Resource.Id.CSIWebServerEdit);
                EnableHTTPSSwitch = view.FindViewById<Switch>(Resource.Id.EnableHTTPSEdit);
                UseRESTForRequestSwitch = view.FindViewById<Switch>(Resource.Id.UseRESTForRequestEdit);
                UserEdit = view.FindViewById<EditText>(Resource.Id.UserEdit);
                PasswordEdit = view.FindViewById<EditText>(Resource.Id.PasswordEdit);
                ConfigurationSpinner = view.FindViewById<Spinner>(Resource.Id.ConfigurationEdit);
                ThemeSpinner = view.FindViewById<Spinner>(Resource.Id.ThemeSpinner); 
                LoadPictureSwitch = view.FindViewById<Switch>(Resource.Id.LoadPictureEdit);
                ForceAutoPostSwitch = view.FindViewById<Switch>(Resource.Id.ForceAutoPostEdit);
                ShowSuccessMessageSwitch = view.FindViewById<Switch>(Resource.Id.ShowSuccessMessageEdit);
                RecordCapEdit = view.FindViewById<EditText>(Resource.Id.RecordCapEdit);
                SaveButton = view.FindViewById<Button>(Resource.Id.SaveButton);
                TestButton = view.FindViewById<Button>(Resource.Id.TestButton);
                SaveUserSwitch = view.FindViewById<Switch>(Resource.Id.SaveUserSwitch);
                SavePasswordSwitch = view.FindViewById<Switch>(Resource.Id.SavePasswordSwitch);
                CloseImage = view.FindViewById<ImageView>(Resource.Id.CloseImage);
                ProgressBar = view.FindViewById<ProgressBar>(Resource.Id.ProgressBar);

                CSIWebServerEdit.Text = CSISystemContext.CSIWebServerName;
                UserEdit.Text = CSISystemContext.SavedUser;
                PasswordEdit.Text = CSISystemContext.SavedPassword;
                EnableHTTPSSwitch.Checked = CSISystemContext.EnableHTTPS;
                UseRESTForRequestSwitch.Checked = CSISystemContext.UseRESTForRequest;
                LoadPictureSwitch.Checked = CSISystemContext.LoadPicture;
                ForceAutoPostSwitch.Checked = CSISystemContext.ForceAutoPost;
                ShowSuccessMessageSwitch.Checked = CSISystemContext.ShowSuccessMessage; 
                RecordCapEdit.Text = CSISystemContext.RecordCap;
                SaveUserSwitch.Checked = CSISystemContext.SaveUser;
                SavePasswordSwitch.Checked = CSISystemContext.SavePassword;

                CSIWebServerName = CSISystemContext.CSIWebServerName;
                Configuration = CSISystemContext.Configuration;
                Theme = CSISystemContext.Theme;
                SavedUser = CSISystemContext.SavedUser;
                SavedPassword = CSISystemContext.SavedPassword;
                EnableHTTPS = CSISystemContext.EnableHTTPS;
                UseRESTForRequest = CSISystemContext.UseRESTForRequest;
                LoadPicture = CSISystemContext.LoadPicture;
                ForceAutoPost = CSISystemContext.ForceAutoPost;
                ShowSuccessMessage = CSISystemContext.ShowSuccessMessage;
                RecordCap = CSISystemContext.RecordCap;
                SaveUser = CSISystemContext.SaveUser;
                SavePassword = CSISystemContext.SavePassword;

                UserEdit.Enabled = SaveUserSwitch.Checked;
                PasswordEdit.Enabled = SavePasswordSwitch.Checked;

                ForceAutoPostSwitch.Enabled = (CSISystemContext.Token != string.Empty && CSISystemContext.User == "sa");//only can be changed by SA
                ShowSuccessMessageSwitch.Enabled = (CSISystemContext.Token != string.Empty && CSISystemContext.User == "sa");//only can be changed by SA
                SavePasswordSwitch.Enabled = (CSISystemContext.Token != string.Empty && CSISystemContext.User == "sa");//only can be changed by SA

                ShowProgressBar(false);

                SeThemeSpin();
                SetConfigurationSpin();
                
                CSIWebServerEdit.KeyPress += CSIWebServerEdit_KeyPress;

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
                    GetConfiguration();
                };

                Dialog.KeyPress += Dialog_KeyPress;

                return view;
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return null;
            }
        }

        private void CSIWebServerEdit_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Enter || e.KeyCode == Keycode.Tab)
            {
                if (e.Event.Action == KeyEventActions.Up)
                {
                    //GetConfiguration();
                    //SetConfigurationSpin();
                }
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }
        
        protected override void Dialog_KeyPress(object sender, DialogKeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Back)
            {
                if (e.Event.Action == KeyEventActions.Up)
                {
                    if (ValidateExit(true))
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        e.Handled = false;
                    }
                }
            }
            else
            {
                e.Handled = false;
            }
        }

        private void OnGetConfigurationNamesCompleted(object sender, GetConfigurationNamesCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                CSISystemContext.ConfigurationList = new List<string>(e.Result);
                SetConfigurationSpin();
            }
            else
            {
                WriteErrorLog(e.Error);
                //Toast.MakeText(Application.Context, CSIBaseInvoker.TranslateError(e.Error), ToastLength.Short).Show();
            }
            ShowProgressBar(false);
        }

        private void SeThemeSpin()
        {
            ArrayAdapter adapter = new ArrayAdapter(Application.Context, Android.Resource.Layout.SimpleSpinnerItem);
            try
            {
                adapter.Add(GetString(Resource.String.LightTheme));
                adapter.Add(GetString(Resource.String.DarkTheme));
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerItem);
                ThemeSpinner.Adapter = adapter;
                if (CSISystemContext.Theme == InterpretThemeValue(GetString(Resource.String.LightTheme)))
                {
                    ThemeSpinner.SetSelection(0);
                }
                else
                {
                    ThemeSpinner.SetSelection(1);
                }
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                ThemeSpinner.Adapter = adapter;
                ThemeSpinner.SetSelection(0);
            }
        }

        private string InterpretThemeValue(string theme)
        {
            if (theme == GetString(Resource.String.LightTheme))
            {
                return "Light";
            }
            else
            {
                return "Dark";
            }
        }

        private void GetConfiguration()
        {
            ShowProgressBar(true);
            CSISystemContext.CSIWebServerName = CSIWebServerEdit.Text;
            CSISystemContext.Theme = InterpretThemeValue((string)ThemeSpinner.SelectedItem ?? string.Empty);
            CSISystemContext.Configuration = (string)ConfigurationSpinner.SelectedItem ?? string.Empty;
            CSISystemContext.SavedUser = UserEdit.Text;
            CSISystemContext.SavedPassword = PasswordEdit.Text;
            CSISystemContext.EnableHTTPS = EnableHTTPSSwitch.Checked;
            CSISystemContext.UseRESTForRequest = UseRESTForRequestSwitch.Checked;
            CSISystemContext.LoadPicture = LoadPictureSwitch.Checked;
            CSISystemContext.ForceAutoPost = ForceAutoPostSwitch.Checked;
            CSISystemContext.ShowSuccessMessage = ShowSuccessMessageSwitch.Checked;
            CSISystemContext.RecordCap = RecordCapEdit.Text;
            CSISystemContext.SaveUser = SaveUserSwitch.Checked;
            CSISystemContext.SavePassword = SavePasswordSwitch.Checked;
            CSIBaseInvoker invoker = new CSIBaseInvoker(CSISystemContext)
            {
                UseAsync = true
            };
            invoker.GetConfigurationNamesCompleted += OnGetConfigurationNamesCompleted;
            CSISystemContext.ConfigurationList = new List<string>(invoker.GetConfigurationList());
            if (CSISystemContext.ConfigurationList is null)
            {
                CSISystemContext.ConfigurationList = new List<string> { string.Empty };
                ShowProgressBar(false);
            }
        }

        private void SetConfigurationSpin()
        {
            int index = 0, i = 0;
            ArrayAdapter adapter = new ArrayAdapter(Application.Context, Android.Resource.Layout.SimpleSpinnerItem);
            try
            {
                if ((CSISystemContext.ConfigurationList == null) || (CSISystemContext.ConfigurationList.Count <= 0))
                {
                    adapter.Add(string.Empty);
                }
                else
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
                }
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                ConfigurationSpinner.Adapter = adapter;
                ConfigurationSpinner.SetSelection(index);
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                ConfigurationSpinner.Adapter = adapter;
                ConfigurationSpinner.SetSelection(index);
            }
        }

        private bool ValidateExit(bool Exit = false)
        {
            bool IsChanged = false;
            try
            {
                IsChanged = !(
                            CSIWebServerName == CSIWebServerEdit.Text &&
                            Configuration == ((string)ConfigurationSpinner.SelectedItem ?? string.Empty) &&
                            Theme == InterpretThemeValue(((string)ThemeSpinner.SelectedItem ?? string.Empty)) &&
                            SavedUser == UserEdit.Text &&
                            SavedPassword == PasswordEdit.Text &&
                            EnableHTTPS == EnableHTTPSSwitch.Checked &&
                            UseRESTForRequest == UseRESTForRequestSwitch.Checked &&
                            LoadPicture == LoadPictureSwitch.Checked &&
                            ForceAutoPost == ForceAutoPostSwitch.Checked &&
                            ShowSuccessMessage == ShowSuccessMessageSwitch.Checked &&
                            RecordCap == RecordCapEdit.Text &&
                            SaveUser == SaveUserSwitch.Checked &&
                            SavePassword == SavePasswordSwitch.Checked
                            );
                if (IsChanged)
                {
                    FragmentTransaction ft = FragmentManager.BeginTransaction();
                    CSIMessageDialog SignOutDialog = new CSIMessageDialog(GetString(Resource.String.app_name), GetString(Resource.String.SettingChanged), DialogTypes.YesNoCancle, this.BaseActivity);
                    SignOutDialog.YesHandler += (sender, args) =>
                    {
                        bool bChangeTheme = false;
                        if (CSISystemContext.CSIWebServerName != Theme)
                        {
                            bChangeTheme = true;
                        }
                        SaveConfiguration();
                        if (Exit)
                        {
                            Dismiss();
                            Dispose();
                            if (bChangeTheme)
                            {
                                Intent intent = new Intent(Application.Context, typeof(MainActivity));
                                Bundle bundle = BaseActivity.GetCSISystemContext().BuildBundle();
                                intent.PutExtra("CSISystemContext", bundle);
                                BaseActivity.StartActivity(intent);
                                BaseActivity.Finish();
                            }
                        }
                    };
                    SignOutDialog.NoHandler += (sender, args) =>
                    {
                        CSISystemContext.CSIWebServerName = CSIWebServerName;
                        CSISystemContext.Theme = Theme;
                        CSISystemContext.SavedUser = SavedUser;
                        CSISystemContext.SavedPassword = SavedPassword;
                        CSISystemContext.EnableHTTPS = EnableHTTPS;
                        CSISystemContext.UseRESTForRequest = UseRESTForRequest;
                        CSISystemContext.LoadPicture = LoadPicture;
                        CSISystemContext.ForceAutoPost = ForceAutoPost;
                        CSISystemContext.ShowSuccessMessage = ShowSuccessMessage;
                        CSISystemContext.RecordCap = RecordCap;
                        CSISystemContext.SaveUser = SaveUser;
                        CSISystemContext.SavePassword = SavePassword;
                        if (Exit)
                        {
                            Dismiss();
                            Dispose();
                        }
                    };
                    SignOutDialog.CancelHandler += (sender, args) =>
                    {
                    };
                    SignOutDialog.Show(ft, "");
                }
                else
                {
                    Dismiss();
                    Dispose();
                }
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return false;
            }
            return true;
        }

        private void SaveConfiguration()
        {
            CSISystemContext.CSIWebServerName = CSIWebServerEdit.Text;
            CSISystemContext.Configuration = (string)ConfigurationSpinner.SelectedItem;
            CSISystemContext.Theme = InterpretThemeValue((string)ThemeSpinner.SelectedItem);
            CSISystemContext.SavedUser = UserEdit.Text;
            CSISystemContext.SavedPassword = PasswordEdit.Text;
            CSISystemContext.EnableHTTPS = EnableHTTPSSwitch.Checked;
            CSISystemContext.UseRESTForRequest = UseRESTForRequestSwitch.Checked;
            CSISystemContext.LoadPicture = LoadPictureSwitch.Checked;
            CSISystemContext.ForceAutoPost = ForceAutoPostSwitch.Checked;
            CSISystemContext.ShowSuccessMessage = ShowSuccessMessageSwitch.Checked;
            CSISystemContext.RecordCap = RecordCapEdit.Text;
            CSISystemContext.SaveUser = SaveUserSwitch.Checked;
            CSISystemContext.SavePassword = SavePasswordSwitch.Checked;

            CSISystemContext.WriteConfigurations();
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
                if (ProcessCount == 0)
                {
                    ProgressBar.Visibility = ViewStates.Invisible;
                    CSIBaseObject.DisableEnableControls(true, Layout);
                }
            }
            CloseImage.Visibility = HasTitle ? ViewStates.Gone : ViewStates.Visible;
            UserEdit.Enabled = SaveUserSwitch.Enabled && SaveUserSwitch.Checked;
            PasswordEdit.Enabled = SavePasswordSwitch.Enabled && SavePasswordSwitch.Checked;
        }
    }
}