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
using CSIMobile.Class.Business;

namespace CSIMobile.Class.Fragments
{
    public class ChangeWarehouseFragment : CSIBaseDialogFragment
    {
        private TextView WhseNameText;
        private Spinner WhseSpinner;
        private Dictionary<string, string> Whses = new Dictionary<string, string>();

        public ChangeWarehouseFragment(CSIBaseActivity activity = null) : base(activity)
        {
            Title = Application.Context.GetString(Resource.String.ChangeWarehouse);

            CSISystemContext.ReadConfigurations();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                base.OnCreateView(inflater, container, savedInstanceState);

                var view = inflater.Inflate(Resource.Layout.CSIChangeWarehouse, container, false);
                
                WhseSpinner = view.FindViewById<Spinner>(Resource.Id.WhseSpinner);
                WhseNameText = view.FindViewById<TextView>(Resource.Id.WhseNameText);
                
                WhseSpinner.ItemSelected += (o,e) =>
                {
                    string whse = (string)WhseSpinner.SelectedItem ?? string.Empty;
                    CSISystemContext.DefaultWarehouse = whse;
                    WhseNameText.Text = Whses.GetValueOrDefault(whse);
                };

                if (!string.IsNullOrEmpty(CSISystemContext.Token))
                {
                    SetWhseSpin();
                }
                return view;
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return null;
            }
        }

        private void SetWhseSpin()
        {
            string DeftWhse = CSISystemContext.DefaultWarehouse, Whse = "", Name = "";
            int index = 0, i = 0;
            ArrayAdapter adapter = new ArrayAdapter(Application.Context, Android.Resource.Layout.SimpleSpinnerItem);
            try
            {
                CSIWhses SLWhses = new CSIWhses(CSISystemContext);
                SLWhses.SetRecordCap(-1);
                SLWhses.UseAsync(false);
                SLWhses.LoadIDO();
                if (SLWhses.CurrentTable.Rows.Count > 0)
                {
                    for (i = 0;i< SLWhses.CurrentTable.Rows.Count; i++)
                    {
                        Whse = SLWhses.GetPropertyValue(i,"Whse").ToString();
                        Name = SLWhses.GetPropertyValue(i,"Name").ToString();
                        Whses.Add(Whse, Name);
                        adapter.Add(Whse);
                        if (Whse == DeftWhse)
                        {
                            WhseNameText.Text = Name;
                            index = i;
                        }
                    }
                    adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                    WhseSpinner.Adapter = adapter;
                    WhseSpinner.SetSelection(index);
                }
                else
                {
                    Whses.Add(DeftWhse, DeftWhse);
                    adapter.Add(DeftWhse);
                }
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                WhseSpinner.Adapter = adapter;
                WhseSpinner.SetSelection(index);
            }
        }
    }
}