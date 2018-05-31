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
    public class SerialGenerator : CSIBaseDialogFragment
    {

        ImageView CloseImage;
        ProgressBar ProgressBar;
        LinearLayout Layout;

        public CSIBaseDialogFragment ParentFragment { get; set; }
        List<string> SNs = new List<string>();

        public SerialGenerator(CSIBaseActivity activity = null) : base(activity)
        {
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                base.OnCreateView(inflater, container, savedInstanceState);

                var view = inflater.Inflate(Resource.Layout.CSISerialGenerator, container, false);
                //Cancelable = false;

                //CloseImage = view.FindViewById<ImageView>(Resource.Id.CloseImage);
                //ProgressBar = view.FindViewById<ProgressBar>(Resource.Id.ProgressBar);
                

                //CloseImage.Click += (sender, args) =>
                //{
                //    Dismiss();
                //    Dispose();
                //};

                //ShowProgressBar(false);
                //Initialize();
                //EnableDisableComponents();

                return view;
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return null;
            }
        }

        private void EnableDisableComponents()
        {
        }

        private void Initialize()
        {

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
                    ProgressBar.Visibility = ViewStates.Gone;
                    CSIBaseObject.DisableEnableControls(true, Layout);
                    EnableDisableComponents();
                }
            }

        }

        public static SerialGenerator RunFragment(CSIBaseDialogFragment BaseDialogFragment)
        {
            try
            {
                FragmentTransaction ft = BaseDialogFragment.FragmentManager.BeginTransaction();

                SerialGenerator SerialGeneratorDialog = (SerialGenerator)BaseDialogFragment.FragmentManager.FindFragmentByTag("SerialGenerator");
                if (SerialGeneratorDialog != null)
                {
                    ft.Show(SerialGeneratorDialog);
                    //ft.AddToBackStack(null);
                }
                else
                {
                    // Create and show the dialog.
                    SerialGeneratorDialog = new SerialGenerator(BaseDialogFragment.GetBaseActivity())
                    {
                        ParentFragment = BaseDialogFragment
                    };
                    //Add fragment
                    SerialGeneratorDialog.Show(ft, "SerialGenerator");
                }
                return SerialGeneratorDialog;
            }
            catch (Exception Ex)
            {
                CSIErrorLog.WriteErrorLog(Ex);
                return null;
            }
        }
    }
}