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
    public class DCQuantityMoveFragment : CSIBaseDialogFragment
    {
        CSIDcmoves SLDcmoves;

        ImageButton ItemScanButton;
        ImageButton UMScanButton;
        ImageButton QtyScanButton;
        ImageButton FromLocScanButton;
        ImageButton FromLotScanButton;
        ImageButton ToLocScanButton;
        ImageButton ToLotScanButton;
        EditText WhseEdit;
        TextView TransDateText;
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
        bool SNPicked = true;

        private int ProcessCount = 0;

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
            SLDcmoves.AddProperty("DocumentNum");
            SLDcmoves.AddProperty("ErrorMessage");

            SLDcmoves.SetFilter("1=0");
            SLDcmoves.LoadIDO();
            SLDcmoves.SaveDataSetCompleted += SLDcmoves_SaveDataSetCompleted;
            SLDcmoves.LoadDataSetCompleted += SLDcmoves_LoadDataSetCompleted;
            SLDcmoves.CallMethodCompleted += SLDcmoves_CallMethodCompleted;
        }

        private void SLDcmoves_SaveDataSetCompleted(object sender, SaveDataSetCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    //check result status
                    if (SLDcmoves.CurrentTable.Rows.Count <= 0)
                    {
                        //nothing happen or just delete rows
                    }
                    else
                    {
                        string RowStatus = SLDcmoves.GetCurrentPropertyValueOfString("Stat");
                        string ErrorMessage = SLDcmoves.GetCurrentPropertyValueOfString("ErrorMessage");

                        if ((RowStatus != "E") || string.IsNullOrEmpty(ErrorMessage))
                        {
                            //Ready to Post -- calling DcmovePSp
                            ShowProgressBar(true);
                            string strParmeters = "";
                            strParmeters = CSIBaseInvoker.BuildXMLParameters(strParmeters, SLDcmoves.GetCurrentPropertyValueOfString("TransNum"));
                            strParmeters = CSIBaseInvoker.BuildXMLParameters(strParmeters, "");
                            strParmeters = CSIBaseInvoker.BuildXMLParameters(strParmeters, "", true);
                            SLDcmoves.InvokeMethod("DcmovePSp", strParmeters);
                        }
                        else
                        {
                            //delete first before prompt message.
                            SLDcmoves.CurrentTable.Rows[0].Delete();
                            ShowProgressBar(true);
                            SLDcmoves.DeleteIDO();

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

        private void SLDcmoves_CallMethodCompleted(object sender, CallMethodCompletedEventArgs e)
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
                        SLDcmoves.CurrentTable.Rows[0].Delete();
                        ShowProgressBar(true);
                        SLDcmoves.DeleteIDO();
                        WriteErrorLog(new Exception(CSIBaseInvoker.GetXMLParameters(e.strMethodParameters,1)));
                    }
                }
                else
                {
                    //try to delete post
                    SLDcmoves.CurrentTable.Rows[0].Delete();
                    ShowProgressBar(true);
                    SLDcmoves.DeleteIDO();
                    WriteErrorLog(e.Error);
                }
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
            ShowProgressBar(false);
        }

        private void SLDcmoves_LoadDataSetCompleted(object sender, LoadDataSetCompletedEventArgs e)
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

                var view = inflater.Inflate(Resource.Layout.CSIQuantityMove, container, false);
                Cancelable = false;
                
                WhseEdit = view.FindViewById<EditText>(Resource.Id.WhseEdit);
                TransDateText = view.FindViewById<TextView>(Resource.Id.TransDateText);
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

                ItemEdit.KeyPress += ItemEdit_KeyPress;
                UMEdit.KeyPress += UMEdit_KeyPress;
                QtyEdit.KeyPress += QtyEdit_KeyPress;
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
                    if (LotTracked)
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
                ToLocValidated = false;
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
                    if (LotTracked)
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

        private void ItemEdit_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Enter || e.KeyCode == Keycode.Tab)
            {
                if (e.Event.Action == KeyEventActions.Up)
                {
                    ValidateItem();
                    UMEdit.RequestFocus();
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
            FromLocEdit.Text = string.Empty;
            FromLotEdit.Text = string.Empty;
            ToLocEdit.Text = string.Empty;
            ToLotEdit.Text = string.Empty;
            ItemDescText.Text = string.Empty;
            ItemUMText.Text = string.Empty;
            OnHandQuantityText.Text = string.Empty;
            FromLocDescText.Text = string.Empty;
            ToLocDescText.Text = string.Empty;
            ItemValidated = false;
            UMValidated = false;
            QtyValidated = false;
            FromLocValidated = false;
            FromLotValidated = false;
            ToLocValidated = false;
            ToLotValidated = false;
            SetSNLabel();
        }

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            PerformValidation();
            if (ItemValidated && UMValidated && QtyValidated && FromLocValidated && (FromLotValidated || !LotTracked) && ToLocValidated && (ToLotValidated || !LotTracked) && SNPicked)
            {
                SLDcmoves.CurrentTable.Rows.Clear();
                DataRow Row = SLDcmoves.CurrentTable.NewRow();
                Row["TransNum"] = 0;//TransNum
                Row["TransType"] = "1";//TransType
                Row["Stat"] = "U";//Stat
                Row["Termid"] = CSISystemContext.AndroidId.Substring(CSISystemContext.AndroidId.Length - 4, 4);//Termid
                Row["TransDate"] = DateTime.Now;//TransDate
                Row["Whse"] = CSISystemContext.DefaultWarehouse;//Whse
                Row["EmpNum"] = CSISystemContext.EmpNum;//EmpNum
                Row["Item"] = ItemEdit.Text;//Item
                Row["UM"] = UMEdit.Text;//UM
                Row["QtyMoved"] = QtyEdit.Text;//QtyMoved
                Row["Loc1"] = FromLocEdit.Text;//Loc1
                Row["Lot1"] = FromLotEdit.Text;//Lot1
                Row["Loc2"] = ToLocEdit.Text;//Loc2
                Row["Lot2"] = ToLotEdit.Text;//Lot2
                SLDcmoves.CurrentTable.Rows.Add(Row);
                //Row.BeginEdit();
                //Row.EndEdit();
                //Row.AcceptChanges();
                SLDcmoves.InsertIDO();
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
                ValidateFromLoc ();
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
                            }
                            OnHandQuantityText.Text = Qty;
                        }
                        else
                        {
                            ItemDescText.Text = string.Empty;
                            ItemUMText.Text = string.Empty;
                            OnHandQuantityText.Text = "0";
                        }

                        string Loc = FromLocEdit.Text, LocType = "";
                        bool RtnCSIItemLocs = CSIItemLocs.GetItemLocInfor(CSISystemContext, ItemEdit.Text, WhseEdit.Text, ref Loc, ref LocType, ref Qty);
                        if (RtnCSIItemLocs == true)
                        {
                            FromLocEdit.Text = Loc;
                            FromLocValidated = false;
                            ValidateFromLoc();
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
                    string Loc = FromLocEdit.Text, LocDescription = "", Lot = FromLotEdit.Text, Qty = "";
                    FromLocValidated = CSIItemLocs.GetItemLocInfor(CSISystemContext, ItemEdit.Text, WhseEdit.Text, ref Loc, ref LocDescription, ref Qty);
                    if (FromLocValidated)
                    {
                        FromLocEdit.Text = Loc;
                        FromLocDescText.Text = LocDescription;
                        bool RtnCSILotLocs = CSILotLocs.GetItemLotLocInfor(CSISystemContext, ItemEdit.Text, WhseEdit.Text, FromLocEdit.Text, ref Lot, ref Qty);
                        if (RtnCSILotLocs)
                        {
                            FromLotEdit.Text = Lot;
                            FromLotValidated = false;
                            ValidateFromLot();
                            //OnHandQuantityText.Text = Qty; //used for validate Qty
                        }
                    }
                    else
                    {
                        FromLocDescText.Text = string.Empty;
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
                    FromLotValidated = false;
                }
                else
                {
                    FromLotValidated = true;
                    if (string.IsNullOrEmpty(ToLotEdit.Text))
                    {
                        ToLotEdit.Text = FromLotEdit.Text;
                    }
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
                    string Loc = ToLocEdit.Text, LocDescription = "", Lot = ToLotEdit.Text, Qty = "";
                    ToLocValidated = CSIItemLocs.GetItemLocInfor(CSISystemContext, ItemEdit.Text, WhseEdit.Text, ref Loc, ref LocDescription, ref Qty);
                    if (ToLocValidated)
                    {
                        ToLocEdit.Text = Loc;
                        ToLocDescText.Text = LocDescription;
                        bool RtnCSILotLocs = CSILotLocs.GetItemLotLocInfor(CSISystemContext, ItemEdit.Text, WhseEdit.Text, Loc, ref Lot, ref Qty);
                        if (RtnCSILotLocs)
                        {
                            ToLotEdit.Text = Lot;
                            ToLotValidated = false;
                            ValidateToLot();
                            //OnHandQuantityText.Text = Qty; //used for validate Qty
                        }
                    }
                    else
                    {
                        try
                        {
                            CSILocations SLLoc = new CSILocations(CSISystemContext);
                            SLLoc.UseSync(false);
                            SLLoc.AddProperty("Loc");
                            SLLoc.AddProperty("Description");
                            SLLoc.SetFilter(string.Format("Loc = N'{0}'", Loc));
                            SLLoc.LoadIDO();
                            if (SLLoc.CurrentTable.Rows.Count <= 0)
                            {
                                ToLocDescText.Text = string.Empty;
                                ToLocValidated = false;
                            }
                            else
                            {
                                ToLocEdit.Text = SLLoc.GetCurrentPropertyValueOfString("Loc"); ;
                                ToLocDescText.Text = SLLoc.GetCurrentPropertyValueOfString("Description"); ;
                                ToLocValidated = true;
                            }
                        }
                        catch (Exception Ex)
                        {
                            WriteErrorLog(Ex);
                            ToLocValidated = false;
                        }
                        
                    }
                }
            }
            EnableDisableComponents();
            return ToLocValidated;
        }

        private bool ValidateToLot()
        {
            if (!ToLotValidated)
            {
                if (string.IsNullOrEmpty(ToLotEdit.Text))
                {
                    ToLotValidated = false;
                }
                else
                {
                    ToLotValidated = true;
                }
            }
            EnableDisableComponents();
            return ToLotValidated;
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
            FromLotLinearLayout.Visibility = LotTracked ? ViewStates.Visible : ViewStates.Gone;
            ToLotLinearLayout.Visibility = LotTracked ? ViewStates.Visible : ViewStates.Gone;
            FromLotScanButton.Enabled = LotTracked;
            ToLotScanButton.Enabled = LotTracked;
            SNButton.Visibility = SNTracked ? ViewStates.Visible : ViewStates.Gone;
            SNButton.Enabled = SNTracked;
            ProcessButton.Enabled = ItemValidated && UMValidated && QtyValidated
                && FromLocValidated && ((LotTracked && FromLotValidated) || !LotTracked) 
                && ToLocValidated && ((LotTracked && ToLotValidated) || !LotTracked)  
                && ((SNTracked && SNPicked) || !SNTracked);
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
            bool rtn = CSIDcJsonObjects.ReadQtyMoveJson(Result, out string Item, out string UM, out string Qty, out string Loc1, out string Lot1, out string Loc2, out string Lot2);
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
                if (!string.IsNullOrEmpty(Loc1))
                {
                    FromLocEdit.Text = Loc1;
                    FromLocValidated = false;
                    ValidateFromLoc();
                }
                if (!string.IsNullOrEmpty(Lot1))
                {
                    FromLotEdit.Text = Lot1;
                    FromLotValidated = false;
                    ValidateFromLot();
                }
                if (!string.IsNullOrEmpty(Loc2))
                {
                    ToLocEdit.Text = Loc2;
                    ToLocValidated = false;
                    ValidateToLoc();
                }
                if (!string.IsNullOrEmpty(Lot2))
                {
                    ToLotEdit.Text = Lot2;
                    ToLotValidated = false;
                    ValidateToLot();
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
            return ValidateItem() && ValidateUM() && ValidateQty() && ValidateFromLoc() && ValidateFromLot() && ValidateToLoc() && ValidateToLot();
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

                DCQuantityMoveFragment QtyMoveDialog = (DCQuantityMoveFragment)activity.FragmentManager.FindFragmentByTag("QtyMove");
                if (QtyMoveDialog != null)
                {
                    ft.Show(QtyMoveDialog);
                    //ft.AddToBackStack(null);
                }
                else
                {
                    // Create and show the dialog.
                    QtyMoveDialog = new DCQuantityMoveFragment(activity);
                    //Add fragment
                    QtyMoveDialog.Show(ft, "QtyMove");
                }
            }
            catch (Exception Ex)
            {
                CSIErrorLog.WriteErrorLog(Ex);
            }
        }
        
    }
}