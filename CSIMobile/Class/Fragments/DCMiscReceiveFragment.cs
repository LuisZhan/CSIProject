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
using CSIMobile.Class.Business.IO;

namespace CSIMobile.Class.Fragments
{
    public class DCMiscReceiveFragment : CSIBaseDialogFragment
    {
        CSIDcitems SLDcitems;

        ImageButton ItemScanButton;
        ImageButton UMScanButton;
        ImageButton QtyScanButton;
        ImageButton LocScanButton;
        ImageButton LotScanButton;
        ImageButton ReasonScanButton;
        EditText WhseEdit;
        TextView TransDateText;
        EditText ItemEdit;
        EditText UMEdit;
        EditText QtyEdit;
        EditText LocEdit;
        EditText LotEdit;
        EditText ReasonEdit;
        TextView ItemDescText;
        TextView ItemUMText;
        TextView OnHandQuantityText;
        TextView LocDescText;
        TextView ReasonDescText;
        LinearLayout QtyLinearLayout;
        LinearLayout LotLinearLayout; 
        Button SNButton;
        Button ProcessButton;

        ImageView CloseImage;
        ProgressBar ProgressBar;
        LinearLayout Layout;

        bool LotTracked = false, SNTracked = false;

        bool ItemValidated = false, UMValidated = false, QtyValidated = false, LocValidated = false, LotValidated = false, ReasonValidated = false;
        List<string> SNs = new List<string>();
        bool SNPicked = true;

        private int ProcessCount = 0;

        public DCMiscReceiveFragment(CSIBaseActivity activity = null) : base(activity)
        {
            CSISystemContext.ReadConfigurations();
            SLDcitems = new CSIDcitems(CSISystemContext);
            SLDcitems.AddProperty("TransNum");
            SLDcitems.AddProperty("TransType");
            SLDcitems.AddProperty("Stat");
            SLDcitems.AddProperty("Termid");
            SLDcitems.AddProperty("TransDate");
            SLDcitems.AddProperty("Whse");
            SLDcitems.AddProperty("EmpNum");
            SLDcitems.AddProperty("Item");
            SLDcitems.AddProperty("UM");
            SLDcitems.AddProperty("CountQty");
            SLDcitems.AddProperty("Loc");
            SLDcitems.AddProperty("Lot");
            SLDcitems.AddProperty("ReasonCode");
            SLDcitems.AddProperty("DocumentNum");
            SLDcitems.AddProperty("ErrorMessage");

            SLDcitems.SetFilter("1=0");
            SLDcitems.LoadIDO();
            SLDcitems.SaveDataSetCompleted += SLDcitems_SaveDataSetCompleted;
            SLDcitems.LoadDataSetCompleted += SLDcitems_LoadDataSetCompleted;
            SLDcitems.CallMethodCompleted += SLDcitems_CallMethodCompleted;
        }

        private void SLDcitems_SaveDataSetCompleted(object sender, SaveDataSetCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    //check result status
                    if (SLDcitems.CurrentTable.Rows.Count <= 0)
                    {
                        //nothing happen or just delete rows
                    }
                    else
                    {
                        string RowStatus = SLDcitems.GetCurrentPropertyValueOfString("Stat");
                        string ErrorMessage = SLDcitems.GetCurrentPropertyValueOfString("ErrorMessage");

                        if ((RowStatus != "E") || string.IsNullOrEmpty(ErrorMessage))
                        {
                            //Ready to Post -- calling DcmatlPSp
                            ShowProgressBar(true);
                            string strParmeters = "";
                            strParmeters = CSIBaseInvoker.BuildXMLParameters(strParmeters, SLDcitems.GetCurrentPropertyValueOfString("TransNum"));
                            strParmeters = CSIBaseInvoker.BuildXMLParameters(strParmeters, "");
                            strParmeters = CSIBaseInvoker.BuildXMLParameters(strParmeters, "", true);
                            SLDcitems.InvokeMethod("DcmatlPSp", strParmeters);
                        }
                        else
                        {
                            //delete first before prompt message.
                            SLDcitems.CurrentTable.Rows[0].Delete();
                            ShowProgressBar(true);
                            SLDcitems.DeleteIDO();

                            //Populate Error
                            FragmentTransaction ft = FragmentManager.BeginTransaction();
                            CSIMessageDialog DeleteDialog = (CSIMessageDialog)FragmentManager.FindFragmentByTag("DeleteDialog");

                            if (DeleteDialog != null)
                            {
                                ft.Show(DeleteDialog);
                            }
                            else
                            {
                                DeleteDialog = new CSIMessageDialog(GetString(Resource.String.app_name), ErrorMessage, DialogTypes.OK);
                                DeleteDialog.Show(ft, "DeleteDialog");
                            }
                        }
                    }
                }
                else
                {
                    WriteErrorLog(e.Error);
                }
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
            ShowProgressBar(false);
        }

        private void SLDcitems_CallMethodCompleted(object sender, CallMethodCompletedEventArgs e)
        {
            try
            {
                //throw new NotImplementedException();
                if (e.Error == null)
                {
                    if (e.Result.ToString() == "0")
                    {
                        Initialize();
                    }
                    else
                    {
                        //get error - delete first.
                        SLDcitems.CurrentTable.Rows[0].Delete();
                        ShowProgressBar(true);
                        SLDcitems.DeleteIDO();
                        WriteErrorLog(new Exception(CSIBaseInvoker.GetXMLParameters(e.strMethodParameters,1)));
                    }
                }
                else
                {
                    //try to delete post
                    SLDcitems.CurrentTable.Rows[0].Delete();
                    ShowProgressBar(true);
                    SLDcitems.DeleteIDO();
                    WriteErrorLog(e.Error);
                }
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
            ShowProgressBar(false);
        }

        private void SLDcitems_LoadDataSetCompleted(object sender, LoadDataSetCompletedEventArgs e)
        {
            try
            {
                //throw new NotImplementedException();
                if (e.Error == null)
                {
                }
                else
                {
                }
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
            ShowProgressBar(false);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                var view = inflater.Inflate(Resource.Layout.CSIMiscIssueReceiptAndQtyAdjus, container, false);
                Cancelable = false;
                
                WhseEdit = view.FindViewById<EditText>(Resource.Id.WhseEdit);
                TransDateText = view.FindViewById<TextView>(Resource.Id.TransDateText);
                ItemScanButton = view.FindViewById<ImageButton>(Resource.Id.ItemScanButton);
                ItemEdit = view.FindViewById<EditText>(Resource.Id.ItemEdit);
                UMScanButton = view.FindViewById<ImageButton>(Resource.Id.UMScanButton);
                UMEdit = view.FindViewById<EditText>(Resource.Id.UMEdit);
                QtyScanButton = view.FindViewById<ImageButton>(Resource.Id.QtyScanButton);
                QtyEdit = view.FindViewById<EditText>(Resource.Id.QtyEdit);
                LocScanButton = view.FindViewById<ImageButton>(Resource.Id.LocScanButton);
                LocEdit = view.FindViewById<EditText>(Resource.Id.LocEdit);
                LotScanButton = view.FindViewById<ImageButton>(Resource.Id.LotScanButton);
                LotEdit = view.FindViewById<EditText>(Resource.Id.LotEdit);
                ReasonScanButton = view.FindViewById<ImageButton>(Resource.Id.ReasonScanButton);
                ReasonEdit = view.FindViewById<EditText>(Resource.Id.ReasonEdit);

                QtyLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.QtyLinearLayout);
                LotLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.LotLinearLayout);

                SNButton = view.FindViewById<Button>(Resource.Id.SNButton);
                ProcessButton = view.FindViewById<Button>(Resource.Id.ProcessButton);
                Layout = view.FindViewById<LinearLayout>(Resource.Id.LinearLayout);

                ItemDescText = view.FindViewById<TextView>(Resource.Id.ItemDescText);
                ItemUMText = view.FindViewById<TextView>(Resource.Id.ItemUMText);
                OnHandQuantityText = view.FindViewById<TextView>(Resource.Id.OnHandQuantityText);
                LocDescText = view.FindViewById<TextView>(Resource.Id.LocDescText);
                ReasonDescText = view.FindViewById<TextView>(Resource.Id.ReasonDescText);

                CloseImage = view.FindViewById<ImageView>(Resource.Id.CloseImage);
                ProgressBar = view.FindViewById<ProgressBar>(Resource.Id.ProgressBar);

                ItemScanButton.Click += ItemScanButton_Click;
                UMScanButton.Click += UMScanButton_Click;
                QtyScanButton.Click += QtyScanButton_Click;
                LocScanButton.Click += LocScanButton_Click;
                LotScanButton.Click += LotScanButton_Click;
                ReasonScanButton.Click += ReasonScanButton_Click;

                ItemEdit.FocusChange += ItemEdit_FocusChange;
                UMEdit.FocusChange += UMEdit_FocusChange;
                QtyEdit.FocusChange += QtyEdit_FocusChange;
                LocEdit.FocusChange += LocEdit_FocusChange;
                LotEdit.FocusChange += LotEdit_FocusChange;
                ReasonEdit.FocusChange += ReasonEdit_FocusChange;

                ItemEdit.KeyPress += ItemEdit_KeyPress;
                UMEdit.KeyPress += UMEdit_KeyPress;
                QtyEdit.KeyPress += QtyEdit_KeyPress;
                LocEdit.KeyPress += LocEdit_KeyPress;
                LotEdit.KeyPress += LotEdit_KeyPress;
                ReasonEdit.KeyPress += ReasonEdit_KeyPress;

                SNButton.Click += SNButton_Click;
                ProcessButton.Click += ProcessButton_Click;

                CloseImage.Click += (sender, args) =>
                {
                    Dismiss();
                    Dispose();
                };

                ShowProgressBar(false);
                Initialize();
                EnableDisableComponents();

                return view;
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return null;
            }
        }

        private void LotEdit_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Enter || e.KeyCode == Keycode.Tab)
            {
                if (e.Event.Action == KeyEventActions.Up)
                {
                    ValidateLot();
                    ReasonEdit.RequestFocus();
                }
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
                LotValidated = false;
            }
        }

        private void LocEdit_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Enter || e.KeyCode == Keycode.Tab)
            {
                if (e.Event.Action == KeyEventActions.Up)
                {
                    ValidateLoc();
                    if (LotTracked)
                    {
                        LotEdit.RequestFocus();
                    }
                    else
                    {
                        ReasonEdit.RequestFocus();
                    }
                }
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
                LocValidated = false;
            }

        }

        private void ReasonEdit_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Enter || e.KeyCode == Keycode.Tab)
            {
                if (e.Event.Action == KeyEventActions.Up)
                {
                    ValidateReason();
                    ProcessButton.RequestFocus();
                }
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
                ReasonValidated = false;
            }
        }

        private void QtyEdit_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Enter || e.KeyCode == Keycode.Tab)
            {
                if (e.Event.Action == KeyEventActions.Up)
                {
                    ValidateQty();
                    LocEdit.RequestFocus();
                }
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
                QtyValidated = false;
            }
        }

        private void UMEdit_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Enter || e.KeyCode == Keycode.Tab)
            {
                if (e.Event.Action == KeyEventActions.Up)
                {
                    ValidateUM();
                    QtyEdit.RequestFocus();
                }
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
                UMValidated = false;
            }
        }

        private void ItemEdit_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Enter || e.KeyCode == Keycode.Tab)
            {
                if (e.Event.Action == KeyEventActions.Up)
                {
                    ValidateItem();
                    UMEdit.RequestFocus();
                    //
                }
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
                ItemValidated = false;
            }
        }

        private void Initialize()
        {
            WhseEdit.Text = CSISystemContext.DefaultWarehouse;
            TransDateText.Text = string.Format("{0} {1}",DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
            ItemEdit.Text = string.Empty;
            UMEdit.Text = string.Empty;
            QtyEdit.Text = "0";
            LocEdit.Text = string.Empty;
            LotEdit.Text = string.Empty;
            ItemDescText.Text = string.Empty;
            ItemUMText.Text = string.Empty;
            OnHandQuantityText.Text = string.Empty;
            LocDescText.Text = string.Empty;
            ReasonEdit.Text = string.Empty;
            ReasonDescText.Text = string.Empty;
            ItemValidated = false;
            UMValidated = false;
            QtyValidated = false;
            LocValidated = false;
            LotValidated = false;
            ReasonValidated = false;
            SetSNLabel();
        }

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            PerformValidation();
            if (ItemValidated && UMValidated && QtyValidated && LocValidated && (LotValidated || !LotTracked) && ReasonValidated && SNPicked)
            {
                SLDcitems.CurrentTable.Rows.Clear();
                DataRow Row = SLDcitems.CurrentTable.NewRow();
                Row["TransNum"] = 0;//TransNum
                Row["TransType"] = "3";//TransType
                Row["Stat"] = "U";//Stat
                Row["Termid"] = CSISystemContext.AndroidId.Substring(CSISystemContext.AndroidId.Length - 4, 4);//Termid
                Row["TransDate"] = DateTime.Now;//TransDate
                Row["Whse"] = CSISystemContext.DefaultWarehouse;//Whse
                Row["EmpNum"] = CSISystemContext.EmpNum;//EmpNum
                Row["Item"] = ItemEdit.Text;//Item
                Row["UM"] = UMEdit.Text;//UM
                Row["CountQty"] = QtyEdit.Text;//CountQty
                Row["Loc"] = LocEdit.Text;//Loc
                Row["Lot"] = LotEdit.Text;//Lot
                Row["ReasonCode"] = ReasonEdit.Text;//ReasonCode
                SLDcitems.CurrentTable.Rows.Add(Row);
                //Row.BeginEdit();
                //Row.EndEdit();
                //Row.AcceptChanges();
                SLDcitems.InsertIDO();
                ShowProgressBar(true);
            }
        }

        private void SNButton_Click(object sender, EventArgs e)
        {
            SerialGenerator.RunFragment(this);
        }

        private void ReasonEdit_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {//gain focus
                ReasonEdit.SelectAll();
            }
            else
            {//lose focus
                ValidateReason();
            }
        }

        private void LotEdit_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {//gain focus
                LotEdit.SelectAll();
            }
            else
            {//lose focus
                ValidateLot();
            }
        }

        private void LocEdit_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {//gain focus
                LocEdit.SelectAll();
            }
            else
            {//lose focus
                ValidateLoc();
            }
        }

        private void QtyEdit_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {//gain focus
                QtyEdit.SelectAll();
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
                UMEdit.SelectAll();
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
                ItemEdit.SelectAll();
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
                if (string.IsNullOrEmpty(ItemEdit.Text))
                {
                    ItemValidated = false;
                }
                else
                {
                    try
                    {
                        string Item = ItemEdit.Text, Desc = ItemDescText.Text, UM = ItemUMText.Text, Qty = OnHandQuantityText.Text;
                        ItemValidated = CSIItems.GetItemInfor(CSISystemContext, ref Item, ref Desc, ref UM, ref Qty, ref LotTracked, ref SNTracked);
                        if (ItemValidated == true)
                        {
                            ItemEdit.Text = Item;
                            ItemDescText.Text = Desc;
                            ItemUMText.Text = UM;
                            if (string.IsNullOrEmpty(UMEdit.Text))
                            {
                                UMEdit.Text = UM;
                                UMValidated = true;
                            }
                            OnHandQuantityText.Text = Qty;
                        }
                        else
                        {
                            ItemDescText.Text = string.Empty;
                            ItemUMText.Text = string.Empty;
                            OnHandQuantityText.Text = "0";
                        }

                        string Loc = LocEdit.Text, LocType = "";
                        bool RtnCSIItemLocs = CSIItemLocs.GetItemLocInfor(CSISystemContext, ItemEdit.Text, WhseEdit.Text, ref Loc, ref LocType, ref Qty);
                        if (RtnCSIItemLocs == true)
                        {
                            LocEdit.Text = Loc;
                            LocValidated = false;
                            ValidateLoc();
                            //OnHandQuantityText.Text = Qty; //used for validate Qty
                        }
                    }
                    catch (Exception Ex)
                    {
                        WriteErrorLog(Ex);
                        ItemValidated = false;
                    }
                }
            }
            EnableDisableComponents();
            return ItemValidated;
        }

        private bool ValidateUM()
        {
            if (!UMValidated)
            {
                if (string.IsNullOrEmpty(UMEdit.Text))
                {
                    UMValidated = false;
                }
                else
                {
                    UMValidated = true;
                }
            }
            EnableDisableComponents();
            return UMValidated;
        }

        private bool ValidateQty()
        {
            if (!QtyValidated)
            {
                if (string.IsNullOrEmpty(QtyEdit.Text))
                {
                    QtyValidated = false;
                }
                else
                {
                    QtyValidated = true;
                }
            }
            EnableDisableComponents();
            return QtyValidated;
        }

        private bool ValidateLoc()
        {
            if (!LocValidated)
            {
                if (string.IsNullOrEmpty(LocEdit.Text))
                {
                    LocValidated = false;
                }
                else
                {
                    string Loc = LocEdit.Text, LocDescription = "", Lot = LotEdit.Text, Qty = "";
                    LocValidated = CSIItemLocs.GetItemLocInfor(CSISystemContext, ItemEdit.Text, WhseEdit.Text, ref Loc, ref LocDescription, ref Qty);
                    if (LocValidated)
                    {
                        LocEdit.Text = Loc;
                        LocDescText.Text = LocDescription;
                        bool RtnCSILotLocs = CSILotLocs.GetItemLotLocInfor(CSISystemContext, ItemEdit.Text, WhseEdit.Text, LocEdit.Text, ref Lot, ref Qty);
                        if (RtnCSILotLocs)
                        {
                            LotEdit.Text = Lot;
                            LotValidated = false;
                            ValidateLot();
                            //OnHandQuantityText.Text = Qty; //used for validate Qty
                        }
                    }
                    else
                    {
                        LocDescText.Text = string.Empty;
                    }
                }
            }
            EnableDisableComponents();
            return LocValidated;
        }

        private bool ValidateLot()
        {
            if (!LotValidated)
            {
                if (string.IsNullOrEmpty(LotEdit.Text))
                {
                    LotValidated = false;
                }
                else
                {
                    LotValidated = true;
                }
            }
            EnableDisableComponents();
            return LotValidated;
        }

        private bool ValidateReason()
        {
            if (!ReasonValidated)
            {
                if (string.IsNullOrEmpty(ReasonEdit.Text))
                {
                    ReasonValidated = false;
                }
                else
                {
                    string ReasonCode = ReasonEdit.Text, ReasonDescription = "";
                    ReasonValidated = CSIReasons.GetReason(CSISystemContext, ref ReasonCode, "MISC RCPT", ref ReasonDescription);
                    if (ReasonValidated)
                    {
                        ReasonEdit.Text = ReasonCode;
                        ReasonDescText.Text = ReasonDescription;
                    }
                    else
                    {
                        ReasonDescText.Text = string.Empty;
                    }
                }
            }
            EnableDisableComponents();
            return ReasonValidated;
        }

        private bool ValidateSN()
        {
            if (SNTracked)
            {
                SNPicked = SNs.Count == int.Parse(QtyEdit.Text);
            }
            else
            {
                SNPicked = true;
            }
            return SNPicked;
        }

        private void EnableDisableComponents()
        {
            if (string.IsNullOrEmpty(ItemEdit.Text))
            {
                //QtyLinearLayout.Visibility = ViewStates.Gone;
                //FromLinearLayout.Visibility = ViewStates.Gone;
                //ToLinearLayout.Visibility = ViewStates.Gone;
            }
            else
            {
                //QtyLinearLayout.Visibility = ViewStates.Visible;
                //FromLinearLayout.Visibility = ViewStates.Visible;
                //ToLinearLayout.Visibility = ViewStates.Visible;
            }
            LotLinearLayout.Visibility = LotTracked ? ViewStates.Visible : ViewStates.Gone;
            LotScanButton.Enabled = LotTracked;
            SNButton.Visibility = SNTracked ? ViewStates.Visible : ViewStates.Gone;
            SNButton.Enabled = SNTracked;
            ProcessButton.Enabled = ItemValidated && UMValidated && QtyValidated
                && LocValidated && ((LotTracked && LotValidated) || !LotTracked) 
                && ReasonValidated
                && ((SNTracked && SNPicked) || !SNTracked);
        }

        private async void LotScanButton_Click(object sender, EventArgs e)
        {
            string ScanResult = await CSISanner.ScanAsync();
            if (string.IsNullOrEmpty(ScanResult))
            {
                return;
            }
            if (!AnalysisScanResult(ScanResult))
            {
                LotEdit.Text = ScanResult;
                if (!ValidateLot())
                {
                    LotEdit.RequestFocus();
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
        }

        private async void LocScanButton_Click(object sender, EventArgs e)
        {
            string ScanResult = await CSISanner.ScanAsync();
            if (string.IsNullOrEmpty(ScanResult))
            {
                return;
            }
            if (!AnalysisScanResult(ScanResult))
            {
               LocEdit.Text = ScanResult;
                if (!ValidateLoc())
                {
                    LocEdit.RequestFocus();
                }
                else
                {
                    if (LotTracked)
                    {
                        LotEdit.RequestFocus();
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
            }
        }

        private async void ReasonScanButton_Click(object sender, EventArgs e)
        {
            string ScanResult = await CSISanner.ScanAsync();
            if (string.IsNullOrEmpty(ScanResult))
            {
                return;
            }
            if (!AnalysisScanResult(ScanResult))
            {
                ReasonEdit.Text = ScanResult;
                if (!ValidateReason())
                {
                    ReasonEdit.RequestFocus();
                }
                else
                {
                    ProcessButton.RequestFocus();
                }
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
                    LocEdit.RequestFocus();
                }
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
            }
        }

        private bool AnalysisScanResult(string Result)
        {
            //this is designed for future scan enhancement, such as scan one code to fill in all stuff...
            bool rtn = CSIDcJsonObjects.ReadMiscIssueReceiptAndQtyAdjustJson(Result, out string Item, out string UM, out string Qty, out string Loc, out string Lot, out string Reason);
            if (rtn)
            {
                if (!string.IsNullOrEmpty(Item))
                {
                    ItemEdit.Text = Item;
                    ItemValidated = false;
                    ValidateItem();
                }
                if (!string.IsNullOrEmpty(UM))
                {
                    UMEdit.Text = UM;
                    UMValidated = false;
                    ValidateUM();
                }
                if (!string.IsNullOrEmpty(Qty))
                {
                    QtyEdit.Text = Qty;
                    QtyValidated = false;
                    ValidateQty();
                }
                if (!string.IsNullOrEmpty(Loc))
                {
                    LocEdit.Text = Loc;
                    LocValidated = false;
                    ValidateLoc();
                }
                if (!string.IsNullOrEmpty(Lot))
                {
                    LotEdit.Text = Lot;
                    LotValidated = false;
                    ValidateLot();
                }
                if (!string.IsNullOrEmpty(Reason))
                {
                    ReasonEdit.Text = Reason;
                    ReasonValidated = false;
                    ValidateReason();
                }
                if (SNTracked)
                {
                    SNPicked = false;
                    ValidateSN();
                }
                EnableDisableComponents();
                try
                {
                    ProcessButton_Click(null, null);
                }catch(Exception Ex)
                {
                    WriteErrorLog(Ex);
                }
            }
            return rtn;
        }

        private bool PerformValidation()
        {
            TransDateText.Text = string.Format("{0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
            return ValidateItem() && ValidateUM() && ValidateQty() && ValidateLoc() && ValidateLot() && ValidateReason();
        }

        private void SetSNLabel()
        {
            SNButton.Text = string.Format(GetString(Resource.String.SNPicked), SNs.Count.ToString());
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

        public static void RunFragment(CSIBaseActivity activity)
        {
            try
            {
                FragmentTransaction ft = activity.FragmentManager.BeginTransaction();

                DCMiscReceiveFragment MiscReceiveDialog = (DCMiscReceiveFragment)activity.FragmentManager.FindFragmentByTag("MiscReceive");
                if (MiscReceiveDialog != null)
                {
                    ft.Show(MiscReceiveDialog);
                    //ft.AddToBackStack(null);
                }
                else
                {
                    // Create and show the dialog.
                    MiscReceiveDialog = new DCMiscReceiveFragment(activity);
                    //Add fragment
                    MiscReceiveDialog.Show(ft, "MiscReceive");
                }
            }
            catch (Exception Ex)
            {
                CSIErrorLog.WriteErrorLog(Ex);
            }
        }
        
    }
}