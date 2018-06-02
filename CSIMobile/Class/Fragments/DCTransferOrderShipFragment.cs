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
    public class DCTransferOrderShipFragment : CSIBaseDialogFragment
    {
        CSIDctrans SLDctrans;

        ImageButton TransferOrderScanButton;
        ImageButton TransferLineScanButton;
        ImageButton UMScanButton;
        ImageButton QtyScanButton;
        ImageButton FromLocScanButton;
        ImageButton FromLotScanButton;
        ImageButton ToLocScanButton;
        ImageButton ToLotScanButton;
        EditText WhseEdit;
        TextView TransDateText;
        EditText TransferOrderEdit;
        EditText TransferLineEdit;
        EditText QtyEdit;
        EditText UMEdit;
        EditText FromLocEdit;
        EditText FromLotEdit;
        EditText ToLocEdit;
        EditText ToLotEdit;
        TextView ItemText;
        TextView ItemDescText;
        TextView ItemUMText;
        TextView QtyText;
        TextView FromLocDescText;
        TextView ToLocDescText;
        LinearLayout QtyLinearLayout;
        LinearLayout UMLinearLayout;
        LinearLayout FromLocLinearLayout;
        LinearLayout FromLotLinearLayout;
        LinearLayout ToLocLinearLayout;
        LinearLayout ToLotLinearLayout;
        Button SNButton;
        Button ProcessButton;

        ImageView CloseImage;
        ProgressBar ProgressBar;
        LinearLayout Layout;

        bool FromLotTracked = false, ToLotTracked = false, FromSNTracked = false, ToSNTracked = false;
        bool InvUseExistingSerials = false, FobFromSite = false;
        bool TransferOrderValidated = false, TransferLineValidated = false, QtyValidated = false, UMValidated = false, FromLocValidated = false, FromLotValidated = false,ToLocValidated = false, ToLotValidated = false;
        List<string> SNs = new List<string>();
        bool FromSNPicked = true, ToSNPicked = true;

        public DCTransferOrderShipFragment(CSIBaseActivity activity = null) : base(activity)
        {
            CSISystemContext.ReadConfigurations();
            SLDctrans = new CSIDctrans(CSISystemContext);
            SLDctrans.AddProperty("TransNum");
            SLDctrans.AddProperty("TransType");
            SLDctrans.AddProperty("Stat");
            SLDctrans.AddProperty("Termid");
            SLDctrans.AddProperty("TransDate");
            SLDctrans.AddProperty("EmpNum");
            SLDctrans.AddProperty("TrnNum");
            SLDctrans.AddProperty("TrnLine");
            SLDctrans.AddProperty("UM");
            SLDctrans.AddProperty("Qty");
            SLDctrans.AddProperty("Loc");
            SLDctrans.AddProperty("Lot");
            SLDctrans.AddProperty("TrnLot");
            SLDctrans.AddProperty("UseExistingSerials");
            SLDctrans.AddProperty("DocumentNum");
            SLDctrans.AddProperty("ErrorMessage");

            SLDctrans.SetFilter("1=0");
            SLDctrans.UseAsync(false);
            SLDctrans.LoadIDO();
            SLDctrans.UseAsync(true);
            SLDctrans.SaveDataSetCompleted += SLDctrans_SaveDataSetCompleted;
            SLDctrans.LoadDataSetCompleted += SLDctrans_LoadDataSetCompleted;
            SLDctrans.CallMethodCompleted += SLDctrans_CallMethodCompleted;
        }

        private void SLDctrans_SaveDataSetCompleted(object sender, SaveDataSetCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    //check result status
                    if (SLDctrans.CurrentTable.Rows.Count <= 0)
                    {
                        //nothing happen or just delete rows
                    }
                    else
                    {
                        string RowStatus = SLDctrans.GetCurrentPropertyValueOfString("Stat");
                        string ErrorMessage = SLDctrans.GetCurrentPropertyValueOfString("ErrorMessage");

                        if ((RowStatus != "E") || string.IsNullOrEmpty(ErrorMessage))
                        {
                            if (CSISystemContext.ForceAutoPost)
                            {
                                //Ready to Post -- calling DctsPSp
                                ShowProgressBar(true);
                                string strParmeters = "";
                                strParmeters = CSIBaseInvoker.BuildXMLParameters(strParmeters, SLDctrans.GetCurrentPropertyValueOfString("TransNum"));
                                strParmeters = CSIBaseInvoker.BuildXMLParameters(strParmeters, "", true);
                                SLDctrans.InvokeMethod("DctsPSp", strParmeters);
                            }
                            else
                            {
                                if (CSISystemContext.ShowSuccessMessage)
                                {
                                    ShowProcessedMessage();
                                }
                                //Clear Result if no error.
                                Initialize();
                            }
                        }
                        else
                        {
                            //delete first before prompt message.
                            SLDctrans.CurrentTable.Rows[0].Delete();
                            ShowProgressBar(true);
                            SLDctrans.DeleteIDO();

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

        private void SLDctrans_CallMethodCompleted(object sender, CallMethodCompletedEventArgs e)
        {
            try
            {
                //throw new NotImplementedException();
                if (e.Error == null)
                {
                    if (e.Result.ToString() == "0")
                    {
                        if (CSISystemContext.ShowSuccessMessage)
                        {
                            ShowProcessedMessage();
                        }
                        Initialize();
                    }
                    else
                    {
                        //get error - delete first.
                        SLDctrans.CurrentTable.Rows[0].Delete();
                        ShowProgressBar(true);
                        SLDctrans.DeleteIDO();
                        WriteErrorLog(new Exception(CSIBaseInvoker.GetXMLParameters(e.strMethodParameters,1)));
                    }
                }
                else
                {
                    //try to delete post
                    SLDctrans.CurrentTable.Rows[0].Delete();
                    ShowProgressBar(true);
                    SLDctrans.DeleteIDO();
                    WriteErrorLog(e.Error);
                }
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
            ShowProgressBar(false);
        }

        private void SLDctrans_LoadDataSetCompleted(object sender, LoadDataSetCompletedEventArgs e)
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
                base.OnCreateView(inflater, container, savedInstanceState);

                var view = inflater.Inflate(Resource.Layout.CSITransferOrderShip, container, false);
                Cancelable = false;

                WhseEdit = view.FindViewById<EditText>(Resource.Id.WhseEdit);
                TransDateText = view.FindViewById<TextView>(Resource.Id.TransDateText);

                TransferOrderEdit = view.FindViewById<EditText>(Resource.Id.TransferOrderEdit);
                TransferOrderScanButton = view.FindViewById<ImageButton>(Resource.Id.TransferOrderScanButton);
                TransferLineEdit = view.FindViewById<EditText>(Resource.Id.TransferLineEdit);
                TransferLineScanButton = view.FindViewById<ImageButton>(Resource.Id.TransferLineScanButton);
                UMEdit = view.FindViewById<EditText>(Resource.Id.UMEdit);
                UMScanButton = view.FindViewById<ImageButton>(Resource.Id.UMScanButton);
                QtyEdit = view.FindViewById<EditText>(Resource.Id.QtyEdit);
                QtyScanButton = view.FindViewById<ImageButton>(Resource.Id.QtyScanButton);
                FromLocEdit = view.FindViewById<EditText>(Resource.Id.FromLocEdit);
                FromLocScanButton = view.FindViewById<ImageButton>(Resource.Id.FromLocScanButton);
                FromLotEdit = view.FindViewById<EditText>(Resource.Id.FromLotEdit);
                FromLotScanButton = view.FindViewById<ImageButton>(Resource.Id.FromLotScanButton);
                ToLocEdit = view.FindViewById<EditText>(Resource.Id.ToLocEdit);
                ToLocScanButton = view.FindViewById<ImageButton>(Resource.Id.ToLocScanButton);
                ToLotEdit = view.FindViewById<EditText>(Resource.Id.ToLotEdit);
                ToLotScanButton = view.FindViewById<ImageButton>(Resource.Id.ToLotScanButton);

                QtyLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.QtyLinearLayout);
                UMLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.UMLinearLayout);
                FromLocLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.FromLocLinearLayout);
                FromLotLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.FromLotLinearLayout);
                ToLocLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.ToLocLinearLayout);
                ToLotLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.ToLotLinearLayout);

                SNButton = view.FindViewById<Button>(Resource.Id.SNButton);
                ProcessButton = view.FindViewById<Button>(Resource.Id.ProcessButton);
                Layout = view.FindViewById<LinearLayout>(Resource.Id.LinearLayout);

                ItemText = view.FindViewById<TextView>(Resource.Id.ItemText);
                ItemDescText = view.FindViewById<TextView>(Resource.Id.ItemDescText);
                ItemUMText = view.FindViewById<TextView>(Resource.Id.ItemUMText);
                QtyText = view.FindViewById<TextView>(Resource.Id.QtyText);
                FromLocDescText = view.FindViewById<TextView>(Resource.Id.FromLocDescText);
                ToLocDescText = view.FindViewById<TextView>(Resource.Id.ToLocDescText);

                CloseImage = view.FindViewById<ImageView>(Resource.Id.CloseImage);
                ProgressBar = view.FindViewById<ProgressBar>(Resource.Id.ProgressBar);

                TransferOrderScanButton.Click += TransferOrderScanButton_Click;
                TransferLineScanButton.Click += TransferLineScanButton_Click;
                QtyScanButton.Click += QtyScanButton_Click;
                UMScanButton.Click += UMScanButton_Click;
                FromLocScanButton.Click += FromLocScanButton_Click;
                FromLotScanButton.Click += FromLotScanButton_Click;
                ToLocScanButton.Click += ToLocScanButton_Click;
                ToLotScanButton.Click += ToLotScanButton_Click;

                TransferOrderEdit.FocusChange += TransferOrderEdit_FocusChange;
                TransferLineEdit.FocusChange += TransferLineEdit_FocusChange;
                QtyEdit.FocusChange += QtyEdit_FocusChange;
                UMEdit.FocusChange += UMEdit_FocusChange;
                FromLocEdit.FocusChange += FromLocEdit_FocusChange;
                FromLotEdit.FocusChange += FromLotEdit_FocusChange;
                ToLocEdit.FocusChange += ToLocEdit_FocusChange;
                ToLotEdit.FocusChange += ToLotEdit_FocusChange;

                TransferOrderEdit.KeyPress += TransferOrderEdit_KeyPress;
                TransferLineEdit.KeyPress += TransferLineEdit_KeyPress;
                QtyEdit.KeyPress += QtyEdit_KeyPress;
                UMEdit.KeyPress += UMEdit_KeyPress;
                FromLocEdit.KeyPress += FromLocEdit_KeyPress;
                FromLotEdit.KeyPress += FromLotEdit_KeyPress;
                ToLocEdit.KeyPress += ToLocEdit_KeyPress;
                ToLotEdit.KeyPress += ToLotEdit_KeyPress;

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
                Dismiss();
                Dispose();
                return null;
            }
        }
        private void ToLotEdit_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Enter || e.KeyCode == Keycode.Tab)
            {
                if (e.Event.Action == KeyEventActions.Up)
                {
                    ValidateToLot();
                    ProcessButton.RequestFocus();
                }
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
                ToLotValidated = false;
            }
        }

        private void ToLocEdit_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Enter || e.KeyCode == Keycode.Tab)
            {
                if (e.Event.Action == KeyEventActions.Up)
                {
                    ValidateToLoc();
                    if (ToLotTracked)
                    {
                        ToLotEdit.RequestFocus();
                    }
                    else
                    {
                        ProcessButton.RequestFocus();
                    }
                }
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
                FromLocValidated = false;
            }

        }
        private void FromLotEdit_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Enter || e.KeyCode == Keycode.Tab)
            {
                if (e.Event.Action == KeyEventActions.Up)
                {
                    ValidateFromLot();
                    ToLocEdit.RequestFocus();
                }
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
                FromLotValidated = false;
            }
        }

        private void FromLocEdit_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Enter || e.KeyCode == Keycode.Tab)
            {
                if (e.Event.Action == KeyEventActions.Up)
                {
                    ValidateFromLoc();
                    if (FromLotTracked)
                    {
                        FromLotEdit.RequestFocus();
                    }
                    else
                    {
                        ToLocEdit.RequestFocus();
                    }
                }
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
                FromLocValidated = false;
            }

        }

        private void QtyEdit_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Enter || e.KeyCode == Keycode.Tab)
            {
                if (e.Event.Action == KeyEventActions.Up)
                {
                    ValidateQty();
                    FromLocEdit.RequestFocus();
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

        private void TransferLineEdit_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Enter || e.KeyCode == Keycode.Tab)
            {
                if (e.Event.Action == KeyEventActions.Up)
                {
                    ValidateTransferLine();
                    UMEdit.RequestFocus();
                }
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
                TransferLineValidated = false;
            }
        }

        private void TransferOrderEdit_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Enter || e.KeyCode == Keycode.Tab)
            {
                if (e.Event.Action == KeyEventActions.Up)
                {
                    ValidateTransferOrder();
                    TransferLineEdit.RequestFocus();
                }
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
                TransferOrderValidated = false;
            }
        }

        private void Initialize()
        {
            WhseEdit.Text = CSISystemContext.DefaultWarehouse;
            TransDateText.Text = string.Format("{0} {1}",DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
            TransferOrderEdit.Text = string.Empty;
            TransferLineEdit.Text = string.Empty;
            UMEdit.Text = string.Empty;
            QtyEdit.Text = string.Empty;
            FromLocEdit.Text = string.Empty;
            FromLotEdit.Text = string.Empty;
            ToLocEdit.Text = string.Empty;
            ToLotEdit.Text = string.Empty;
            ItemText.Text = string.Empty;
            ItemDescText.Text = string.Empty;
            ItemUMText.Text = string.Empty;
            QtyText.Text = string.Empty;
            FromLocDescText.Text = string.Empty;
            ToLocDescText.Text = string.Empty;

            TransferOrderValidated = false;
            TransferLineValidated = false;
            QtyValidated = false;
            UMValidated = false;
            FromLocValidated = false;
            FromLotValidated = false;
            ToLocValidated = false;
            ToLotValidated = false;

            SetSNLabel();
        }

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            PerformValidation();
            if (TransferOrderValidated && TransferLineValidated && QtyValidated && UMValidated && FromLocValidated && (FromLotValidated || !FromLotTracked) && ToLocValidated && (ToLotValidated || !ToLotTracked) && FromSNPicked)
            {
                SLDctrans.CurrentTable.Rows.Clear();
                DataRow Row = SLDctrans.CurrentTable.NewRow();
                Row["TransNum"] = SLDctrans.NextTransNum();//TransNum
                Row["TransType"] = "1";//TransType 1: Ship, 2:Receive
                Row["Stat"] = "U";//Stat
                Row["Termid"] = CSISystemContext.AndroidId.Substring(CSISystemContext.AndroidId.Length - 4, 4);//Termid
                Row["TransDate"] = DateTime.Now;//TransDate
                Row["EmpNum"] = CSISystemContext.EmpNum;//EmpNum
                Row["TrnNum"] = TransferOrderEdit.Text;//Item
                Row["TrnLine"] = TransferLineEdit.Text;//UM
                Row["Qty"] = QtyEdit.Text;//QtyMoved
                Row["UM"] = UMEdit.Text;//UM
                Row["Loc"] = FromLocEdit.Text;//FromLoc
                Row["Lot"] = FromLotEdit.Text;//FromLot
                Row["TrnLot"] = ToLotEdit.Text;//ToLot
                Row["UseExistingSerials"] = InvUseExistingSerials;//UseExistingSerials
                SLDctrans.CurrentTable.Rows.Add(Row);
                //Row.BeginEdit();
                //Row.EndEdit();
                //Row.AcceptChanges();

                SetKeyValues(GetString(Resource.String.OrderNumber), TransferOrderEdit.Text, GetString(Resource.String.OrderLine), TransferLineEdit.Text);

                SLDctrans.InsertIDO();
                ShowProgressBar(true);
            }
        }

        private void SNButton_Click(object sender, EventArgs e)
        {
            SerialGenerator.RunFragment(this);
        }

        private void ToLotEdit_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {//gain focus
                ToLotEdit.SelectAll();
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
                ToLocEdit.SelectAll();
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
                FromLotEdit.SelectAll();
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
                FromLocEdit.SelectAll();
            }
            else
            {//lose focus
                ValidateFromLoc();
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
                ValidateUM ();
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

        private void TransferLineEdit_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {//gain focus
                TransferLineEdit.SelectAll();
            }
            else
            {//lose focus
                ValidateTransferLine();
            }
        }

        private void TransferOrderEdit_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {//gain focus
                TransferOrderEdit.SelectAll();
            }
            else
            {//lose focus
                ValidateTransferOrder();
            }
        }

        private bool ValidateTransferOrder()
        {
            if (!TransferOrderValidated)
            {
                if (string.IsNullOrEmpty(TransferOrderEdit.Text))
                {
                    TransferOrderValidated = false;
                }
                else
                {
                    try
                    {
                        string TransferOrder = TransferOrderEdit.Text, TransferLine = TransferLineEdit.Text, Item = ItemText.Text
                            , ItemDesc = ItemDescText.Text, ItemUM = ItemUMText.Text, ToLoc = ToLocEdit.Text, QtyReq = QtyText.Text
                            , QtyShipped = "", QtyReceived = "", QtyRequired = "";

                        //validate TransferOrder and TransferLine
                        TransferOrderValidated = CSITrnitems.GetTransferLineInfor(CSISystemContext, ref TransferOrder, ref TransferLine, ref Item, ref ItemDesc, ref ItemUM, ref ToLoc, ref InvUseExistingSerials
                            , ref QtyReq, ref QtyShipped, ref QtyReceived, ref QtyRequired, ref FromLotTracked, ref FromSNTracked, ref ToLotTracked, ref ToSNTracked, ref FobFromSite);
                        if (TransferOrderValidated == true)
                        {
                            TransferOrderEdit.Text = TransferOrder;
                            TransferLineEdit.Text = TransferLine;
                            ItemText.Text = Item;
                            ItemDescText.Text = ItemDesc;
                            ItemUMText.Text = ItemUM;
                            QtyText.Text = QtyReq;
                            ToLocEdit.Text = ToLoc;

                            ToLocValidated = true;
                            TransferLineValidated = true;

                            //Validate UM
                            if (string.IsNullOrEmpty(UMEdit.Text))
                            {
                                UMEdit.Text = ItemUM;
                                UMValidated = true;
                            }

                            //validate Qty
                            if (string.IsNullOrEmpty(QtyEdit.Text) || decimal.Parse(QtyEdit.Text) == 0)
                            {
                                QtyEdit.Text = QtyRequired;
                                QtyValidated = true;
                            }

                            //Validate ItemFromLoc
                            string FromLoc = FromLocEdit.Text, FromLocDescription = "", Qty = "";
                            bool RtnCSIItemFromLocs = CSIItemLocs.GetItemLocInfor(CSISystemContext, ItemText.Text, WhseEdit.Text, ref FromLoc, ref FromLocDescription, ref Qty);
                            if (RtnCSIItemFromLocs == true)
                            {
                                FromLocEdit.Text = FromLoc;
                                FromLocDescText.Text = FromLocDescription;
                                FromLocValidated = false;
                                ValidateFromLoc();
                                //OnHandQuantityText.Text = Qty; //used for validate Qty
                            }
                        }
                        else
                        {
                            ItemText.Text = string.Empty;
                            ItemDescText.Text = string.Empty;
                            ItemUMText.Text = string.Empty;
                            QtyText.Text = string.Empty;
                        }

                    }
                    catch (Exception Ex)
                    {
                        WriteErrorLog(Ex);
                        TransferOrderValidated = false;
                    }
                }
            }
            EnableDisableComponents();
            return TransferOrderValidated;
        }

        private bool ValidateTransferLine()
        {
            if (!TransferLineValidated)
            {
                if (string.IsNullOrEmpty(TransferLineEdit.Text))
                {
                    TransferLineValidated = false;
                }
                else
                {
                    try
                    {
                        string TransferOrder = TransferOrderEdit.Text, TransferLine = TransferLineEdit.Text, Item = ItemText.Text
                            , ItemDesc = ItemDescText.Text, ItemUM = ItemUMText.Text, ToLoc = ToLocEdit.Text, QtyReq = QtyText.Text
                            , QtyShipped = "", QtyReceived = "", QtyRequired = "";

                        //validate TransferOrder and TransferLine
                        TransferOrderValidated = CSITrnitems.GetTransferLineInfor(CSISystemContext, ref TransferOrder, ref TransferLine, ref Item, ref ItemDesc, ref ItemUM, ref ToLoc, ref InvUseExistingSerials
                            , ref QtyReq, ref QtyShipped, ref QtyReceived, ref QtyRequired, ref FromLotTracked, ref FromSNTracked, ref ToLotTracked, ref ToSNTracked, ref FobFromSite);
                        if (TransferOrderValidated == true)
                        {
                            TransferOrderEdit.Text = TransferOrder;
                            TransferLineEdit.Text = TransferLine;
                            ItemText.Text = Item;
                            ItemDescText.Text = ItemDesc;
                            ItemUMText.Text = ItemUM;
                            QtyText.Text = QtyReq;
                            ToLocEdit.Text = ToLoc;

                            ToLocValidated = true;
                            TransferLineValidated = true;

                            //Validate UM
                            if (string.IsNullOrEmpty(UMEdit.Text))
                            {
                                UMEdit.Text = ItemUM;
                                UMValidated = true;
                            }

                            //validate Qty
                            if (string.IsNullOrEmpty(QtyEdit.Text) || decimal.Parse(QtyEdit.Text) == 0)
                            {
                                QtyEdit.Text = QtyRequired;
                                QtyValidated = true;
                            }

                            //Validate ItemFromLoc
                            string FromLoc = FromLocEdit.Text, FromLocDescription = "", Qty = "";
                            bool RtnCSIItemFromLocs = CSIItemLocs.GetItemLocInfor(CSISystemContext, ItemText.Text, WhseEdit.Text, ref FromLoc, ref FromLocDescription, ref Qty);
                            if (RtnCSIItemFromLocs == true)
                            {
                                FromLocEdit.Text = FromLoc;
                                FromLocDescText.Text = FromLocDescription;
                                FromLocValidated = false;
                                ValidateFromLoc();
                                //OnHandQuantityText.Text = Qty; //used for validate Qty
                            }
                        }
                        else
                        {
                            ItemText.Text = string.Empty;
                            ItemDescText.Text = string.Empty;
                            ItemUMText.Text = string.Empty;
                            QtyText.Text = string.Empty;
                        }

                    }
                    catch (Exception Ex)
                    {
                        WriteErrorLog(Ex);
                        TransferOrderValidated = false;
                    }
                }
            }
            EnableDisableComponents();
            return TransferLineValidated;
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

        private bool ValidateFromLoc()
        {
            if (!FromLocValidated)
            {
                if (string.IsNullOrEmpty(FromLocEdit.Text))
                {
                    FromLocValidated = false;
                }
                else
                {
                    string FromLoc = FromLocEdit.Text, FromLocDescription = "", FromLot = FromLotEdit.Text, Qty = "";
                    FromLocValidated = CSIItemLocs.GetItemLocInfor(CSISystemContext, ItemText.Text, WhseEdit.Text, ref FromLoc, ref FromLocDescription, ref Qty);
                    if (FromLocValidated)
                    {
                        FromLocDescText.Text = FromLocDescription;
                    }
                    else
                    {
                        try
                        {
                            CSILocations SLLoc = new CSILocations(CSISystemContext);
                            SLLoc.UseAsync(false);
                            SLLoc.AddProperty("Loc");
                            SLLoc.AddProperty("Description");
                            SLLoc.SetFilter(string.Format("Loc = N'{0}'", FromLoc));
                            SLLoc.LoadIDO();
                            if (SLLoc.CurrentTable.Rows.Count <= 0)
                            {
                                FromLocDescText.Text = string.Empty;
                                FromLocValidated = false;
                            }
                            else
                            {
                                FromLocEdit.Text = SLLoc.GetCurrentPropertyValueOfString("FromLoc"); ;
                                FromLocDescText.Text = SLLoc.GetCurrentPropertyValueOfString("Description"); ;
                                FromLocValidated = true;
                            }
                        }catch (Exception Ex)
                        {
                            WriteErrorLog(Ex);
                            FromLocValidated = false;
                        }
                        //FromLocEdit.Text = FromLoc;
                        //FromLocDescText.Text = FromLocDescription;
                        //bool RtnCSIFromLotFromLocs = CSIFromLotFromLocs.GetItemFromLotFromLocInfor(CSISystemContext, TransferOrderEdit.Text, WhseEdit.Text, FromLoc, ref FromLot, ref Qty);
                        //if (RtnCSIFromLotFromLocs)
                        //{
                        //    FromLotEdit.Text = FromLot;
                        //    FromLotValidated = false;
                        ValidateFromLot();
                        //    //ReleasedQuantityText.Text = Qty; //used for validate Qty
                        //}
                    }
                }
            }
            EnableDisableComponents();
            return FromLocValidated;
        }

        private bool ValidateFromLot()
        {
            if (!FromLotValidated)
            {
                if (string.IsNullOrEmpty(FromLotEdit.Text))
                {
                    if (FromLotTracked)
                    {
                        FromLotValidated = false;
                    }
                    else
                    {
                        if (ToLotTracked)
                        {
                            ToLotEdit.Text = FromLotEdit.Text;
                        }
                        FromLotValidated = true;
                    }
                }
                else
                {
                    FromLotValidated = true;
                }
            }
            EnableDisableComponents();
            return FromLotValidated;
        }

        private bool ValidateToLoc()
        {
            if (!ToLocValidated)
            {
                if (string.IsNullOrEmpty(ToLocEdit.Text))
                {
                    ToLocValidated = false;
                }
                else
                {
                    ToLocValidated = true;
                }
            }
            else
            {
                ToLocValidated = true;
            }
            return ToLocValidated;
        }

        private bool ValidateToLot()
        {
            if (!ToLotValidated)
            {
                if (string.IsNullOrEmpty(ToLotEdit.Text))
                {
                    if (ToLotTracked)
                    {
                        ToLotEdit.Text = FromLotEdit.Text;
                        if (string.IsNullOrEmpty(ToLotEdit.Text))
                        {
                            string Message = "", Key = "";
                            //To Do
                            //Get Remote Lot Information.
                            ToLotValidated = CSIItems.GetNextLotSp(CSISystemContext, ItemText.Text, "", ref Message, ref Key);
                            if (string.IsNullOrEmpty(Message))
                            {
                                ToLotEdit.Text = Key;
                            }
                            else
                            {
                                //ToLotValidated = false;
                            }
                        }
                        else
                        {
                            ToLotValidated = true;
                        }
                    }
                    else
                    {
                        ToLotValidated = true;
                    }
                }
                else
                {
                    ToLotValidated = true;
                }
            }
            EnableDisableComponents();
            return ToLotValidated;
        }

        private bool ValidateFromSN()
        {
            if (FromSNTracked)
            {
                FromSNPicked = SNs.Count == int.Parse(QtyEdit.Text);
            }
            else
            {
                FromSNPicked = true;
            }
            return FromSNPicked;
        }

        private bool ValidateToSN()
        {
            if (ToSNTracked)
            {
                ToSNPicked = SNs.Count == int.Parse(QtyEdit.Text);
            }
            else
            {
                ToSNPicked = true;
            }
            return ToSNPicked;
        }
        private void EnableDisableComponents()
        {
            try
            {
                if (string.IsNullOrEmpty(TransferOrderEdit.Text))
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
                FromLotLinearLayout.Visibility = FromLotTracked ? ViewStates.Visible : ViewStates.Gone;
                FromLotScanButton.Enabled = FromLotTracked;
                ToLotLinearLayout.Visibility = ToLotTracked ? ViewStates.Visible : ViewStates.Gone;
                ToLotScanButton.Enabled = ToLotTracked && !FromLotTracked;
                SNButton.Visibility = (FromSNTracked || (ToSNTracked && FobFromSite)) ? ViewStates.Visible : ViewStates.Gone;
                SNButton.Enabled = (FromSNTracked || (ToSNTracked && FobFromSite));
                ProcessButton.Enabled = TransferOrderValidated && TransferLineValidated && QtyValidated && UMValidated
                    && FromLocValidated && ((FromLotTracked && FromLotValidated) || !FromLotTracked) && ((FromSNTracked && FromSNPicked) || !FromSNTracked) && ((ToSNTracked && FobFromSite && ToSNPicked) || !ToSNTracked);
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
        }
        private async void ToLotScanButton_Click(object sender, EventArgs e)
        {
            if (ToLotTracked && !FromLotTracked)
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
                        if (ToSNTracked)
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

        private async void ToLocScanButton_Click(object sender, EventArgs e)
        {
            /*
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
                    if (ToLotTracked)
                    {
                        ToLotEdit.RequestFocus();
                    }
                    else
                    {
                        if (ToSNTracked)
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
            */
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
                    if (FromSNTracked)
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
                    if (FromLotTracked)
                    {
                        FromLotEdit.RequestFocus();
                    }
                    else
                    {
                        if (FromSNTracked)
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

        private async void TransferLineScanButton_Click(object sender, EventArgs e)
        {
            string ScanResult = await CSISanner.ScanAsync();
            if (string.IsNullOrEmpty(ScanResult))
            {
                return;
            }
            if (!AnalysisScanResult(ScanResult))
            {
                TransferLineEdit.Text = ScanResult;
                if (!ValidateTransferLine())
                {
                    TransferLineEdit.RequestFocus();
                }
                else
                {
                    UMEdit.RequestFocus();
                }
            }
        }

        private async void TransferOrderScanButton_Click(object sender, EventArgs e)
        {
            string ScanResult = await CSISanner.ScanAsync();
            if (string.IsNullOrEmpty(ScanResult))
            {
                return;
            }
            if (!AnalysisScanResult(ScanResult))
            {
                TransferOrderEdit.Text = ScanResult;
                if (!ValidateTransferOrder())
                {
                    TransferOrderEdit.RequestFocus();
                }
                else
                {
                    TransferLineEdit.RequestFocus();
                }
            }
        }

        private bool AnalysisScanResult(string Result)
        {
            //this is designed for future scan enhancement, such as scan one code to fill in all stuff...
            bool rtn = CSIDcJsonObjects.ReadTransferOrderShipJson(Result, out string TransferOrder, out string TransferLine, out string UM, out string Qty
                , out string FromLoc, out string FromLot, out string ToLot);
            if (rtn)
            {
                if (!string.IsNullOrEmpty(TransferOrder))
                {
                    TransferOrderEdit.Text = TransferOrder;
                    TransferOrderValidated = false;
                    ValidateTransferOrder();
                }
                if (!string.IsNullOrEmpty(TransferLine))
                {
                    TransferLineEdit.Text = TransferLine;
                    TransferLineValidated = false;
                    ValidateTransferLine();
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
                if (!string.IsNullOrEmpty(FromLoc))
                {
                    FromLocEdit.Text = FromLoc;
                    FromLocValidated = false;
                    ValidateFromLoc();
                }
                if (!string.IsNullOrEmpty(FromLot))
                {
                    if (FromLotTracked) {
                        FromLotEdit.Text = FromLot;
                    }
                    FromLotValidated = false;
                    ValidateFromLot();
                }
                if (!string.IsNullOrEmpty(ToLot))
                {
                    if (!FromLotTracked && ToLotTracked) {
                        ToLotEdit.Text = ToLot;
                    }
                    ToLotValidated = false;
                    ValidateToLot();
                }
                if (FromSNTracked)
                {
                    FromSNPicked = false;
                    ValidateFromSN();
                }
                if (ToSNTracked)
                {
                    ToSNPicked = false;
                    ValidateToSN();
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
            return ValidateTransferOrder() && ValidateTransferLine() && ValidateQty() && ValidateUM() && ValidateFromLoc() && ValidateFromLot() && ValidateToLoc() && ValidateToLot();
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
                    ProgressBar.Visibility = ViewStates.Invisible;
                    CSIBaseObject.DisableEnableControls(true, Layout);

                    EnableDisableComponents();
                }
            }
            CloseImage.Visibility = HasTitle ? ViewStates.Gone : ViewStates.Visible;
        }

        public static void RunFragment(CSIBaseActivity activity)
        {
            try
            {
                FragmentTransaction ft = activity.FragmentManager.BeginTransaction();

                DCTransferOrderShipFragment TransferOrderShipDialog = (DCTransferOrderShipFragment)activity.FragmentManager.FindFragmentByTag("TransferOrderShip");
                if (TransferOrderShipDialog != null)
                {
                    ft.Show(TransferOrderShipDialog);
                    //ft.AddToBackStack(null);
                }
                else
                {
                    // Create and show the dialog.
                    TransferOrderShipDialog = new DCTransferOrderShipFragment(activity);
                    //Add fragment
                    TransferOrderShipDialog.Show(ft, "TransferOrderShip");
                }
            }
            catch (Exception Ex)
            {
                CSIErrorLog.WriteErrorLog(Ex);
            }
        }
    }
}
