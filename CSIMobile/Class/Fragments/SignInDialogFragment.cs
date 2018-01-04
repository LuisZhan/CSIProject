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
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                var view = inflater.Inflate(Resource.Layout.CSISignIn, container, false);

                //not allow to cancel sign in process...
                Cancelable = false;

                // Set up a handler to dismiss this DialogFragment when this button is clicked.
                view.FindViewById<Button>(Resource.Id.SignInButton).Click += (sender, args) => Dismiss();
                return view;
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
        }

    }
}