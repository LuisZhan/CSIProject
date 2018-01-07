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
using Android.Support.V4.App;
using CSIMobile.Class.Common;
using CSIMobile.Class.Fragments;
using CSIMobile.Class.Activities;

namespace CSIMobile.Class.Fragments.Adapter
{
    [BroadcastReceiver]
    public class ModuleDeckAdapter : CSIBaseFragmentPagerAdapter
    {
        public ModuleDeck ModuleDecks;

        public ModuleDeckAdapter(Android.Support.V4.App.FragmentManager fm, ModuleDeck ModuleDecks) : base(fm)
        {
            this.ModuleDecks = ModuleDecks;
        }
        
        // Returns the number of cards in the deck:
        public override int Count
        {
            get { return ModuleDecks.NumModules; }
        }

        // Returns a new fragment for the flash card at this position:
        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return (Android.Support.V4.App.Fragment)ModuleDeckFragment.NewInstance(ModuleDecks[position]);
        }

        // Display the problem number in the PagerTitleStrip:
        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String("Module: " + (position + 1));
        }
    }
}