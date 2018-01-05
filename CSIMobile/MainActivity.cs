using Android.App;
using Android.OS;
using Android.Util;
using Android.Widget;
using CSIMobile;
using CSIMobile.Class.Common;
using CSIMobile.Class.Fragments;
using System;
using System.Threading.Tasks;

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
                CSISystemContext.Activity = GetType().ToString();

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
                //Remove fragment else it will crash as it is already added to backstack
                Fragment prev = FragmentManager.FindFragmentByTag("dialog");
                if (prev != null)
                {
                    ft.Remove(prev);
                }

                ft.AddToBackStack(null);
                // Create and show the dialog.
                SignInDialogFragment newFragment = new SignInDialogFragment(this);
                //Add fragment
                newFragment.Show(ft, "");
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
        }
    }
}