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
    public class DCOrderReturnFragment : CSIBaseDialogFragment
    {
        CSIDccos SLDccos;

        ImageButton CoScanButton;
        ImageButton UMScanButton;
        ImageButton QtyScanButton;
        ImageButton LocScanButton;
        ImageButton LotScanButton;
        ImageButton ReasonCodeScanButton;
        EditText WhseEdit;
        TextView TransDateText;
        EditText CoNumEdit;
        EditText LineEdit;
        EditText ReleaseEdit;
        TextView CustomerText;
        EditText QtyEdit;
        EditText UMEdit;
        EditText LocEdit;
        EditText LotEdit;
        EditText ReasonCodeEdit;
        TextView ReasonDescText;
        TextView ItemText;
        TextView ItemDescText;
        TextView ItemUMText;
        TextView QuantityOrderedText;
        TextView LocDescText;
        LinearLayout QtyLinearLayout;
        LinearLayout LocationLinearLayout;
        LinearLayout LotLinearLayout;
        Button SNButton;
        Button ProcessButton;

        ImageView CloseImage;
        ProgressBar ProgressBar;
        LinearLayout Layout;

        bool LotTracked = false, SNTracked = false;
        string CoType = "R";

        bool CoNumValidated = false, LineValidated = false, UMValidated = false, QtyValidated = false, ReleaseValidated = false, LocValidated = false, LotValidated = false, ReasonCodeValidated = false;
        List<string> SNs = new List<string>();
        bool SNPicked = true;

        private int ProcessCount = 0;

        public DCOrderReturnFragment(CSIBaseActivity activity = null) : base(activity)
        {
            CSISystemContext.ReadConfigurations();
            SLDccos = new CSIDccos(CSISystemContext);
            SLDccos.AddProperty("TransNum");
            SLDccos.AddProperty("TransType");
            SLDccos.AddProperty("Stat");
            SLDccos.AddProperty("Termid");
            SLDccos.AddProperty("TransDate");
            SLDccos.AddProperty("EmpNum");
            SLDccos.AddProperty("CoNum");
            SLDccos.AddProperty("CoLine");
            SLDccos.AddProperty("CoRelease");
            SLDccos.AddProperty("Item");
            SLDccos.AddProperty("UM");
            SLDccos.AddProperty("Whse");
            SLDccos.AddProperty("Loc");
            SLDccos.AddProperty("Lot");
            SLDccos.AddProperty("QtyShipped");
            SLDccos.AddProperty("QtyReturned");
            SLDccos.AddProperty("ReasonCode");
            SLDccos.AddProperty("DocumentNum");
            SLDccos.AddProperty("ErrorMessage");

            SLDccos.SetFilter("1=0");
            SLDccos.LoadIDO();
            SLDccos.SaveDataSetCompleted += SLDccos_SaveDataSetCompleted;
            SLDccos.LoadDataSetCompleted += SLDccos_LoadDataSetCompleted;
            SLDccos.CallMethodCompleted += SLDccos_CallMethodCompleted;
        }

        private void SLDccos_SaveDataSetCompleted(object sender, SaveDataSetCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    //check result status
                    if (SLDccos.CurrentTable.Rows.Count <= 0)
                    {
                        //nothing happen or just delete rows
                    }
                    else
                    {
                        string RowStatus = SLDccos.GetCurrentPropertyValueOfString("Stat");
                        string ErrorMessage = SLDccos.GetCurrentPropertyValueOfString("ErrorMessage");

                        if ((RowStatus != "E") || string.IsNullOrEmpty(ErrorMessage))
                        {
                            if (CSISystemContext.ForceAutoPost)
                            {
                                //Ready to Post -- calling DccoPSp
                                ShowProgressBar(true);
                                string strParmeters = "";
                                strParmeters = CSIBaseInvoker.BuildXMLParameters(strParmeters, "0");//From EDI
                                strParmeters = CSIBaseInvoker.BuildXMLParameters(strParmeters, "");
                                strParmeters = CSIBaseInvoker.BuildXMLParameters(strParmeters, "");
                                strParmeters = CSIBaseInvoker.BuildXMLParameters(strParmeters, "", true);
                                //strParmeters = CSIBaseInvoker.BuildXMLParameters(strParmeters, "");//DocumentNum 
                                SLDccos.InvokeMethod("DccoPSp", strParmeters);
                            }
                            else
                            {
                                //Clear Result if no error.
                                Initialize();
                            }
                        }
                        else
                        {
                            //delete first before prompt message.
                            SLDccos.CurrentTable.Rows[0].Delete();
                            ShowProgressBar(true);
                            SLDccos.DeleteIDO();

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

        private void SLDccos_CallMethodCompleted(object sender, CallMethodCompletedEventArgs e)
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
                        SLDccos.CurrentTable.Rows[0].Delete();
                        ShowProgressBar(true);
                        SLDccos.DeleteIDO();
                        WriteErrorLog(new Exception(CSIBaseInvoker.GetXMLParameters(e.strMethodParameters,1)));
                    }
                }
                else
                {
                    //try to delete post
                    SLDccos.CurrentTable.Rows[0].Delete();
                    ShowProgressBar(true);
                    SLDccos.DeleteIDO();
                    WriteErrorLog(e.Error);
                }
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
            ShowProgressBar(false);
        }

        private void SLDccos_LoadDataSetCompleted(object sender, LoadDataSetCompletedEventArgs e)
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

                var view = inflater.Inflate(Resource.Layout.CSIOrderReturn, container, false);
                Cancelable = false;
                
                WhseEdit = view.FindViewById<EditText>(Resource.Id.WhseEdit);
                TransDateText = view.FindViewById<TextView>(Resource.Id.TransDateText);
                CoScanButton = view.FindViewById<ImageButton>(Resource.Id.CoScanButton);
                CoNumEdit = view.FindViewById<EditText>(Resource.Id.CoNumEdit);
                LineEdit = view.FindViewById<EditText>(Resource.Id.LineEdit);
                QtyScanButton = view.FindViewById<ImageButton>(Resource.Id.QtyScanButton);
                QtyEdit = view.FindViewById<EditText>(Resource.Id.QtyEdit);
                UMScanButton = view.FindViewById<ImageButton>(Resource.Id.UMScanButton);
                UMEdit = view.FindViewById<EditText>(Resource.Id.UMEdit);
                ReleaseEdit = view.FindViewById<EditText>(Resource.Id.ReleaseEdit);
                LocScanButton = view.FindViewById<ImageButton>(Resource.Id.LocScanButton);
                LocEdit = view.FindViewById<EditText>(Resource.Id.LocEdit);
                LotScanButton = view.FindViewById<ImageButton>(Resource.Id.LotScanButton);
                LotEdit = view.FindViewById<EditText>(Resource.Id.LotEdit);
                ReasonCodeEdit = view.FindViewById<EditText>(Resource.Id.ReasonCodeEdit);
                ReasonCodeScanButton = view.FindViewById<ImageButton>(Resource.Id.ReasonCodeScanButton);
                ReasonDescText = view.FindViewById<TextView>(Resource.Id.ReasonDescText);

                QtyLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.QtyLinearLayout);
                LocationLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.LocationLinearLayout);
                LotLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.LotLinearLayout);

                SNButton = view.FindViewById<Button>(Resource.Id.SNButton);
                ProcessButton = view.FindViewById<Button>(Resource.Id.ProcessButton);
                Layout = view.FindViewById<LinearLayout>(Resource.Id.LinearLayout);

                CustomerText = view.FindViewById<TextView>(Resource.Id.CustomerText);
                ItemText = view.FindViewById<TextView>(Resource.Id.ItemText);
                ItemDescText = view.FindViewById<TextView>(Resource.Id.ItemDescText);
                ItemUMText = view.FindViewById<TextView>(Resource.Id.ItemUMText);
                QuantityOrderedText = view.FindViewById<TextView>(Resource.Id.QuantityOrderedText);
                LocDescText = view.FindViewById<TextView>(Resource.Id.LocDescText);

                CloseImage = view.FindViewById<ImageView>(Resource.Id.CloseImage);
                ProgressBar = view.FindViewById<ProgressBar>(Resource.Id.ProgressBar);

                CoScanButton.Click += CoScanButton_Click;
                QtyScanButton.Click += QtyScanButton_Click;
                UMScanButton.Click += UMScanButton_Click;
                LocScanButton.Click += LocScanButton_Click;
                LotScanButton.Click += LotScanButton_Click;
                ReasonCodeScanButton.Click += ReasonCodeScanButton_Click;

                CoNumEdit.FocusChange += CoNumEdit_FocusChange;
                LineEdit.FocusChange += LineEdit_FocusChange;
                ReleaseEdit.FocusChange += ReleaseEdit_FocusChange;
                QtyEdit.FocusChange += QtyEdit_FocusChange;
                UMEdit.FocusChange += UMEdit_FocusChange;
                LocEdit.FocusChange += LocEdit_FocusChange;
                LotEdit.FocusChange += LotEdit_FocusChange;
                ReasonCodeEdit.FocusChange += ReasonCodeEdit_FocusChange;

                CoNumEdit.KeyPress += CoNumEdit_KeyPress;
                LineEdit.KeyPress += LineEdit_KeyPress;
                ReleaseEdit.KeyPress += ReleaseEdit_KeyPress;
                QtyEdit.KeyPress += QtyEdit_KeyPress;
                UMEdit.KeyPress += UMEdit_KeyPress;
                LocEdit.KeyPress += LocEdit_KeyPress;
                LotEdit.KeyPress += LotEdit_KeyPress;
                ReasonCodeEdit.KeyPress += ReasonCodeEdit_KeyPress;

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

        private void ReasonCodeEdit_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Enter || e.KeyCode == Keycode.Tab)
            {
                if (e.Event.Action == KeyEventActions.Up)
                {
                    ValidateReasonCode();
                    ProcessButton.RequestFocus();
                }
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
                ReasonCodeValidated = false;
            }
        }

        private void LotEdit_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Enter || e.KeyCode == Keycode.Tab)
            {
                if (e.Event.Action == KeyEventActions.Up)
                {
                    ValidateLine();
                    ReasonCodeEdit.RequestFocus();
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
                    if (LotTracked)
                    {
                        ValidateLoc();
                        LotEdit.RequestFocus();
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
                LocValidated = false;
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

        private void ReleaseEdit_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Enter || e.KeyCode == Keycode.Tab)
            {
                if (e.Event.Action == KeyEventActions.Up)
                {
                    ValidateRelease();
                    UMEdit.RequestFocus();
                }
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
                ReleaseValidated = false;
            }
        }

        private void LineEdit_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Enter || e.KeyCode == Keycode.Tab)
            {
                if (e.Event.Action == KeyEventActions.Up)
                {
                    ValidateLine();
                    if (CoType == "R")
                    {
                        UMEdit.RequestFocus();
                    }
                    else
                    {
                        ReleaseEdit.RequestFocus();
                    }
                }
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
                LineValidated = false;
            }
        }

        private void CoNumEdit_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Enter || e.KeyCode == Keycode.Tab)
            {
                if (e.Event.Action == KeyEventActions.Up)
                {
                    ValidateCoNum();
                    LineEdit.RequestFocus();
                }
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
                CoNumValidated = false;
            }
        }

        private void Initialize()
        {
            WhseEdit.Text = CSISystemContext.DefaultWarehouse;
            TransDateText.Text = string.Format("{0} {1}",DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
            CoNumEdit.Text = string.Empty;
            LineEdit.Text = string.Empty;
            ReleaseEdit.Text = string.Empty;            
            QtyEdit.Text = "0";
            UMEdit.Text = string.Empty;
            LocEdit.Text = string.Empty;
            LotEdit.Text = string.Empty;
            ReasonCodeEdit.Text = string.Empty;
            ReasonDescText.Text = string.Empty;

            CustomerText.Text = string.Empty;
            ItemText.Text = string.Empty;
            ItemDescText.Text = string.Empty;
            ItemUMText.Text = string.Empty;
            QuantityOrderedText.Text = string.Empty;
            LocDescText.Text = string.Empty;

            CoNumValidated = false;
            LineValidated = false;
            UMValidated = false;
            QtyValidated = false;
            ReleaseValidated = false;
            LocValidated = false;
            LotValidated = false;
            ReasonCodeValidated = false;

            SetSNLabel();
        }

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            PerformValidation();
            if (CoNumValidated && LineValidated && ReleaseValidated && UMValidated && QtyValidated && LocValidated && ReasonCodeValidated && (LotValidated || !LotTracked) && SNPicked)
            {
                SLDccos.CurrentTable.Rows.Clear();
                DataRow Row = SLDccos.CurrentTable.NewRow();
                Row["TransNum"] = 0;//TransNum
                Row["TransType"] = "1";//TransType 1:Ship 2:CR Return
                Row["Stat"] = "U";//Stat
                Row["Termid"] = CSISystemContext.AndroidId.Substring(CSISystemContext.AndroidId.Length - 4, 4);//Termid
                Row["TransDate"] = DateTime.Now;//TransDate
                Row["Whse"] = CSISystemContext.DefaultWarehouse;//Whse
                Row["EmpNum"] = CSISystemContext.EmpNum;//EmpNum
                Row["CoNum"] = CoNumEdit.Text;//CoNum
                Row["CoLine"] = LineEdit.Text;//CoLine
                Row["CoRelease"] = ReleaseEdit.Text;//Release
                Row["Item"] = ItemText.Text;//Item
                Row["QtyShipped"] = "-" + QtyEdit.Text;//QtyShipped
                Row["UM"] = UMEdit.Text;//UM
                Row["Loc"] = LocEdit.Text;//Loc
                Row["Lot"] = LotEdit.Text;//Lot
                Row["ReasonCode"] = ReasonCodeEdit.Text;//ReasonCode
                SLDccos.CurrentTable.Rows.Add(Row);
                //Row.BeginEdit();
                //Row.EndEdit();
                //Row.AcceptChanges();
                SLDccos.InsertIDO();
                ShowProgressBar(true);
            }
        }

        private void SNButton_Click(object sender, EventArgs e)
        {
            SerialGenerator.RunFragment(this);
        }

        private void ReasonCodeEdit_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {//gain focus
                ReasonCodeEdit.SelectAll();
            }
            else
            {//lose focus
                ValidateReasonCode();
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

        private void ReleaseEdit_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {//gain focus
                ReleaseEdit.SelectAll();
            }
            else
            {//lose focus
                ValidateRelease ();
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

        private void LineEdit_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {//gain focus
                LineEdit.SelectAll();
            }
            else
            {//lose focus
                ValidateLine();
            }
        }

        private void CoNumEdit_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {//gain focus
                CoNumEdit.SelectAll();
            }
            else
            {//lose focus
                ValidateCoNum();
            }
        }

        private bool ValidateCoNum()
        {
            if (!CoNumValidated)
            {
                if (string.IsNullOrEmpty(CoNumEdit.Text))
                {
                    CoNumValidated = false;
                }
                else
                {
                    try
                    {
                        string CoNum = CoNumEdit.Text, Line = LineEdit.Text, Release = ReleaseEdit.Text, Customer = CustomerText.Text
                            , Item = ItemText.Text, ItemDesc = ItemDescText.Text, UM = ItemUMText.Text
                            , QuantityOrdered = QuantityOrderedText.Text, QtyShipped = "", QtyToBeShipped = "";

                        //validate CoNum and Line, Release
                        CoNumValidated = CSICoItems.GetCoNumInfor(CSISystemContext, ref CoNum, ref Line, ref Release, ref CoType, ref Customer, ref Item
                            , ref ItemDesc, ref UM, ref QuantityOrdered, ref QtyShipped, ref QtyToBeShipped, ref LotTracked, ref SNTracked);
                        if (CoNumValidated == true)
                        {
                            CoNumEdit.Text = CoNum;
                            LineEdit.Text = Line;
                            LineValidated = true;
                            ReleaseEdit.Text = Release;
                            ReleaseValidated = true;
                            CustomerText.Text = Customer;
                            ItemText.Text = Item;
                            ItemDescText.Text = ItemDesc;
                            ItemUMText.Text = UM;
                            if (string.IsNullOrEmpty(UMEdit.Text))
                            {
                                UMEdit.Text = UM;
                                UMValidated = true;
                            }
                            QuantityOrderedText.Text = QuantityOrdered;

                            if (CoType == "R")
                            {
                                ReleaseEdit.Enabled = false;
                            }
                            else
                            {
                                ReleaseEdit.Enabled = true;
                            }

                            //Validate ItemLoc
                            string Loc = LocEdit.Text, LocDescription = "", Qty = "";
                            bool RtnCSIItemLocs = CSIItemLocs.GetItemLocInfor(CSISystemContext, ItemText.Text, WhseEdit.Text, ref Loc, ref LocDescription, ref Qty);
                            if (RtnCSIItemLocs == true)
                            {
                                LocEdit.Text = Loc;
                                LocDescText.Text = LocDescription;
                                LocValidated = false;
                                ValidateLoc();
                            }
                        }
                        else
                        {
                            CustomerText.Text = string.Empty;
                            ItemText.Text = string.Empty;
                            ItemDescText.Text = string.Empty;
                            ItemUMText.Text = string.Empty;
                            QuantityOrderedText.Text = string.Empty;
                            LotTracked = false;
                            SNTracked = false;

                            LineValidated = false;
                            ReleaseValidated = false;
                        }

                    }
                    catch (Exception Ex)
                    {
                        WriteErrorLog(Ex);
                        CoNumValidated = false;
                        LineValidated = false;
                        ReleaseValidated = false;
                    }
                }
            }
            EnableDisableComponents();
            return CoNumValidated;
        }

        private bool ValidateLine()
        {
            if (!LineValidated)
            {
                if (string.IsNullOrEmpty(LineEdit.Text))
                {
                    LineValidated = false;
                }
                else
                {
                    try
                    {
                        string CoNum = CoNumEdit.Text, Line = LineEdit.Text, Release = ReleaseEdit.Text, Customer = CustomerText.Text
                            , Item = ItemText.Text, ItemDesc = ItemDescText.Text, UM = ItemUMText.Text
                            , QuantityOrdered = QuantityOrderedText.Text, QtyShipped = "", QtyToBeShipped = "";

                        //validate CoNum and Line, Release
                        CoNumValidated = CSICoItems.GetCoNumInfor(CSISystemContext, ref CoNum, ref Line, ref Release, ref CoType, ref Customer, ref Item
                            , ref ItemDesc, ref UM, ref QuantityOrdered, ref QtyShipped, ref QtyToBeShipped, ref LotTracked, ref SNTracked);
                        if (CoNumValidated == true)
                        {
                            CoNumEdit.Text = CoNum;
                            LineEdit.Text = Line;
                            LineValidated = true;
                            ReleaseEdit.Text = Release;
                            ReleaseValidated = true;
                            CustomerText.Text = Customer;
                            ItemText.Text = Item;
                            ItemDescText.Text = ItemDesc;
                            ItemUMText.Text = UM;
                            if (string.IsNullOrEmpty(UMEdit.Text))
                            {
                                UMEdit.Text = UM;
                                UMValidated = true;
                            }
                            QuantityOrderedText.Text = QuantityOrdered;

                            if (CoType == "R")
                            {
                                ReleaseEdit.Enabled = false;
                            }
                            else
                            {
                                ReleaseEdit.Enabled = true;
                            }

                            //Validate ItemLoc
                            string Loc = LocEdit.Text, LocDescription = "", Qty = "";
                            bool RtnCSIItemLocs = CSIItemLocs.GetItemLocInfor(CSISystemContext, ItemText.Text, WhseEdit.Text, ref Loc, ref LocDescription, ref Qty);
                            if (RtnCSIItemLocs == true)
                            {
                                LocEdit.Text = Loc;
                                LocDescText.Text = LocDescription;
                                LocValidated = false;
                                ValidateLoc();
                            }
                        }
                        else
                        {
                            CustomerText.Text = string.Empty;
                            ItemText.Text = string.Empty;
                            ItemDescText.Text = string.Empty;
                            ItemUMText.Text = string.Empty;
                            QuantityOrderedText.Text = string.Empty;
                            LotTracked = false;
                            SNTracked = false;

                            LineValidated = false;
                            ReleaseValidated = false;
                        }

                    }
                    catch (Exception Ex)
                    {
                        WriteErrorLog(Ex);
                        CoNumValidated = false;
                        LineValidated = false;
                        ReleaseValidated = false;
                    }
                }
            }
            EnableDisableComponents();
            return LineValidated;
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

        private bool ValidateRelease()
        {
            if (!ReleaseValidated)
            {
                if (string.IsNullOrEmpty(ReleaseEdit.Text))
                {
                    ReleaseValidated = false;
                }
                else
                {
                    try
                    {
                        string CoNum = CoNumEdit.Text, Line = LineEdit.Text, Release = ReleaseEdit.Text, Customer = CustomerText.Text
                            , Item = ItemText.Text, ItemDesc = ItemDescText.Text, UM = ItemUMText.Text
                            , QuantityOrdered = QuantityOrderedText.Text, QtyShipped = "", QtyToBeShipped = "";

                        //validate CoNum and Line, Release
                        CoNumValidated = CSICoItems.GetCoNumInfor(CSISystemContext, ref CoNum, ref Line, ref Release, ref CoType, ref Customer, ref Item
                            , ref ItemDesc, ref UM, ref QuantityOrdered, ref QtyShipped, ref QtyToBeShipped, ref LotTracked, ref SNTracked);
                        if (CoNumValidated == true)
                        {
                            CoNumEdit.Text = CoNum;
                            LineEdit.Text = Line;
                            LineValidated = true;
                            ReleaseEdit.Text = Release;
                            ReleaseValidated = true;
                            CustomerText.Text = Customer;
                            ItemText.Text = Item;
                            ItemDescText.Text = ItemDesc;
                            ItemUMText.Text = UM;
                            if (string.IsNullOrEmpty(UMEdit.Text))
                            {
                                UMEdit.Text = UM;
                                UMValidated = true;
                            }
                            QuantityOrderedText.Text = QuantityOrdered;

                            if (CoType == "R")
                            {
                                ReleaseEdit.Enabled = false;
                            }
                            else
                            {
                                ReleaseEdit.Enabled = true;
                            }

                            //Validate ItemLoc
                            string Loc = LocEdit.Text, LocDescription = "", Qty = "";
                            bool RtnCSIItemLocs = CSIItemLocs.GetItemLocInfor(CSISystemContext, ItemText.Text, WhseEdit.Text, ref Loc, ref LocDescription, ref Qty);
                            if (RtnCSIItemLocs == true)
                            {
                                LocEdit.Text = Loc;
                                LocDescText.Text = LocDescription;
                                LocValidated = false;
                                ValidateLoc();
                            }
                        }
                        else
                        {
                            CustomerText.Text = string.Empty;
                            ItemText.Text = string.Empty;
                            ItemDescText.Text = string.Empty;
                            ItemUMText.Text = string.Empty;
                            QuantityOrderedText.Text = string.Empty;
                            LotTracked = false;
                            SNTracked = false;

                            LineValidated = false;
                            ReleaseValidated = false;
                        }

                    }
                    catch (Exception Ex)
                    {
                        WriteErrorLog(Ex);
                        CoNumValidated = false;
                        LineValidated = false;
                        ReleaseValidated = false;
                    }
                }
            }
            EnableDisableComponents();
            return ReleaseValidated;
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
                    LocValidated = CSIItemLocs.GetItemLocInfor(CSISystemContext, CoNumEdit.Text, WhseEdit.Text, ref Loc, ref LocDescription, ref Qty);
                    if (LocValidated)
                    {
                        LocDescText.Text = LocDescription;
                    }
                    else
                    {
                        try
                        {
                            CSILocations SLLoc = new CSILocations(CSISystemContext);
                            SLLoc.UseAsync(false);
                            SLLoc.AddProperty("Loc");
                            SLLoc.AddProperty("Description");
                            SLLoc.SetFilter(string.Format("Loc = N'{0}'", Loc));
                            SLLoc.LoadIDO();
                            if (SLLoc.CurrentTable.Rows.Count <= 0)
                            {
                                LocDescText.Text = string.Empty;
                                LocValidated = false;
                            }
                            else
                            {
                                LocEdit.Text = SLLoc.GetCurrentPropertyValueOfString("Loc"); ;
                                LocDescText.Text = SLLoc.GetCurrentPropertyValueOfString("Description"); ;
                                LocValidated = true;
                            }
                        }catch (Exception Ex)
                        {
                            WriteErrorLog(Ex);
                            LocValidated = false;
                        }
                        LocEdit.Text = Loc;
                        LocDescText.Text = LocDescription;
                        ValidateLot();
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
                    if (LotTracked)
                    {
                        /* Get Current Lot */
                        string Loc = LocEdit.Text, Lot = LotEdit.Text, Qty = "";
                        bool RtnCSILotLocs = CSILotLocs.GetItemLotLocInfor(CSISystemContext, CoNumEdit.Text, WhseEdit.Text, Loc, ref Lot, ref Qty);
                        if (RtnCSILotLocs)
                        {
                            LotEdit.Text = Lot;
                            LotValidated = true;
                        }
                        /* Get Next Lot */
                        //string Message = "", Key = "";
                        //LotValidated = CSIItems.GetNextLotSp(CSISystemContext, ItemText.Text, "", ref Message, ref Key);
                        //if (string.IsNullOrEmpty(Message))
                        //{
                        //    LotEdit.Text = Key;
                        //}
                        //else
                        //{
                        //    //LotValidated = false;
                        //}
                    }
                    else
                    {
                        LotValidated = true;
                    }
                }
                else
                {
                    LotValidated = true;
                }
            }
            EnableDisableComponents();
            return LotValidated;
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

        private bool ValidateReasonCode()
        {
            if (!ReasonCodeValidated)
            {
                if (string.IsNullOrEmpty(ReasonCodeEdit.Text))
                {
                    ReasonCodeValidated = false;
                }
                else
                {
                    string ReasonCode = ReasonCodeEdit.Text, ReasonDescription = "";
                    ReasonCodeValidated = CSIReasons.GetReason(CSISystemContext, ref ReasonCode, "CO RETURN", ref ReasonDescription);
                    if (ReasonCodeValidated)
                    {
                        ReasonCodeEdit.Text = ReasonCode;
                        ReasonDescText.Text = ReasonDescription;
                    }
                    else
                    {
                        ReasonDescText.Text = string.Empty;
                    }
                }
            }
            EnableDisableComponents();
            return ReasonCodeValidated;
        }

        private void EnableDisableComponents()
        {
            try
            {
                if (string.IsNullOrEmpty(CoNumEdit.Text))
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
                ProcessButton.Enabled = CoNumValidated && LineValidated && ReleaseValidated && UMValidated && QtyValidated
                    && LocValidated && ReasonCodeValidated && ((LotTracked && LotValidated) || !LotTracked) && ((SNTracked && SNPicked) || !SNTracked);
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
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

        private async void ReasonCodeScanButton_Click(object sender, EventArgs e)
        {
            string ScanResult = await CSISanner.ScanAsync();
            if (string.IsNullOrEmpty(ScanResult))
            {
                return;
            }
            if (!AnalysisScanResult(ScanResult))
            {
                ReasonCodeEdit.Text = ScanResult;
                if (!ValidateUM())
                {
                    ReasonCodeEdit.RequestFocus();
                }
                else
                {
                    ProcessButton.RequestFocus();
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
                    ReleaseEdit.RequestFocus();
                }
            }
        }

        private async void CoScanButton_Click(object sender, EventArgs e)
        {
            string ScanResult = await CSISanner.ScanAsync();
            if (string.IsNullOrEmpty(ScanResult))
            {
                return;
            }
            if (!AnalysisScanResult(ScanResult))
            {
                CoNumEdit.Text = ScanResult;
                if (!ValidateCoNum())
                {
                    CoNumEdit.RequestFocus();
                }
                else
                {
                    LineEdit.RequestFocus();
                }
            }
        }

        private bool AnalysisScanResult(string Result)
        {
            //this is designed for future scan enhancement, such as scan one code to fill in all stuff...
            bool rtn = CSIDcJsonObjects.ReadOrderShippingJson(Result, out string CoNum, out string Line, out string Release, out string UM, out string Qty, out string Loc, out string Lot,out string ReasonCode);
            if (rtn)
            {
                if (!string.IsNullOrEmpty(CoNum))
                {
                    CoNumEdit.Text = CoNum;
                    CoNumValidated = false;
                    ValidateCoNum();
                }
                if (!string.IsNullOrEmpty(Line))
                {
                    LineEdit.Text = Line;
                    LineValidated = false;
                    ValidateLine();
                }
                if (!string.IsNullOrEmpty(Release))
                {
                    ReleaseEdit.Text = Release;
                    ReleaseValidated = false;
                    ValidateRelease();
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
                if (!string.IsNullOrEmpty(ReasonCode))
                {
                    ReasonCodeEdit.Text = ReasonCode;
                    ReasonCodeValidated = false;
                    ValidateReasonCode();
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
            return ValidateCoNum() && ValidateLine() && ValidateQty() && ValidateRelease() && ValidateLoc() && ValidateLot() && ValidateReasonCode();
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

                DCOrderReturnFragment OrderReturnDialog = (DCOrderReturnFragment)activity.FragmentManager.FindFragmentByTag("OrderShipping");
                if (OrderReturnDialog != null)
                {
                    ft.Show(OrderReturnDialog);
                    //ft.AddToBackStack(null);
                }
                else
                {
                    // Create and show the dialog.
                    OrderReturnDialog = new DCOrderReturnFragment(activity);
                    //Add fragment
                    OrderReturnDialog.Show(ft, "OrderShipping");
                }
            }
            catch (Exception Ex)
            {
                CSIErrorLog.WriteErrorLog(Ex);
            }
        }
    }
}