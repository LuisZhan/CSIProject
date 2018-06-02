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
        //Key Information
        protected string Key = string.Empty;
        protected string LineSuffix = string.Empty;
        protected string Release = string.Empty;
        protected string KeyLabel = string.Empty;
        protected string LineSuffixLabel = string.Empty;
        protected string ReleaseLabel = string.Empty;

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
            Dialog.SetCanceledOnTouchOutside(false);
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

        public void ShowProcessedMessage(EventHandler<DialogClickEventArgs> OkHandler = null)
        {
            string msg = string.Empty;
            if (string.IsNullOrEmpty(BuildKeyString()))
            {
                msg = CSISystemContext.ForceAutoPost ? GetString(Resource.String.Posted) : GetString(Resource.String.Processed);
            }
            else
            {
                msg = string.Format(CSISystemContext.ForceAutoPost ? GetString(Resource.String.Posted) : GetString(Resource.String.Processed1), BuildKeyString());
            }

            FragmentTransaction ft = FragmentManager.BeginTransaction();
            CSIMessageDialog AlertDialog = new CSIMessageDialog(GetString(Resource.String.app_name), msg, DialogTypes.OK, this.BaseActivity);
            AlertDialog.OkHandler += OkHandler;
            AlertDialog.Show(ft, "");
        }

        public void SetKeyValues(string keyLabel, string key, string linesuffixLabel = null, string linesuffix = null, string releaseLabel = null, string release = null)
        {
            KeyLabel = keyLabel ?? string.Empty;
            Key = key ?? string.Empty;
            LineSuffixLabel = linesuffixLabel ?? string.Empty;
            LineSuffix = linesuffix ?? string.Empty;
            ReleaseLabel = releaseLabel ?? string.Empty;
            Release = release ?? string.Empty;
        }

        private string BuildKeyString()
        {
            string msg = "";
            if (!string.IsNullOrEmpty(KeyLabel))
            {
                msg = msg + string.Format("{0}:[{1}] ", KeyLabel, ConvertEmptyString(Key));
                if (!string.IsNullOrEmpty(LineSuffixLabel))
                {
                    msg = msg + string.Format("{0}:[{1}] ", LineSuffixLabel, ConvertEmptyString(LineSuffix));
                    if (!string.IsNullOrEmpty(ReleaseLabel))
                    {
                        msg = msg + string.Format("{0}:[{1}] ", ReleaseLabel, ConvertEmptyString(Release));
                    }
                }
            }
            return msg;
        }

        private string ConvertEmptyString(string value)
        {
            return string.IsNullOrEmpty(value) ? string.Format("<{0}>",GetString(Resource.String.Blank)) : value;
        }

        protected void ShowMessage(string Msg, EventHandler<DialogClickEventArgs> OkHandler)
        {
            FragmentTransaction ft = FragmentManager.BeginTransaction();
            CSIMessageDialog AlertDialog = new CSIMessageDialog(GetString(Resource.String.app_name), Msg, DialogTypes.OK, this.BaseActivity);
            AlertDialog.OkHandler += OkHandler;
            AlertDialog.Show(ft, "");
        }

        protected virtual void Dialog_KeyPress(object sender, DialogKeyEventArgs e)
        {
        }
    }
}