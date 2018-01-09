using Android.App;
using Android.OS;
using Android.Widget;
using CSIMobile.Class.Common;
using CSIMobile.Class.Fragments;
using System;
using System.Threading.Tasks;
using Android.Runtime;
using Android.Views;
using Android.Support.V4.View;
using CSIMobile.Class.Activities;
using CSIMobile.Class.Fragments.Adapter;
using System.Collections.Generic;
using static CSIMobile.Class.Common.CSIMessageDialog;

namespace CSIMobile
{
    [Activity(Label = "@string/app_name")]
    public class MainActivity : CSIBaseActivity
    {
        public TextView[] MoudleButton = { null, null, null, null };
        ModuleDeck Modules;
        ModuleDeckAdapter DeckAdapter;
        ViewPager ModulePage;

        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                base.OnCreate(bundle);

                if (CSISystemContext == null)
                {
                    CSISystemContext = new CSIContext();
                }

                SetContentView(Resource.Layout.CSIMain);

                ModulePage = FindViewById<ViewPager>(Resource.Id.ModulePage);

                // Instantiate the deck of flash cards:
                Modules = new ModuleDeck(CSISystemContext);

                // Instantiate the adapter and pass in the deck of flash cards:
                DeckAdapter = new ModuleDeckAdapter(SupportFragmentManager, Modules, this);

                // Find the ViewPager and plug in the adapter:
                ModulePage.Adapter = DeckAdapter;
                ModulePage.PageSelected += (o, e) => { GetModuleDeck(); };

                MoudleButton[0] = FindViewById<TextView>(Resource.Id.MoudleButton1);
                MoudleButton[1] = FindViewById<TextView>(Resource.Id.MoudleButton2);
                MoudleButton[2] = FindViewById<TextView>(Resource.Id.MoudleButton3);
                MoudleButton[3] = FindViewById<TextView>(Resource.Id.MoudleButton4);
                MoudleButton[0].Click += (o, e) => { SetModuleDeck(0); };
                MoudleButton[1].Click += (o, e) => { SetModuleDeck(1); };
                MoudleButton[2].Click += (o, e) => { SetModuleDeck(2); };
                MoudleButton[3].Click += (o, e) => { SetModuleDeck(3); };
                GetModuleDeck();

                //Show SignIn
                Task startupWork = new Task(() => { ShowSignInDialog(); });
                startupWork.Start();
            }
            catch (Exception Ex){
                WriteErrorLog(Ex);
            }
        }

        private void ShowSettingsDialog()
        {
            try
            {
                FragmentTransaction ft = FragmentManager.BeginTransaction();

                SettingsDialogFragment SettingsDialog = (SettingsDialogFragment)FragmentManager.FindFragmentByTag("Settings");
                if (SettingsDialog != null)
                {
                    ft.Show(SettingsDialog);
                    //ft.AddToBackStack(null);
                }
                else
                {
                    // Create and show the dialog.
                    SettingsDialog = new SettingsDialogFragment(this);
                    //Add fragment
                    SettingsDialog.Show(ft, "Settings");
                }
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
        }

        private void ShowSignInDialog()
        {
            try
            {
                FragmentTransaction ft = FragmentManager.BeginTransaction();

                SignInDialogFragment SignInDialog = (SignInDialogFragment)FragmentManager.FindFragmentByTag("SignIn");
                if (SignInDialog != null)
                {
                    ft.Show(SignInDialog);
                    //ft.AddToBackStack(null);
                }
                else
                {
                    // Create and show the dialog.
                    SignInDialog = new SignInDialogFragment(this);
                    //Add fragment
                    SignInDialog.Show(ft, "SignIn");
                }
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back && e.Action == KeyEventActions.Down)
            {
                FragmentTransaction ft = FragmentManager.BeginTransaction();

                CSIMessageDialog SignOutDialog = (CSIMessageDialog)FragmentManager.FindFragmentByTag("Exit");

                if (SignOutDialog != null)
                {
                    ft.Show(SignOutDialog);
                }
                else
                {
                    SignOutDialog = new CSIMessageDialog(GetString(Resource.String.app_name), string.Format(GetString(Resource.String.AskForExit), CSISystemContext.UserName), DialogTypes.OKCancel, this);
                    SignOutDialog.OkHandler += (sender, args) =>
                    {
                        Finish();
                    };
                    SignOutDialog.Show(ft, "Exit");
                }
                return true;
            }
            return base.OnKeyDown(keyCode, e);
        }

        public void GetModuleDeck()
        {
            for(int i= 0; i< MoudleButton.Length; i++)
            {
                if (i == ModulePage.CurrentItem)
                {
                    MoudleButton[i].SetCompoundDrawablesWithIntrinsicBounds(null, GetDrawable(Android.Resource.Drawable.ButtonOnoffIndicatorOn), null, null);
                }
                else
                {
                    MoudleButton[i].SetCompoundDrawablesWithIntrinsicBounds(null, GetDrawable(Android.Resource.Drawable.ButtonOnoffIndicatorOff), null, null);
                }
            }
        }

        public void SetModuleDeck(int Position)
        {
            ModulePage.SetCurrentItem(Position, true);
            GetModuleDeck();
        }

        public override bool InvokeCommand(string Command, Dictionary<string, object> ParmList = null)
        {
            bool Success = false;
            switch (Command)
            {
                case "CreateToken":
                    object User, Password, SaveUser, SavePassword, Configuration;
                    CSISystemContext.Token = "";

                    if (ParmList.TryGetValue("User", out User))
                        CSISystemContext.User = (string)User;
                    if (ParmList.TryGetValue("Password", out Password))
                        CSISystemContext.Password = (string)Password;
                    if (ParmList.TryGetValue("SaveUser", out SaveUser))
                        CSISystemContext.SaveUser = (bool)SaveUser;
                    if (ParmList.TryGetValue("SavePassword", out SavePassword))
                        CSISystemContext.SavePassword = (bool)SavePassword;
                    if (ParmList.TryGetValue("Configuration", out Configuration))
                        CSISystemContext.Configuration = (string)Configuration;

                    CSISystemContext.Token = CSIBaseInvoker.CreateToken(CSISystemContext);
                    if (string.IsNullOrEmpty(CSISystemContext.Token))
                        Success = false;
                    else
                        Success = true;
                    break;
                case "GetToken":
                    if (string.IsNullOrEmpty(CSISystemContext.Token))
                    {
                        ShowSignInDialog();
                        Success = false;
                    }
                    else
                    {
                        Success = true;
                    }
                    break;
                case "ShowSignIn":
                    if (string.IsNullOrEmpty(CSISystemContext.Token))
                        ShowSignInDialog();
                    else
                    {
                        FragmentTransaction ft = FragmentManager.BeginTransaction();
                        CSIMessageDialog AlertDialog = new CSIMessageDialog(GetString(Resource.String.app_name), string.Format(GetString(Resource.String.AlreadySignIn), CSISystemContext.UserName), DialogTypes.OKCancel, this);
                        AlertDialog.OkHandler += (sender, args) =>
                        {
                            CSISystemContext.Token = "";
                            ShowSignInDialog();
                        };
                        AlertDialog.Show(ft, "Exit");
                    }
                    Success = true;
                    break;
                case "ShowSettings":
                    ShowSettingsDialog();
                    Success = true;
                    break;
                default:
                    break;
            }
            return Success;
        }
    }
}