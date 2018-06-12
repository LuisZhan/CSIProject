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
using Android.Support.V7.Widget;

namespace CSIMobile.Class.Fragments
{
    public class ItemListFragment : CSIBaseDialogFragment
    {
        private Android.Support.V7.Widget.ActionMenuView MenuView;
        private ListView ItemListView;

        //public event CreateSessionTokenCompletedEventHandler CreateSessionTokenCompleted;
        
        public ItemListFragment() : base()
        {
        }

        public ItemListFragment(CSIBaseActivity activity = null) : base(activity)
        {
            Title = Application.Context.GetString(Resource.String.Items);

            try
            {

            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                base.OnCreateView(inflater, container, savedInstanceState);

                var view = inflater.Inflate(Resource.Layout.CSItemList, container, false);

                MenuView = view.FindViewById<Android.Support.V7.Widget.ActionMenuView>(Resource.Id.MenuView);
                ItemListView = view.FindViewById<ListView>(Resource.Id.ItemListView);

                MenuView.MenuItemClick += MenuView_MenuItemClick;
                MenuView.Menu.Add("Test");
                return view;
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return null;
            }
        }

        private void MenuView_MenuItemClick(object sender, Android.Support.V7.Widget.ActionMenuView.MenuItemClickEventArgs e)
        {
            return;
        }

        public static void RunFragment(CSIBaseActivity activity)
        {
            try
            {
                FragmentTransaction ft = activity.FragmentManager.BeginTransaction();

                ItemListFragment ItemListFragmentDialog = (ItemListFragment)activity.FragmentManager.FindFragmentByTag("Items");
                if (ItemListFragmentDialog != null)
                {
                    ft.Show(ItemListFragmentDialog);
                    //ft.AddToBackStack(null);
                }
                else
                {
                    // Create and show the dialog.
                    ItemListFragmentDialog = new ItemListFragment(activity);
                    //Add fragment
                    ItemListFragmentDialog.Show(ft, "Items");
                }
            }
            catch (Exception Ex)
            {
                CSIErrorLog.WriteErrorLog(Ex);
            }
        }
        
    }
}