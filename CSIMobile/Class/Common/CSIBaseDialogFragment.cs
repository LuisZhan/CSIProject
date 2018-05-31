using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using static CSIMobile.Class.Common.CSIMessageDialog;

namespace CSIMobile.Class.Common
{
    public class CSIBaseDialogFragment : DialogFragment
    {
        protected CSIBaseActivity BaseActivity;
        protected CSIContext CSISystemContext;
        protected string Title = Application.Context.GetString(Resource.String.app_name);
        private bool hasTitle = true;
        private int processCount = 0;

        protected int ThemeId = Resource.Style.MyTheme_Dialog;
        protected int ThemeIdNoTitle = Resource.Style.MyTheme_Dialog_NoTitle;

        protected bool HasTitle
        {
            set
            {
                hasTitle = value;
                if (hasTitle)
                {
                    Cancelable = (processCount == 0);
                }
                else
                {
                    Cancelable = false;
                }
            }
            get
            {
                return hasTitle;
            }
        }

        protected int ProcessCount
        {
            set
            {
                processCount = value;
                if (HasTitle)
                {
                    Cancelable = (value == 0);
                }
                else
                {
                    Cancelable = false;
                }
            }
            get
            {
                return processCount;
            }
        }

        public CSIBaseDialogFragment(CSIBaseActivity activity = null)
        {
            if (activity == null)
            {
                CSISystemContext = new CSIContext();
            }
            else
            {
                BaseActivity = activity;
                if (BaseActivity.GetCSISystemContext() == null)
                {
                    CSISystemContext = new CSIContext();
                }
                else
                {
                    CSISystemContext = BaseActivity.GetCSISystemContext();
                }
            }
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            Dialog dialog = base.OnCreateDialog(savedInstanceState);
            return dialog;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            SetDialogStyle();
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            Dialog.SetTitle(Title);
            SetDialogStyle();
            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public virtual void SetBaseActivity(CSIBaseActivity activity)
        {
            if (activity == null)
            {
                CSISystemContext = new CSIContext();
            }
            else
            {
                BaseActivity = activity;
                if (BaseActivity.GetCSISystemContext() == null)
                {
                    CSISystemContext = new CSIContext();
                }
                else
                {
                    CSISystemContext = BaseActivity.GetCSISystemContext();
                }
            }
        }

        public CSIBaseActivity GetBaseActivity()
        {
            return BaseActivity;
        }

        protected virtual void WriteErrorLog(Exception Ex)
        {
            if (CSISystemContext.DisplayWhenError)
            {
                Toast.MakeText(Application.Context, Ex.Message, ToastLength.Long).Show();
            }
            CSIErrorLog.WriteErrorLog(Ex);
            ShowDialog(Ex);
        }

        protected virtual void WriteLog(string content)
        {
            CSIErrorLog.WriteLog(content);
        }

        protected virtual void WriteLog()
        {
            CSIErrorLog.WriteLog(CSISystemContext);
        }
        
        protected void SetDialogStyle()
        {
            if (!HasTitle)
                SetStyleNoTitle();
            else
                SetStyleNormal();
        }

        protected void SetStyleNoTitle()
        {
            SetStyle(DialogFragmentStyle.NoTitle, ThemeIdNoTitle);
        }

        protected void SetStyleNormal()
        {
            SetStyle(DialogFragmentStyle.Normal, ThemeId);
        }

        protected void SetStyleNoInput()
        {
            SetStyle(DialogFragmentStyle.NoInput, ThemeId);
        }

        private void ShowDialog(Exception Ex)
        {
            FragmentTransaction ft = FragmentManager.BeginTransaction();
            CSIMessageDialog Dialog = (CSIMessageDialog)FragmentManager.FindFragmentByTag("Dialog");

            if (Dialog != null)
            {
                ft.Show(Dialog);
            }
            else
            {
                Dialog = new CSIMessageDialog(Application.Context.GetString(Resource.String.app_name), CSIBaseInvoker.TranslateError(Ex), DialogTypes.OK);
                Dialog.Show(ft, "Dialog");
            }
        }
    }
}