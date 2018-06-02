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

namespace CSIMobile.Class.Common
{
   public class CSIMessageDialog : CSIBaseDialogFragment
    {
        public enum DialogTypes {OKCancel, OK, YesNo, YesNoCancle}
        private string Title { get; set; }
        private string Message { get; set; }

        public EventHandler<DialogClickEventArgs> OkHandler;
        public EventHandler<DialogClickEventArgs> CancelHandler;
        public EventHandler<DialogClickEventArgs> YesHandler;
        public EventHandler<DialogClickEventArgs> NoHandler;
        public EventHandler<DialogClickEventArgs> CustomeHandler;
        private DialogTypes DefaultType = DialogTypes.OKCancel;

        public CSIMessageDialog(string DialogTitle, String DialogMessage, DialogTypes DialogType = DialogTypes.OKCancel, CSIBaseActivity activity = null) : base(activity)
        {
            Title = DialogTitle;
            Message = DialogMessage;
            DefaultType = DialogType;
        }

        public CSIMessageDialog(int TitleResourceID, int MessageTitleResourceID, DialogTypes DialogType = DialogTypes.OKCancel)
        {
            Title = GetString(TitleResourceID);
            Message = GetString(MessageTitleResourceID);
            DefaultType = DialogType;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            AlertDialog.Builder builder = BuildMessage();
            return builder.Create();
        }

        private AlertDialog.Builder BuildMessage()
        {
            AlertDialog.Builder Builder = new AlertDialog.Builder(Activity)
                .SetMessage(Message)
                .SetTitle(Title);
            switch (DefaultType)
            {
                case DialogTypes.YesNo:
                    Builder.SetPositiveButton(GetString(Android.Resource.String.Yes), YesHandler);
                    Builder.SetNegativeButton(GetString(Android.Resource.String.No), NoHandler);
                    break;
                case DialogTypes.YesNoCancle:
                    Builder.SetPositiveButton(GetString(Resource.String.Yes), YesHandler);
                    Builder.SetNegativeButton(GetString(Resource.String.No), NoHandler);
                    Builder.SetNeutralButton(GetString(Android.Resource.String.Cancel), CancelHandler);
                    break;
                case DialogTypes.OKCancel:
                    Builder.SetPositiveButton(GetString(Android.Resource.String.Ok), OkHandler);
                    Builder.SetNegativeButton(GetString(Android.Resource.String.Cancel), CancelHandler);
                    break;
                case DialogTypes.OK:
                default:
                    Builder.SetPositiveButton(GetString(Android.Resource.String.Ok), OkHandler);
                    break;
            }
            return Builder;
        }
    }
}