using Android.App;
using Android.OS;
using Android.Util;
using Android.Widget;
using CSIMobile;
using CSIMobile.Class.Common;
using CSIMobile.Class.Fragments;
using System;

namespace CSIMobile
{
    [Activity(Label = "@string/app_name")]
    public class MainActivity : CSIBaseActivity
    {
        static readonly string TAG = "X:" + typeof(MainActivity).Name;
        Button _button;
        //int _clickCount;

        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                base.OnCreate(bundle);
                SetContentView(Resource.Layout.Main);

                _button = FindViewById<Button>(Resource.Id.MyButton);
                _button.Click += (sender, args) =>
                {
                    //string message = string.Format("You clicked {0} times.", ++_clickCount);
                    //_button.Text = message;
                    //Log.Debug(TAG, message);
                    FragmentTransaction ft = FragmentManager.BeginTransaction();
                    //Remove fragment else it will crash as it is already added to backstack
                    Fragment prev = FragmentManager.FindFragmentByTag("dialog");
                    if (prev != null)
                    {
                        ft.Remove(prev);
                    }

                    ft.AddToBackStack(null);
                    // Create and show the dialog.
                    SignInDialogFragment newFragment = new SignInDialogFragment();
                    //Add fragment
                    newFragment.Show(ft, "");
                };
            }catch (Exception Ex){
                WriteErrorLog(Ex);
            }
            finally
            {
                Log.Debug(TAG, "MainActivity is loaded.");
            }
        }
    }
}