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
    interface ICSIWebServiceEventInterface
    {        
        /// <remarks/>
        event GetConfigurationNamesCompletedEventHandler GetConfigurationNamesCompleted;

        /// <remarks/>
        event CreateSessionTokenCompletedEventHandler CreateSessionTokenCompleted;

        /// <remarks/>
        event LoadDataSetCompletedEventHandler LoadDataSetCompleted;

        /// <remarks/>
        event SaveDataSetCompletedEventHandler SaveDataSetCompleted;

        /// <remarks/>
        event CallMethodCompletedEventHandler CallMethodCompleted;

        ///// <remarks/>
        //event LoadJsonCompletedEventHandler LoadJsonCompleted;

        ///// <remarks/>
        //event SaveJsonCompletedEventHandler SaveJsonCompleted;
    }
}