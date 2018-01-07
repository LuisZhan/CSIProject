using Android.App;
using Android.OS;
using Android.Util;
using Android.Widget;
using CSIMobile;
using CSIMobile.Class.Common;
using CSIMobile.Class.Fragments;
using System;
using System.Threading.Tasks;
using Android.Runtime;
using Android.Views;

namespace CSIMobile
{
    [Activity(Label = "@string/app_name")]
    public class MainActivity : CSIBaseActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                base.OnCreate(bundle);

                SetContentView(Resource.Layout.Main);

                Task startupWork = new Task(() => { ShowSignInDialog(); });
                startupWork.Start();
            }
            catch (Exception Ex){
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
                    SignInDialog = new SignInDialogFragment
                    {
                        //Cancelable = false
                    };
                    //Add fragment
                    SignInDialog.Show(ft, "SignIn");
                }
                
            }catch (Exception Ex)
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
                    SignOutDialog = new CSIMessageDialog(GetString(Resource.String.app_name), string.Format(GetString(Resource.String.AskForExit), CSISystemContext.UserName));
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
    }
}