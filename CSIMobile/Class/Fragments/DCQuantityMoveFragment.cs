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
using CSIMobile.Class.Business;
using System.Data;
using System.IO;
using Android.Util;

namespace CSIMobile.Class.Fragments
{
    public class DCQuantityMoveFragment : CSIBaseFullScreenDialogFragment
    {
        CSIDcmoves SLDcmoves;

        ImageButton ItemScanButton;
        ImageButton UMScanButton;
        ImageButton QtyScanButton;
        ImageButton FromLocScanButton;
        ImageButton FromLotScanButton;
        ImageButton ToLocScanButton;
        ImageButton ToLotScanButton;
        EditText ItemEdit;
        EditText UMEdit;
        EditText QtyEdit;
        EditText FromLocEdit;
        EditText FromLotEdit;
        EditText ToLocEdit;
        EditText ToLotEdit;
        TextView ItemDescText;
        TextView ItemUMText;
        TextView OnHandQuantityText; 
        TextView FromLocDescText;
        TextView ToLocDescText;
        LinearLayout QtyLinearLayout;
        LinearLayout FromLinearLayout;
        LinearLayout ToLinearLayout; 
        LinearLayout FromLotLinearLayout;
        LinearLayout ToLotLinearLayout;
        Button SNButton;
        Button ProcessButton;

        ImageView CloseImage;
        ProgressBar ProgressBar;
        LinearLayout Layout;

        bool LotTracked = false, SNTracked = false;

        bool ItemValidated = false, UMValidated = false, QtyValidated = false, FromLocValidated = false, FromLotValidated = false, ToLocValidated = false, ToLotValidated = false;
        List<string> SNs = new List<string>();
        bool SNPicked = false;

        public DCQuantityMoveFragment(CSIBaseActivity activity = null) : base(activity)
        {
            CSISystemContext.ReadConfigurations();
            SLDcmoves = new CSIDcmoves(CSISystemContext);
            SLDcmoves.AddProperty("TransNum");
            SLDcmoves.AddProperty("TransType");
            SLDcmoves.AddProperty("Stat");
            SLDcmoves.AddProperty("Termid");
            SLDcmoves.AddProperty("TransDate");
            SLDcmoves.AddProperty("Whse");
            SLDcmoves.AddProperty("EmpNum");
            SLDcmoves.AddProperty("Item");
            SLDcmoves.AddProperty("UM");
            SLDcmoves.AddProperty("QtyMoved");
            SLDcmoves.AddProperty("Loc1");
            SLDcmoves.AddProperty("Lot1");
            SLDcmoves.AddProperty("Loc2");
            SLDcmoves.AddProperty("Lot2");
            SLDcmoves.SaveDataSetCompleted += SLDcmoves_SaveDataSetCompleted;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                var view = inflater.Inflate(Resource.Layout.CSIQuantityMove, container, false);
                Cancelable = false;
                

                ItemScanButton = view.FindViewById<ImageButton>(Resource.Id.ItemScanButton);
                ItemEdit = view.FindViewById<EditText>(Resource.Id.ItemEdit);
                UMScanButton = view.FindViewById<ImageButton>(Resource.Id.UMScanButton);
                UMEdit = view.FindViewById<EditText>(Resource.Id.UMEdit);
                QtyScanButton = view.FindViewById<ImageButton>(Resource.Id.QtyScanButton);
                QtyEdit = view.FindViewById<EditText>(Resource.Id.QtyEdit);
                FromLocScanButton = view.FindViewById<ImageButton>(Resource.Id.FromLocScanButton);
                FromLocEdit = view.FindViewById<EditText>(Resource.Id.FromLocEdit);
                FromLotScanButton = view.FindViewById<ImageButton>(Resource.Id.FromLotScanButton);
                FromLotEdit = view.FindViewById<EditText>(Resource.Id.FromLotEdit);
                ToLocScanButton = view.FindViewById<ImageButton>(Resource.Id.ToLocScanButton);
                ToLocEdit = view.FindViewById<EditText>(Resource.Id.ToLocEdit);
                ToLotScanButton = view.FindViewById<ImageButton>(Resource.Id.ToLotScanButton);
                ToLotEdit = view.FindViewById<EditText>(Resource.Id.ToLotEdit);

                QtyLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.QtyLinearLayout);
                FromLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.FromLinearLayout);
                ToLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.ToLinearLayout);
                FromLotLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.FromLotLinearLayout);
                ToLotLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.ToLotLinearLayout);

                SNButton = view.FindViewById<Button>(Resource.Id.SNButton);
                ProcessButton = view.FindViewById<Button>(Resource.Id.ProcessButton);
                Layout = view.FindViewById<LinearLayout>(Resource.Id.LinearLayout);

                ItemDescText = view.FindViewById<TextView>(Resource.Id.ItemDescText);
                ItemUMText = view.FindViewById<TextView>(Resource.Id.ItemUMText);
                OnHandQuantityText = view.FindViewById<TextView>(Resource.Id.OnHandQuantityText);
                FromLocDescText = view.FindViewById<TextView>(Resource.Id.FromLocDescText);
                ToLocDescText = view.FindViewById<TextView>(Resource.Id.ToLocDescText);

                CloseImage = view.FindViewById<ImageView>(Resource.Id.CloseImage);
                ProgressBar = view.FindViewById<ProgressBar>(Resource.Id.ProgressBar);

                ItemScanButton.Click += ItemScanButton_Click;
                UMScanButton.Click += UMScanButton_Click;
                QtyScanButton.Click += QtyScanButton_Click;
                FromLocScanButton.Click += FromLocScanButton_Click;
                FromLotScanButton.Click += FromLotScanButton_Click;
                ToLocScanButton.Click += ToLocScanButton_Click;
                ToLotScanButton.Click += ToLotScanButton_Click;

                ItemEdit.FocusChange += ItemEdit_FocusChange;
                UMEdit.FocusChange += UMEdit_FocusChange;
                QtyEdit.FocusChange += QtyEdit_FocusChange;
                FromLocEdit.FocusChange += FromLocEdit_FocusChange;
                FromLotEdit.FocusChange += FromLotEdit_FocusChange;
                ToLocEdit.FocusChange += ToLocEdit_FocusChange;
                ToLotEdit.FocusChange += ToLotEdit_FocusChange;

                SNButton.Click += SNButton_Click;
                ProcessButton.Click += ProcessButton_Click;

                CloseImage.Click += (sender, args) =>
                {
                    Dismiss();
                    Dispose();
                };

                Initialize();
                EnableDisableComponents();

                return view;
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return null;
            }
        }

        private void Initialize()
        {
            ItemEdit.Text = string.Empty;
            UMEdit.Text = string.Empty;
            QtyEdit.Text = "0";
            FromLocEdit.Text = string.Empty;
            FromLotEdit.Text = string.Empty;
            ToLocEdit.Text = string.Empty;
            ToLotEdit.Text = string.Empty;
            ItemDescText.Text = string.Empty;
            ItemUMText.Text = string.Empty;
            OnHandQuantityText.Text = string.Empty;
            FromLocDescText.Text = string.Empty;
            ToLocDescText.Text = string.Empty;
            SetSNLabel();
            ShowProgressBar(false);
        }

        private void SLDcmoves_SaveDataSetCompleted(object sender, SaveDataSetCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Initialize();
            }
            else
            {
                WriteErrorLog(e.Error);
            }
        }

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            if (ItemValidated && UMValidated && QtyValidated && FromLocValidated && FromLotValidated && ToLocValidated && ToLotValidated && SNPicked)
            {
                SLDcmoves.CurrentTable.Rows.Clear();
                DataRow Row = SLDcmoves.CurrentTable.NewRow();
                Row.BeginEdit();
                Row.ItemArray[0] = "";//TransNum
                Row.ItemArray[1] = "";//TransType
                Row.ItemArray[2] = "";//Stat
                Row.ItemArray[3] = DateTime.Now;//TransDate
                Row.ItemArray[4] = "";//Termid
                Row.ItemArray[5] = CSISystemContext.DefaultWarehouse;//Whse
                Row.ItemArray[6] = CSISystemContext.EmpNum;//EmpNum
                Row.ItemArray[7] = ItemEdit.Text;//Item
                Row.ItemArray[8] = UMEdit.Text;//UM
                Row.ItemArray[9] = QtyEdit.Text;//QtyMoved
                Row.ItemArray[10] = FromLocEdit.Text;//Loc1
                Row.ItemArray[11] = FromLotEdit.Text;//Lot1
                Row.ItemArray[12] = ToLocEdit.Text;//Loc2
                Row.ItemArray[13] = ToLotEdit.Text;//Lot2
                Row.EndEdit();
                Row.AcceptChanges();
                SLDcmoves.InsertIDO();
            }
        }

        private void SNButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ToLotEdit_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {//gain focus

            }
            else
            {//lose focus
                ValidateToLot();
            }
        }

        private void ToLocEdit_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {//gain focus

            }
            else
            {//lose focus
                ValidateToLoc();
            }
        }

        private void FromLotEdit_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {//gain focus

            }
            else
            {//lose focus
                ValidateFromLot();
            }
        }

        private void FromLocEdit_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {//gain focus

            }
            else
            {//lose focus
                ValidateFromLoc ();
            }
        }

        private void QtyEdit_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {//gain focus

            }
            else
            {//lose focus
                ValidateQty();
            }
        }

        private void UMEdit_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {//gain focus

            }
            else
            {//lose focus
                ValidateUM();
            }
        }

        private void ItemEdit_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {//gain focus

            }
            else
            {//lose focus
                ValidateItem();
            }
        }

        private bool ValidateItem()
        {
            if (!ItemValidated)
            {
                ItemValidated = true;
            }
            return ItemValidated;
        }

        private bool ValidateUM()
        {
            if (!UMValidated)
            {
                UMValidated = true;
            }
            return UMValidated;
        }

        private bool ValidateQty()
        {
            if (!QtyValidated)
            {
                QtyValidated = true;
            }
            return QtyValidated;
        }

        private bool ValidateFromLoc()
        {
            if (!FromLocValidated)
            {
                FromLocValidated = true;
            }
            return FromLocValidated;
        }

        private bool ValidateFromLot()
        {
            if (!FromLotValidated)
            {
                FromLotValidated = true;
            }
            return FromLotValidated;
        }

        private bool ValidateToLoc()
        {
            if (!ToLocValidated)
            {
                ToLocValidated = true;
            }
            return ToLocValidated;
        }

        private bool ValidateToLot()
        {
            if (!ToLotValidated)
            {
                ToLotValidated = true;
            }
            return ToLotValidated;
        }

        private void EnableDisableComponents()
        {
            if (string.IsNullOrEmpty(ItemEdit.Text))
            {
                QtyLinearLayout.Visibility = ViewStates.Gone;
                FromLinearLayout.Visibility = ViewStates.Gone;
                ToLinearLayout.Visibility = ViewStates.Gone;
            }
            else
            {
                QtyLinearLayout.Visibility = ViewStates.Visible;
                FromLinearLayout.Visibility = ViewStates.Visible;
                ToLinearLayout.Visibility = ViewStates.Visible;
            }
            FromLotLinearLayout.Visibility = LotTracked ? ViewStates.Visible : ViewStates.Gone;
            ToLotLinearLayout.Visibility = LotTracked ? ViewStates.Visible : ViewStates.Gone;
            FromLotScanButton.Enabled = LotTracked;
            ToLotScanButton.Enabled = LotTracked;
            SNButton.Visibility = SNTracked ? ViewStates.Visible : ViewStates.Gone;
            SNButton.Enabled = SNTracked;
            ProcessButton.Enabled = ItemValidated && UMValidated && QtyValidated && FromLocValidated && FromLotValidated && ToLocValidated && ToLotValidated && SNPicked;
        }

        private async void ToLotScanButton_Click(object sender, EventArgs e)
        {
            string ScanResult = await CSISanner.ScanAsync();
            if (string.IsNullOrEmpty(ScanResult))
            {
                return;
            }
            if (!AnalysisScanResult(ScanResult))
            {
                ToLotEdit.Text = ScanResult;
                if (!ValidateToLot())
                {
                    ToLotEdit.RequestFocus();
                }
                else
                {
                    if (SNTracked)
                    {
                        SNButton.RequestFocus();
                    }
                    else
                    {
                        ProcessButton.RequestFocus();
                    }
                }
                EnableDisableComponents();
            }
        }

        private async void ToLocScanButton_Click(object sender, EventArgs e)
        {
            string ScanResult = await CSISanner.ScanAsync();
            if (string.IsNullOrEmpty(ScanResult))
            {
                return;
            }
            if (!AnalysisScanResult(ScanResult))
            {
                ToLocEdit.Text = ScanResult;
                if (!ValidateToLoc())
                {
                    ToLocEdit.RequestFocus();
                }
                else
                {
                    if (LotTracked)
                    {
                        ToLotEdit.RequestFocus();
                    }
                    else
                    {
                        if (SNTracked)
                        {
                            SNButton.RequestFocus();
                        }
                        else
                        {
                            ProcessButton.RequestFocus();
                        }
                    }
                }
                EnableDisableComponents();
            }
        }

        private async void FromLotScanButton_Click(object sender, EventArgs e)
        {
            string ScanResult = await CSISanner.ScanAsync();
            if (string.IsNullOrEmpty(ScanResult))
            {
                return;
            }
            if (!AnalysisScanResult(ScanResult))
            {
                FromLotEdit.Text = ScanResult;
                if (!ValidateFromLot())
                {
                    FromLotEdit.RequestFocus();
                }
                else
                {
                    ToLocEdit.RequestFocus();
                }
                EnableDisableComponents();
            }
        }

        private async void FromLocScanButton_Click(object sender, EventArgs e)
        {
            string ScanResult = await CSISanner.ScanAsync();
            if (string.IsNullOrEmpty(ScanResult))
            {
                return;
            }
            if (!AnalysisScanResult(ScanResult))
            {
                FromLocEdit.Text = ScanResult;
                if (!ValidateFromLoc())
                {
                    FromLocEdit.RequestFocus();
                }
                else
                {
                    if (LotTracked)
                    {
                        FromLotEdit.RequestFocus();
                    }
                    else
                    {
                        ToLocEdit.RequestFocus();
                    }
                }
                EnableDisableComponents();
            }
        }

        private async void QtyScanButton_Click(object sender, EventArgs e)
        {
            string ScanResult = await CSISanner.ScanAsync();
            if (string.IsNullOrEmpty(ScanResult))
            {
                return;
            }
            if (!AnalysisScanResult(ScanResult))
            {
                QtyEdit.Text = ScanResult;
                if (!ValidateQty())
                {
                    QtyEdit.RequestFocus();
                }
                else
                {
                    FromLocEdit.RequestFocus();
                }
                EnableDisableComponents();
            }
        }

        private async void UMScanButton_Click(object sender, EventArgs e)
        {
            string ScanResult = await CSISanner.ScanAsync();
            if (string.IsNullOrEmpty(ScanResult))
            {
                return;
            }
            if (!AnalysisScanResult(ScanResult))
            {
                UMEdit.Text = ScanResult;
                if (!ValidateUM())
                {
                    UMEdit.RequestFocus();
                }
                else
                {
                    QtyEdit.RequestFocus();
                }
                EnableDisableComponents();
            }
        }

        private async void ItemScanButton_Click(object sender, EventArgs e)
        {
            string ScanResult = await CSISanner.ScanAsync();
            if (string.IsNullOrEmpty(ScanResult))
            {
                return;
            }
            if (!AnalysisScanResult(ScanResult))
            {
                ItemEdit.Text = ScanResult;
                if (!ValidateItem())
                {
                    ItemEdit.RequestFocus();
                }
                else
                {
                    UMEdit.RequestFocus();
                }
                EnableDisableComponents();
            }
        }

        private bool AnalysisScanResult(string Result)
        {
            //this is designed for future scan enhancement, such as scan one code to fill in all stuff...
            bool rtn = CSIJsonObjects.ReadQtyMoveJson(Result, out string Item, out string UM, out string Qty, out string Loc1, out string Lot1, out string Loc2, out string Lot2);
            if (rtn)
            {
                ItemEdit.Text = Item;
                ValidateItem();
                UMEdit.Text = UM;
                ValidateUM();
                QtyEdit.Text = Qty;
                ValidateQty();
                FromLocEdit.Text = Loc1;
                ValidateFromLoc();
                FromLotEdit.Text = Lot1;
                ValidateFromLot();
                ToLocEdit.Text = Loc2;
                ValidateToLoc();
                ToLotEdit.Text = Lot2;
                ValidateToLot();
                EnableDisableComponents();
            }
            return rtn;
        }

        private bool PerformValidation()
        {
            return ValidateItem() && ValidateUM() && ValidateQty() && ValidateFromLoc() && ValidateFromLot() && ValidateToLoc() && ValidateToLot();
        }

        private void SetSNLabel()
        {
            SNButton.Text = string.Format(GetString(Resource.String.SNPicked), SNs.Count.ToString());
        }

        private void ShowProgressBar(bool show)
        {
            ProgressBar.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
            CSIBaseObject.DisableEnableControls(!show, Layout);
            
            EnableDisableComponents();
        }
    }
}